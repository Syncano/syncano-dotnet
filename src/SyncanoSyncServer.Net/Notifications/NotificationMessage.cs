using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class NotificationMessage
    {
        public NotificationMessage()
        {

        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("data")]
        public NotificationBody Body { get; set; }
    }
}