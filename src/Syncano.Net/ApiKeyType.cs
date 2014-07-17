
using System;
using Newtonsoft.Json;

namespace Syncano.Net
{
    public enum ApiKeyType
    {
        Backend,
        User
    }

    public class ApiKeyTypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;
            ApiKeyType type;

            switch (enumString)
            {
                case "backend":
                    type = ApiKeyType.Backend;
                    break;
                case "user":
                    type = ApiKeyType.User;
                    break;
                default:
                    throw new ArgumentException("Unknown ApiKey type value");
            }

            return type;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = (ApiKeyType)value;

            switch (type)
            {
                case ApiKeyType.Backend:
                    writer.WriteValue("backend");
                    break;
                case ApiKeyType.User:
                    writer.WriteValue("user");
                    break;
                default:
                    throw new ArgumentException("Unknown ApiKey value");
            }
        }
    }
}
