﻿using System;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.RealTimeSyncApi
{
    /// <summary>
    /// Context to subscribe within.
    /// </summary>
    public enum Context
    {
        /// <summary>
        /// Subscribe all connections of current API client.
        /// </summary>
        Client,

        /// <summary>
        /// Store subscription in current session.
        /// </summary>
        Session,

        /// <summary>
        /// Subscribe current connection only (requires Sync Server connection).
        /// </summary>
        Connection
    }

    /// <summary>
    /// Class converting Context objects to string ant the other way.
    /// </summary>
    public class ContextEnumStringConverter
    {
        private const string ClientString = "client";
        private const string SessionString = "session";
        private const string ConnectionString = "connection";

        public static string GetString(Context context)
        {
            switch (context)
            {
                case Context.Client:
                    return ClientString;

                case Context.Session:
                    return SessionString;

                case Context.Connection:
                    return ConnectionString;

                default:
                    throw new ArgumentException("Unknown Context.");
            }
        }

        public static Context GetContext(string value)
        {
            switch (value)
            {
                case ClientString:
                    return Context.Client;

                case ConnectionString:
                    return Context.Connection;

                case SessionString:
                    return Context.Session;

                default:
                    throw new ArgumentException("Unknown Context string.");
            }
        }
    }

    /// <summary>
    /// Class serializing Context objects to JSON format.
    /// </summary>
    public class ContextEnumJsonConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string) reader.Value;
            return ContextEnumStringConverter.GetContext(enumString);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var context = (Context) value;
            writer.WriteValue(ContextEnumStringConverter.GetString(context));
        }
    }
}
