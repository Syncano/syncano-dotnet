using Newtonsoft.Json;

namespace Syncano.Net.Data
{
    /// <summary>
    /// Class reprezenting avatar.
    /// </summary>
    public class Avatar
    {
        /// <summary>
        /// Image url.
        /// </summary>
        [JsonProperty("image")]
        public string Image { get; set; }

        /// <summary>
        /// Image width.
        /// </summary>
        [JsonProperty("image_width")]
        public int ImageWidth { get; set; }

        /// <summary>
        /// Image height.
        /// </summary>
        [JsonProperty("image_height")]
        public int ImageHeight { get; set; }

        /// <summary>
        /// Thumbnail url.
        /// </summary>
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        /// <summary>
        /// Thumbnail width.
        /// </summary>
        [JsonProperty("thumbnail_width")]
        public int ThumbnailWidth { get; set; }

        /// <summary>
        /// Thumbnail height.
        /// </summary>
        [JsonProperty("thumbnail_height")]
        public int ThumbnailHeight { get; set; }
    }
}
