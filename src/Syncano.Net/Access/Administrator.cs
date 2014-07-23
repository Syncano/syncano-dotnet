using System;
using Newtonsoft.Json;
namespace Syncano.Net.Access
{
    /// <summary>
    /// Class representing administrator.
    /// </summary>
    public class Administrator
    {
        /// <summary>
        /// Admin id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Admin email.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Admin first name.
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Admin last name.
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Date and time of last successful login.
        /// </summary>
        [JsonProperty("last_login")]
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// Admin role.
        /// </summary>
        [JsonProperty("role")]
        public Role Role { get; set; }
    }
}
