using System;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net
{
    public class LoginResponse
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }
        
    }
}