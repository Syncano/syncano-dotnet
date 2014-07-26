using System;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// Types of notifications.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// New object or relation.
        /// </summary>
        New,

        /// <summary>
        /// Changed object.
        /// </summary>
        Change,

        /// <summary>
        /// Removed object or relation.
        /// </summary>
        Delete,

        /// <summary>
        /// Information.
        /// </summary>
        Message,

        /// <summary>
        /// Server login.
        /// </summary>
        Authorization
    }

    /// <summary>
    /// Class converts NotificationTypes to String and the other way.
    /// </summary>
    public static class NotificationTypeEnumStringConverter
    {
        private const string NewString = "new";
        private const string ChangeString = "change";
        private const string DeleteString = "delete";
        private const string MessageString = "message";
        private const string AuthString = "auth";

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="type">NotificationType object.</param>
        /// <returns>Coresponding string value.</returns>
        public static string GetString(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.New:
                    return NewString;

                case NotificationType.Change:
                    return ChangeString;

                case NotificationType.Delete:
                    return DeleteString;

                case NotificationType.Message:
                    return MessageString;

                default:
                    return AuthString;
            }
        }

        /// <summary>
        /// Converts to NotificationType.
        /// </summary>
        /// <param name="value">String object.</param>
        /// <returns>Corresponding NotificationType.</returns>
        public static NotificationType GetNotificationType(string value)
        {
            switch (value)
            {
                case NewString:
                    return NotificationType.New;

                case ChangeString:
                    return NotificationType.Change;

                case DeleteString:
                    return NotificationType.Delete;

                case MessageString:
                    return NotificationType.Message;

                default:
                    return NotificationType.Authorization;
            }
        }
    }

    /// <summary>
    /// Class serializes NotificationTypes to JSON format.
    /// </summary>
    public class NotificationTypeEnumJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = (string)reader.Value;
            return NotificationTypeEnumStringConverter.GetNotificationType(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = (NotificationType) value;
            writer.WriteValue(NotificationTypeEnumStringConverter.GetString(type));
        }
    }
}
