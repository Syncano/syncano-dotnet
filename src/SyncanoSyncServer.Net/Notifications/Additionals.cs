
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class Additionals
    {
        [JsonProperty("replace")]
        public Dictionary<string,string> Replace { get; set; }

        [JsonProperty("add")]
        public Dictionary<string, string> Add { get; set; }

        [JsonProperty("delete")]
        public List<string> Delete { get; set; }
    }
}
