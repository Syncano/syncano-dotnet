using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// Notifiactions send when DataObjects are modified.
    /// </summary>
    public class ChangeDataNotification
    {
        /// <summary>
        /// Target information.
        /// </summary>
        [JsonProperty("target")]
        public DataTarget Target { get; set; }

        /// <summary>
        /// Added properties and their values.
        /// </summary>
        [JsonProperty("add")]
        public Dictionary<string, object> Add { get; set; }

        /// <summary>
        /// Replaced properties and their values.
        /// </summary>
        [JsonProperty("replace")]
        public Dictionary<string, object> Replace { get; set; }

        /// <summary>
        /// Names of deleted properties.
        /// </summary>
        [JsonProperty("delete")]
        public List<string> Delete { get; set; }
        
        /// <summary>
        /// Additional parameters.
        /// </summary>
        [JsonProperty("additional")]
        public Additionals Additional { get; set; }
    }
}
