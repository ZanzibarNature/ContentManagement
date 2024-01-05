using ContentAPI.Domain.Enum;

namespace ContentAPI.Domain.DTO
{
    public class CreateArticleDTO
    {
        public ArticleType ArticleType { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? MainText { get; set; }
        public Dictionary<string, string>? Base64Images { get; set; }
    }
}
