using System;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net
{
    public enum Source
    {
        Tcp,
        WebSocket
    }

    public class SourceEnumStringConverter
    {
        private const string TcpString = "TCP";
        private const string WebSocketString = "WebSocket";

        public static string GetString(Source source)
        {
            switch (source)
            {
                case Source.Tcp:
                    return TcpString;

                case Source.WebSocket:
                    return WebSocketString;

                default:
                    throw new ArgumentException("Unknown Source value.");
            }
        }

        public static Source GetSource(string value)
        {
            switch (value)
            {
                case TcpString:
                    return Source.Tcp;

                case WebSocketString:
                    return Source.WebSocket;

                default:
                    throw new ArgumentException("Unknown Source string.");
            }
        }
    }

    public class SourceEnumJsonConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;
            return SourceEnumStringConverter.GetSource(enumString);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (Source)value;
            writer.WriteValue(SourceEnumStringConverter.GetString(source));
        }
    }
}
