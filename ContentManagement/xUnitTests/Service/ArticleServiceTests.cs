using Azure;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Domain.Enum;
using ContentAPI.Service;
using ContentAPI.Service.Interfaces;
using Moq;

namespace ContentAPI.Tests
{
    public class ArticleServiceTests
    {
        private readonly Mock<IArticleRepo<Article>> _articleRepoMock;
        private readonly Mock<IBlobStorageService> _blobServiceMock;
        private readonly IArticleService _articleService;
        private readonly string _mockRowKey;
        private readonly string _mockPartKey;

        public ArticleServiceTests()
        {
            _articleRepoMock = new Mock<IArticleRepo<Article>>();
            _blobServiceMock = new Mock<IBlobStorageService>();
            _articleService = new ArticleService(_articleRepoMock.Object, _blobServiceMock.Object);
            _mockRowKey = Guid.NewGuid().ToString();
            _mockPartKey = "News";
        }

        [Fact]
        public async Task Create_ShouldCreateNewArticle()
        {
            // Arrange
            var createArticleDTO = new CreateArticleDTO
            {
                ArticleType = ArticleType.News,
                Title = "Test Title",
                Subtitle = "Test Subtitle",
                MainText = "Test Main Text",
                Base64Images = new Dictionary<string, string>
                {
                    { "image1.jpg", "base64image1" },
                    { "image2.jpg", "base64image2" }
                }
            };

            // Act
            var createdArticle = await _articleService.Create(createArticleDTO);

            // Assert
            Assert.NotNull(createdArticle);
            Assert.Equal(createArticleDTO.ArticleType.ToString(), createdArticle.PartitionKey);
            Assert.NotNull(createdArticle.RowKey);
            Assert.Equal(createArticleDTO.Title, createdArticle.Title);
            Assert.Equal(createArticleDTO.Subtitle, createdArticle.Subtitle);
            Assert.Equal(createArticleDTO.MainText, createdArticle.MainText);

            _articleRepoMock.Verify(repo => repo.UpsertAsync(It.IsAny<Article>()), Times.Once);
            _blobServiceMock.Verify(service => service.StoreNewImagesAndRetrieveBlobUrls(It.IsAny<Dictionary<string, string>>(), It.IsAny<Article>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateArticle()
        {
            // Arrange
            var updateArticleDTO = new UpdateArticleDTO
            {
                PartitionKey = _mockPartKey,
                RowKey = _mockRowKey,
                ETag = new ETag(),
                Timestamp = DateTime.UtcNow,
                Title = "Updated Title",
                Subtitle = "Updated Subtitle",
                MainText = "Updated Main Text",
                OldSerializedImageURLs = "{\"image1.jpg\":\"https://mock.blob.url/image1.jpg\",\"image2.jpg\":\"https://mock.blob.url/image2.jpg\"}",
                Base64Images = new Dictionary<string, string>
                {
                    { "image1.jpg", "base64image1" },
                    { "image2.jpg", "base64image2" }
                }
            };

            // Act
            await _articleService.UpdateAsync(updateArticleDTO);

            // Assert
            _articleRepoMock.Verify(repo => repo.UpsertAsync(It.IsAny<Article>()), Times.Once);
            _blobServiceMock.Verify(service => service.UpdateImagesAndRetrieveBlobUrls(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<Article>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteArticle()
        {
            // Arrange
            var successResponse = new Mock<Response>();
            successResponse.SetupGet(r => r.Status).Returns(200);
            successResponse.SetupGet(r => r.IsError).Returns(false);

            var succesResponseTask = Task.FromResult(successResponse.Object);

            _articleRepoMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(succesResponseTask);

            // Act
            Response result = await _articleService.DeleteAsync(_mockPartKey, _mockRowKey);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsError);

            _articleRepoMock.Verify(repo => repo.DeleteAsync(_mockPartKey, _mockRowKey), Times.Once);
        }

        [Fact]
        public async Task GetByKeyAsync_ShouldReturnArticle()
        {
            // Arrange
            var partitionKey = "TestPartitionKey";
            var rowKey = "TestRowKey";
            var expectedArticle = new Article
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Title = "Test Title",
                Subtitle = "Test Subtitle",
                MainText = "Test Main Text"
            };

            _articleRepoMock.Setup(repo => repo.GetByKeyAsync(partitionKey, rowKey))
                .ReturnsAsync(expectedArticle);

            // Act
            var result = await _articleService.GetByKeyAsync(partitionKey, rowKey);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedArticle, result);
        }
    }
}