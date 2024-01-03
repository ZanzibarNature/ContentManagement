using Azure;

namespace ContentAPI.Domain.DTO
{
    public class UpdateLocationDTO
    {
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Dictionary<string, string>? Base64Images { get; set; }
        public string? OldSerializedImageURLs { get; set; }
    }
}
