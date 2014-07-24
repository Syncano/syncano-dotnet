using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class DataRelationNotificationMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("target")]
        public DataRelationNotification Target { get; set; }
    }
}