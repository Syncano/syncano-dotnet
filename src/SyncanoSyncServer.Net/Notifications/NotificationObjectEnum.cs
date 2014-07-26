using System;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// Objects of notifications.
    /// </summary>
    public enum NotificationObject
    {
        /// <summary>
        /// DataObject.
        /// </summary>
        Data,

        /// <summary>
        /// DataObjects relation.
        /// </summary>
        DataRelation,

        /// <summary>
        /// Generic notification.
        /// </summary>
        Me
    }

    /// <summary>
    /// Class converts NotificationObject to String and the other way.
    /// </summary>
    public static class NotificationObjectEnumStringConverter
    {
        private const string DataString = "data";
        private const string DataRelationString = "datarelation";
        private const string MeString = "me";

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="value">NotificationObject object.</param>
        /// <returns>Coresponding string value.</returns>
        public static string GetString(NotificationObject value)
        {
            switch (value)
            {
                case NotificationObject.Data:
                    return DataString;

                case NotificationObject.DataRelation:
                    return DataRelationString;

                default:
                    return MeString;
            }
        }

        /// <summary>
        /// Converts to NotificationObject.
        /// </summary>
        /// <param name="value">String object.</param>
        /// <returns>Corresponding NotificationObject.</returns>
        public static NotificationObject GetNotificationObject(string value)
        {
            switch (value)
            {
                case DataString:
                    return NotificationObject.Data;

                case DataRelationString:
                    return NotificationObject.DataRelation;

                default:
                    return NotificationObject.Me;

                
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
