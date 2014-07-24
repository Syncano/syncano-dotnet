using System;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public enum NotificationObject
    {
        Data,
        DataRelation
    }

    public class NotificationObjectEnumStringConverter
    {
        private const string DataString = "data";
        private const string DataRelationString = "datarelation";

        public static string GetString(NotificationObject value)
        {
            switch (value)
            {
                case NotificationObject.Data:
                    return DataString;

                case NotificationObject.DataRelation:
                    return DataRelationString;

                default:
                    throw new ArgumentException("Unknown NotificationObject value.");
            }
        }

        public static NotificationObject GetNotificationObject(string value)
        {
            switch (value)
            {
                case DataString:
                    return NotificationObject.Data;

                case DataRelationString:
                    return NotificationObject.DataRelation;

                default:
                    throw new ArgumentException("Unknown NotificationObject string value.");
            }
        }
    }

    public class NotificationObjectEnumJsonConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = (string)reader.Value;
            return NotificationObjectEnumStringConverter.GetNotificationObject(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = (NotificationObject)value;
            writer.WriteValue(NotificationObjectEnumStringConverter.GetString(type));
        }
    }
}
