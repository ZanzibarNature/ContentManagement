using System.Drawing;

namespace ContentAPI.Domain
{
    public class Article : ContentBase
    {
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? MainText { get; set; }
    }
}
