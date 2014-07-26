using System;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.RealTimeSyncApi
{
    /// <summary>
    /// Sources of connections.
    /// </summary>
    public enum Source
    {
        /// <summary>
        /// Tcp source.
        /// </summary>
        Tcp,

        /// <summary>
        /// Web socket source.
        /// </summary>
        WebSocket
    }

    /// <summary>
    /// Class convertinf Source objects to string and the other way.
    /// </summary>
    public static class SourceEnumStringConverter
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

    /// <summary>
    /// Class serializing Source objects to JSON format.
    /// </summary>
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
