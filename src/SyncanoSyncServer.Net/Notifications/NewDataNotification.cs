using System.Collections.Generic;
using Newtonsoft.Json;
using Syncano.Net.Data;

namespace SyncanoSyncServer.Net.Notifications
{
    public class NewDataNotification : BaseNotification
    {
        [JsonProperty("channel")]
        public Dictionary<string,string> Channel { get; set; }

        [JsonProperty("data")]
        public DataObject Data { get; set; }
    }
}