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
            var content = new Mock<ContentBase>();

            var newBase64Images = new Dictionary<string, string>
            {
                { "image1.jpg", "base64image1" },
                { "image2.jpg", "base64image2" }
            };

            // Act
            _blobStorageService.StoreNewImagesAndRetrieveBlobUrls(newBase64Images, content.Object);

            // Assert
            _blobStorageRepoMock.Verify(repo => repo.StoreJpgImage("image1.jpg", "base64image1", "partitionKeyrowKey/"), Times.Once);
            _blobStorageRepoMock.Verify(repo => repo.StoreJpgImage("image2.jpg", "base64image2", "partitionKeyrowKey/"), Times.Once);
        }

        [Fact]
        public void UpdateImagesAndRetrieveBlobUrls_ShouldCallBlobStorageRepo_StoreJpgImageAndDeleteImageForEachImage()
        {
            // Arrange
            var content = new Mock<ContentBase>();

            var oldImageURLs = JsonSerializer.Serialize(new Dictionary<string, string>
            {
                { "image1.jpg", "https://mock.blob.url/account/container/partitionKey-rowKey/image1.jpg" },
                { "image2.jpg", "https://mock.blob.url/account/container/partitionKey-rowKey/image2.jpg" }
            });

            var newBase64Images = new Dictionary<string, string>
            {
                { "image1.jpg", "newBase64image1" },
                { "image3.jpg", "newBase64image3" }
            };

            // Act
            _blobStorageService.UpdateImagesAndRetrieveBlobUrls(oldImageURLs, newBase64Images, content.Object);

            // Assert
            _blobStorageRepoMock.Verify(repo => repo.DeleteImage("partitionKeyrowKey/image1.jpg"), Times.Once);
            _blobStorageRepoMock.Verify(repo => repo.DeleteImage("partitionKeyrowKey/image2.jpg"), Times.Once);
            _blobStorageRepoMock.Verify(repo => repo.StoreJpgImage("image1.jpg", "newBase64image1", "partitionKeyrowKey/"), Times.Once);
            _blobStorageRepoMock.Verify(repo => repo.StoreJpgImage("image3.jpg", "newBase64image3", "partitionKeyrowKey/"), Times.Once);
        }
    }
}
