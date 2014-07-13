using Newtonsoft.Json;

namespace Syncano.Net
{
    public class Image
    {
        [JsonProperty("image")]
        public string ImageUrl { get; set; }

        [JsonProperty("image_width")]
        public int? ImageWidth { get; set; }

        [JsonProperty("image_height")]
        public int ImageHeight { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("thumbnail_width")]
        public int ThumbnailWidth { get; set; }

        [JsonProperty("thumbnail_height")]
        public int ThumbnailHeight { get; set; }

        [JsonProperty("source_url")]
        public string SourceUrl { get; set; }
    }
}
