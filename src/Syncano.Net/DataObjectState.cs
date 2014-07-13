using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncano.Net
{
    public enum DataObjectState
    {
        Pending,
        Moderated,
        Rejected
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
                    throw new ArgumentException("Unknown DataObjectState value");
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
                    throw new ArgumentException("Unknown DataObjectState value");
            }
        }
    }
}
