using System;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class BaseNotification
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(NotificationTypeEnumJsonConverter))]
        public NotificationType Type { get; set; }

        [JsonProperty("object")]
        [JsonConverter(typeof(NotificationObjectEnumJsonConverter))]
        public NotificationObject Object { get; set; }

        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}
