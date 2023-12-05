using Azure;
using Azure.Data.Tables;

namespace ContentAPI.Domain
{
    public abstract class ContentBase : ITableEntity
    {
        // Azure Table Storage
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Custom Fields
        public string? BannerImageURL { get; set; }
        public List<string>? AdditionalImageURLs { get; set; }
    }
}
