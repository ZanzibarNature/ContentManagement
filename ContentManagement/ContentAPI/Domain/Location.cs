
namespace ContentAPI.Domain
{
    public class Location : ContentBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? SerializedIconNames { get; set; }
        public string? GoogleMapsURL { get; set; }
        public string? InvolvementHighlight { get; set; }
    }
}
