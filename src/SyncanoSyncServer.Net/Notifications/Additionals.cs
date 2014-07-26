
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// Additional parameters of ChangeDataNotification.
    /// </summary>
    public class Additionals
    {
        /// <summary>
        /// Replaced properties and their values.
        /// </summary>
        [JsonProperty("replace")]
        public Dictionary<string,object> Replace { get; set; }

        /// <summary>
        /// Added properties and their values.
        /// </summary>
        [JsonProperty("add")]
        public Dictionary<string, object> Add { get; set; }

        /// <summary>
        /// Deleted properties names.
        /// </summary>
        [JsonProperty("delete")]
        public List<string> Delete { get; set; }
    }
}
