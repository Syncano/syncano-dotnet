using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class ChangeDataNotification
    {
        [JsonProperty("target")]
        public DataTarget Target { get; set; }

        [JsonProperty("add")]
        public Dictionary<string, string> Add { get; set; }

        [JsonProperty("replace")]
        public Dictionary<string, string> Replace { get; set; }

        [JsonProperty("delete")]
        public List<string> Delete { get; set; }
        
        [JsonProperty("additional")]
        public Additionals Additional { get; set; }
    }
}
