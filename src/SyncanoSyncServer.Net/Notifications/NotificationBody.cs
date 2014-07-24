using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class NotificationBody
    {
        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }
}