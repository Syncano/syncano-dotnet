using System;
using Newtonsoft.Json;

namespace Syncano4.Shared.Serialization
{
    public class SyncanoDatetimeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue("datetime");
            writer.WritePropertyName("value");
            writer.WriteValue(value);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            DateTime value = DateTime.MinValue;

            reader.Read(); //{
            reader.Read(); // "type":
            reader.Read(); // "datetime"
            reader.Read(); // "value":
            value = (DateTime) reader.Value;
            reader.Read(); // }

            return value;

        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (DateTime);
        }
    }
}