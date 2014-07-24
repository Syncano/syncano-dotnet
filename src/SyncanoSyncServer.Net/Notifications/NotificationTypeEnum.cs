
using System;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public enum NotificationType
    {
        New,
        Change,
        Delete,
        Message
    }

    public class NotificationTypeEnumStringConverter
    {
        private const string NewString = "new";
        private const string ChangeString = "change";
        private const string DeleteString = "delete";
        private const string MessageString = "message";

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
                    throw new ArgumentException("Unknown NotificationType value.");
            }
        }

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
                    throw new ArgumentException("Unknown NotificationType string value.");
            }
        }
    }

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
