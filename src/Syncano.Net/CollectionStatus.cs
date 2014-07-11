using Newtonsoft.Json;

namespace Syncano.Net
{
    public enum CollectionStatus
    {
        Active,
        Inactive,
        All
    }

    public class CollectionStatusEnumConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string) reader.Value;
            CollectionStatus status;

            switch (enumString)
            {
                case "active":
                    status = CollectionStatus.Active;
                    break;
                case "inactive":
                    status = CollectionStatus.Inactive;
                    break;
                default:
                    status = CollectionStatus.All;
                    break;
            }

            return status;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var status = (CollectionStatus) value;

            switch (status)
            {
                case CollectionStatus.Active:
                    writer.WriteValue("active");
                    break;
                case CollectionStatus.Inactive:
                    writer.WriteValue("inactive");
                    break;
                default:
                    writer.WriteValue("all");
                    break;
            }
        }
    }
}
