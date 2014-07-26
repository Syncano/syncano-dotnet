using System;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// Base class for all notifications. Contains common properties.
    /// </summary>
    public class BaseNotification
    {
        /// <summary>
        /// Id.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Type of notification.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(NotificationTypeEnumJsonConverter))]
        public NotificationType Type { get; set; }

        /// <summary>
        /// Object of notification.
        /// </summary>
        [JsonProperty("object")]
        [JsonConverter(typeof(NotificationObjectEnumJsonConverter))]
        public NotificationObject Object { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}
