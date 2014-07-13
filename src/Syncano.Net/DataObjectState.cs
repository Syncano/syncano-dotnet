using Newtonsoft.Json;
using System;

namespace Syncano.Net
{
    public enum DataObjectState
    {
        Pending,
        Moderated,
        Rejected,
        All
    }

    public class DataObjectStateEnumConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;
            DataObjectState state;

            switch (enumString)
            {
                case "Pending":
                    state = DataObjectState.Pending;
                    break;
                case "Moderated":
                    state = DataObjectState.Moderated;
                    break;
                case "Rejected":
                    state = DataObjectState.Rejected;
                    break;
                default:
                    state = DataObjectState.All;
                    break;
            }

            return state;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var state = (DataObjectState)value;

            switch (state)
            {
                case DataObjectState.Moderated:
                    writer.WriteValue("Moderated");
                    break;
                case DataObjectState.Pending:
                    writer.WriteValue("Pending");
                    break;
                case DataObjectState.Rejected:
                    writer.WriteValue("Rejected");
                    break;
                default:
                    writer.WriteValue("All");
                    break;
            }
        }
    }
}
