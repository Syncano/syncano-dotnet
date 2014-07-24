using System.Collections.Generic;
using Newtonsoft.Json;
using Syncano.Net.Data;

namespace SyncanoSyncServer.Net.Notifications
{
    public class NewDataNotification
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("channel")]
        public Dictionary<string,string> Channel { get; set; }


        [JsonProperty("data")]
        public DataObject Data { get; set; }
    }
}