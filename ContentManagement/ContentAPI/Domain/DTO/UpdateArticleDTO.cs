using Azure;

namespace ContentAPI.Domain.DTO
{
    public class UpdateArticleDTO
    {
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string? SerializedImageURLs { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? MainText { get; set; }
        public Dictionary<string, string>? Base64Images { get; set; }
        public string? OldSerializedImageURLs { get; set; }
    }
}
