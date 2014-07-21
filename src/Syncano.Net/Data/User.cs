using Newtonsoft.Json;

namespace Syncano.Net.Data
{
    /// <summary>
    /// Class representing user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id of user.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Username - unique string.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// User nickname.
        /// </summary>
        [JsonProperty("nick")]
        public string Nick { get; set; }

        /// <summary>
        /// User avatar.
        /// </summary>
        [JsonProperty("avatar")]
        public Avatar Avatar { get; set; }
    }
}
