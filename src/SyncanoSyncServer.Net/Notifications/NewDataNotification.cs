using System.Collections.Generic;
using Newtonsoft.Json;
using Syncano.Net.Data;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// Notifiaction about new DataObject.
    /// </summary>
    public class NewDataNotification : BaseNotification
    {
        /// <summary>
        /// Channel info. Describes the source of associated data notification. For Data Objects it consists of a named array with two keys: project_id (string) and collection_id (string).
        /// </summary>
        [JsonProperty("channel")]
        public Dictionary<string,string> Channel { get; set; }

        /// <summary>
        /// Contains of fully serialized object (if relevant).
        /// </summary>
        [JsonProperty("data")]
        public DataObject Data { get; set; }
    }
}