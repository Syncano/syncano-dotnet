using Newtonsoft.Json;

namespace Syncano.Net.Data
{
    /// <summary>
    /// Representing DataObject Image.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Image URL.
        /// </summary>
        [JsonProperty("image")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Width of image.
        /// </summary>
        [JsonProperty("image_width")]
        public int ImageWidth { get; set; }

        /// <summary>
        /// Height of image.
        /// </summary>
        [JsonProperty("image_height")]
        public int ImageHeight { get; set; }

        /// <summary>
        /// Thumbnail URL.
        /// </summary>
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        /// <summary>
        /// Width of thumbnail.
        /// </summary>
        [JsonProperty("thumbnail_width")]
        public int ThumbnailWidth { get; set; }

        /// <summary>
        /// Height of thumbnail.
        /// </summary>
        [JsonProperty("thumbnail_height")]
        public int ThumbnailHeight { get; set; }

        /// <summary>
        /// Source URL.
        /// </summary>
        [JsonProperty("source_url")]
        public string SourceUrl { get; set; }
    }
}
