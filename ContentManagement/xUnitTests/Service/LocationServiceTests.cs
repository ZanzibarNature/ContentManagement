using Azure;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Domain.DTO;
using ContentAPI.Service;
using ContentAPI.Service.Interfaces;
using Moq;
using System.Text.Json;

namespace ContentAPI.Tests
{
    public class LocationServiceTests
    {
        private readonly Mock<ILocationRepo<Location>> _locationRepoMock;
        private readonly Mock<IBlobStorageService> _blobServiceMock;
        private readonly ILocationService _locationService;

        public LocationServiceTests()
        {
            _locationRepoMock = new Mock<ILocationRepo<Location>>();
            _blobServiceMock = new Mock<IBlobStorageService>();
            _locationService = new LocationService(_locationRepoMock.Object, _blobServiceMock.Object);
        }

        [Fact]
        public async Task Create_ShouldCreateLocationAndReturnLocationEntity()
        {
            // Arrange
            var createLocationDto = new CreateLocationDTO
            {
                Name = "Test Location",
                Description = "Test Description",
                Latitude = 40.7128,
                Longitude = -74.0060,
                InvolvementHighlight = "Test Highlight",
                GoogleMapsURL = "https://maps.google.com/test",
                Base64Images = new Dictionary<string, string>
                {
                    { "image1.jpg", "base64image1" },
                    { "image2.jpg", "base64image2" }
                },
                IconNames = new List<string> { "icon1", "icon2" }
            };

            // Act
            var result = await _locationService.Create(createLocationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Location", result.PartitionKey);
            Assert.NotNull(result.RowKey);
            Assert.Equal(createLocationDto.Name, result.Name);
            Assert.Equal(createLocationDto.Description, result.Description);
            Assert.Equal(createLocationDto.Latitude, result.Latitude);
            Assert.Equal(createLocationDto.Longitude, result.Longitude);
            Assert.Equal(createLocationDto.InvolvementHighlight, result.InvolvementHighlight);
            Assert.Equal(createLocationDto.GoogleMapsURL, result.GoogleMapsURL);
            _locationRepoMock.Verify(repo => repo.UpsertAsync(It.IsAny<Location>()), Times.Once);
            _blobServiceMock.Verify(service => service.StoreNewImagesAndRetrieveBlobUrls(It.IsAny<Dictionary<string, string>>(), It.IsAny<ContentBase>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateLocationAndReturnLocationEntity()
        {
            // Arrange
            var updateLocationDto = new UpdateLocationDTO
            {
                PartitionKey = "TestPartitionKey",
                RowKey = "TestRowKey",
                ETag = new ETag(),
                Timestamp = DateTime.UtcNow,
                Name = "Updated Location",
                Description = "Updated Description",
                Latitude = 37.7749,
                Longitude = -122.4194,
                InvolvementHighlight = "Updated Highlight",
                GoogleMapsURL = "https://maps.google.com/updated",
                OldSerializedImageURLs = JsonSerializer.Serialize(new Dictionary<string, string>
                {
                    { "image1.jpg", "https://mock.blob.url/image1.jpg" },
                    { "image2.jpg", "https://mock.blob.url/image2.jpg" }
                }),
                Base64Images = new Dictionary<string, string>
                {
                    { "image1.jpg", "newBase64image1" },
                    { "image3.jpg", "newBase64image3" }
                },
                IconNames = new List<string> { "icon1", "icon2" }
            };

            // Setup the DeleteImage method to return a successful response
            _blobServiceMock.Setup(service => service.DeleteImage(It.IsAny<string>()));

            // Setup the StoreJpgImage method to return a URL
            _blobServiceMock.Setup(service => service.StoreJpgImage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("https://mock.blob.url/updatedImage.jpg");

            //// Setup the UpsertAsync method to return a successful Response
            //var successResponse = new Mock<Response>();
            //successResponse.SetupGet(r => r.Status).Returns(200);
            //successResponse.SetupGet(r => r.IsError).Returns(false);
            //_locationRepoMock.Setup(repo => repo.UpsertAsync(It.IsAny<Location>()))
            //    .ReturnsAsync(successResponse.Object);

            // Act
            var result = await _locationService.UpdateAsync(updateLocationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateLocationDto.PartitionKey, result.PartitionKey);
            Assert.Equal(updateLocationDto.RowKey, result.RowKey);
            Assert.Equal(updateLocationDto.ETag, result.ETag);
            Assert.Equal(updateLocationDto.Timestamp, result.Timestamp);
            Assert.Equal(updateLocationDto.Name, result.Name);
            Assert.Equal(updateLocationDto.Description, result.Description);
            Assert.Equal(updateLocationDto.Latitude, result.Latitude);
            Assert.Equal(updateLocationDto.Longitude, result.Longitude);
            Assert.Equal(updateLocationDto.InvolvementHighlight, result.InvolvementHighlight);
            Assert.Equal(updateLocationDto.GoogleMapsURL, result.GoogleMapsURL);
            _blobServiceMock.Verify(service => service.DeleteImage("TestPartitionKeyTestRowKey/image1.jpg"), Times.Once);
            _blobServiceMock.Verify(service => service.DeleteImage("TestPartitionKeyTestRowKey/image2.jpg"), Times.Once);
            _blobServiceMock.Verify(service => service.StoreJpgImage("image1.jpg", "newBase64image1", "TestPartitionKeyTestRowKey/"), Times.Once);
            _blobServiceMock.Verify(service => service.StoreJpgImage("image3.jpg", "newBase64image3", "TestPartitionKeyTestRowKey/"), Times.Once);
            _locationRepoMock.Verify(repo => repo.UpsertAsync(It.IsAny<Location>()), Times.Once);
        }

        [Fact]
        public async Task GetByKeyAsync_ShouldReturnLocationEntity()
        {
            // Arrange
            var partitionKey = "TestPartitionKey";
            var rowKey = "TestRowKey";
            var expectedLocationEntity = new Location
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Name = "Test Location",
                Description = "Test Description",
                Latitude = 40.7128,
                Longitude = -74.0060,
                InvolvementHighlight = "Test Highlight",
                GoogleMapsURL = "https://maps.google.com/test"
            };

            _locationRepoMock.Setup(repo => repo.GetByKeyAsync(partitionKey, rowKey))
                .ReturnsAsync(expectedLocationEntity);

            // Act
            var result = await _locationService.GetByKeyAsync(partitionKey, rowKey);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLocationEntity, result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteLocation()
        {
            // Arrange
            var partitionKey = "TestPartitionKey";
            var rowKey = "TestRowKey";

            // Setup the DeleteAsync method to return a successful Response
            var successResponse = new Mock<Response>();
            successResponse.SetupGet(r => r.Status).Returns(200);
            successResponse.SetupGet(r => r.IsError).Returns(false);
            _locationRepoMock.Setup(repo => repo.DeleteAsync(partitionKey, rowKey))
                .ReturnsAsync(successResponse.Object);

            // Act
            var result = await _locationService.DeleteAsync(partitionKey, rowKey);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsError);
            _locationRepoMock.Verify(repo => repo.DeleteAsync(partitionKey, rowKey), Times.Once);
        }
    }
}
