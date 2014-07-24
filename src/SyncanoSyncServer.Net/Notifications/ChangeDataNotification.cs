using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class ChangeDataNotification
    {
        [JsonProperty("add")]
        public Dictionary<string, string> Add { get; set; }

        [JsonProperty("replace")]
        public Dictionary<string, string> Replace { get; set; }

        [JsonProperty]
        public List<string> Delete { get; set; }
        
        [JsonProperty("additional")]
        public Additionals Additional { get; set; }
    }
}
