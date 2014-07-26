using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// There are some general notifications.
    /// </summary>
    public class GenericNotification : BaseNotification
    {
        /// <summary>
        /// Source.
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// Additional data as send in SendNotification method.
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; }
    }
}
