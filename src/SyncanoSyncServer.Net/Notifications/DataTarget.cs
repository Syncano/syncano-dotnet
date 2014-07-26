using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// Contains number of ids. When present in notification objeccts it means, than notification involves multiple objects of ids specified in Target object.
    /// </summary>
    public class DataTarget
    {
        /// <summary>
        /// Ids of objects.
        /// </summary>
        [JsonProperty("id")]
        public List<string> Ids { get; set; }
    }
}
