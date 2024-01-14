using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Service;
using ContentAPI.Service.Interfaces;
using Moq;
using System.Text.Json;

namespace ContentAPI.Tests
{
    public class BlobStorageServiceTests
    {
        private readonly Mock<IBlobStorageRepo> _blobStorageRepoMock;
        private readonly IBlobStorageService _blobStorageService;

        public BlobStorageServiceTests()
        {
            _blobStorageRepoMock = new Mock<IBlobStorageRepo>();
            _blobStorageService = new BlobStorageService(_blobStorageRepoMock.Object);
        }

        [Fact]
        public void StoreJpgImage_ShouldCallBlobStorageRepo_StoreJpgImage()
        {
            // Arrange & Act
            _blobStorageService.StoreJpgImage("prefix", "image", "folder");

            // Assert
            _blobStorageRepoMock.Verify(repo => repo.StoreJpgImage("prefix", "image", "folder"), Times.Once);
        }

        [Fact]
        public void DeleteImage_ShouldCallBlobStorageRepo_DeleteImage()
        {
            // Arrange & Act
            _blobStorageService.DeleteImage("blobName");

            // Assert
            _blobStorageRepoMock.Verify(repo => repo.DeleteImage("blobName"), Times.Once);
        }

        [Fact]
        public void StoreNewImagesAndRetrieveBlobUrls_ShouldCallBlobStorageRepo_StoreJpgImageForEachImage()
        {
            // Arrange
            var content = new Mock<Article>();

            var newBase64Images = new Dictionary<string, string>
            {
                { "ImageKey1", "base64image1" },
                { "ImageKey2", "base64image2" }
            };

            _blobStorageRepoMock.Setup(repo => repo.StoreJpgImage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            // Act
            _blobStorageService.StoreNewImagesAndRetrieveBlobUrls(newBase64Images, content.Object);

            // Assert
            _blobStorageRepoMock.Verify(repo => repo.StoreJpgImage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void UpdateImagesAndRetrieveBlobUrls_ShouldCallBlobStorageRepo_StoreJpgImageAndDeleteImageForEachImage()
        {
            // Arrange
            var content = new Mock<Article>();

            var oldImageURLs = JsonSerializer.Serialize(new Dictionary<string, string>
            {
                { "BannerImage", "https://mock.blob.url/account/container/partitionKey-rowKey/image1.jpg" },
                { "AdditionalImage", "https://mock.blob.url/account/container/partitionKey-rowKey/image2.jpg" }
            });

            var newBase64Images = new Dictionary<string, string>
            {
                { "BannerImage", "newBase64image1" },
                { "AdditionalImage", "newBase64image2" }
            };

            // Act
            _blobStorageService.UpdateImagesAndRetrieveBlobUrls(oldImageURLs, newBase64Images, content.Object);

            // Assert
            _blobStorageRepoMock.Verify(repo => repo.DeleteImage(It.IsAny<string>()), Times.Exactly(2));
            _blobStorageRepoMock.Verify(repo => repo.StoreJpgImage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
