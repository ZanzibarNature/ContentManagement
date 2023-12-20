using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json;

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
        public string? SerializedImageURLs { get; set; }
        public Dictionary<string, string> ImageURLs
        {
            get { return JsonConvert.DeserializeObject<Dictionary<string, string>>(SerializedImageURLs); }
            set { SerializedImageURLs = JsonConvert.SerializeObject(value); }
        }

    }
}
