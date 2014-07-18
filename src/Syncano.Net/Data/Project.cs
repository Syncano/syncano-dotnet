using Newtonsoft.Json;

namespace Syncano.Net.Data
{
    /// <summary>
    /// Project object.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Project id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Project name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Project description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

    }
}