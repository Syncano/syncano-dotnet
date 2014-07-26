using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.RealTimeSyncApi
{
    /// <summary>
    /// Connections provide a way to distinguish between multiple Sync Server connections within one API key. They are uniquely defined by UUID. Each Connection can also be provided with a name and a state, which can help create stateful clients that are externally controlled or monitored.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Connection id - used for paginating with since_id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Connection UUID - used to uniquely identify connection.
        /// </summary>
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        /// <summary>
        /// API client id.
        /// </summary>
        [JsonProperty("api_client_id")]
        public string ApiClientId { get; set; }

        /// <summary>
        /// Connection name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Connection state.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }


        /// <summary>
        /// Connection source - takes values: TCP or WebSocket.
        /// </summary>
        [JsonProperty("source")]
        [JsonConverter(typeof(SourceEnumJsonConverter))]
        public Source Source { get; set; }
    }
}
