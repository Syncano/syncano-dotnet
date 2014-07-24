using System;
using Newtonsoft.Json;
using SyncanoSyncServer.Net.Notifications;

namespace SyncanoSyncServer.Net
{
    /// <summary>
    /// History object.
    /// </summary>
    public class History
    {
        /// <summary>
        /// Notification id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Date and time when notification occurred.
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Defines object that notification was sent for.
        /// </summary>
        [JsonProperty("object")]
        public string Object { get; set; }

        /// <summary>
        /// Defines type of notification.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(NotificationTypeEnumJsonConverter))]
        public NotificationType Type { get; set; }
    }
}
