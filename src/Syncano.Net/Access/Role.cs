using Newtonsoft.Json;

namespace Syncano.Net.Access
{
    /// <summary>
    /// Class reprezenting admin role.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Role id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Role name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
