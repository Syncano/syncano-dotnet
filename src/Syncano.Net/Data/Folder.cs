using Newtonsoft.Json;

namespace Syncano.Net.Data
{
    /// <summary>
    /// Folder object.
    /// </summary>
    public class Folder
    {
        /// <summary>
        /// Folder id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///  If false, client is not allowed to modify it.
        /// </summary>
        [JsonProperty("is_custom")]
        public bool IsCustom { get; set; }

        /// <summary>
        /// Folder name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Optional source id used for mapping folders to external sources.
        /// </summary>
        [JsonProperty("source_id")]
        public string SourceId { get; set; }
    }
}
