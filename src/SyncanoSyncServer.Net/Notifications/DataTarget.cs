using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class DataTarget
    {
        [JsonProperty("id")]
        public List<string> Ids { get; set; }
    }
}
