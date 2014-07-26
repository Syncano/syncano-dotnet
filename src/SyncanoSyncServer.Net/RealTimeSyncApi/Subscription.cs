
using Newtonsoft.Json;
using SyncanoSyncServer.Net.RealTimeSyncApi;

namespace SyncanoSyncServer.Net.RealTimeSyncApi
{
    /// <summary>
    /// Subscription can be created in three separate contexts: API client bound (client context) - subscription is shared for all connections of an API client. Session bound (session context) - shared for all connections associated with specific session_id. This implies that they are of one specific client as well. Connection bound (connection context) - not shared. Defined per one specific connection. There are also two possible levels of subscriptions.: Project - gets notifications for both project and all bound collections, Collection - gets notifications for one specific collection.
    /// <remarks>Maximum 25 subscriptions per client</remarks>
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Subscription type (e.g. Project, Collection).
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Subscription object id (e.g. project_id if type is equal Project).
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Subscription context - client, session or connection.
        /// </summary>
        [JsonProperty("context")]
        [JsonConverter(typeof(ContextEnumJsonConverter))]
        public Context Context { get; set; }
    }
}
