using System;

namespace SyncanoSyncServer.Net
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

    public class ContextStringConverter
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
}
