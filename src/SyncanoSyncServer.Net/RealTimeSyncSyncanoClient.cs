using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net;

namespace SyncanoSyncServer.Net
{
    public class RealTimeSyncSyncanoClient
    {
        /// <summary>
        /// Maximum value of limit parameter.
        /// </summary>
        public const int MaxLimit = 100;

        private readonly SyncServerClient _syncanoClient;

        public RealTimeSyncSyncanoClient(SyncServerClient syncanoClient)
        {
            _syncanoClient = syncanoClient;
        }

        /// <summary>
        /// Subscribe to project level notifications - will get all notifications in contained collections.
        /// <remarks>User API key usage permitted with context session or connection if subscribe permission is added through apikey.authorize() and read_data permission is added to specified project through project.authorize().</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="context">Context to subscribe within.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> SubscribeProject(string projectId, Context context = Context.Client)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("subscription.subscribe_project",
                new {project_id = projectId, context = ContextEnumStringConverter.GetString(context)});
        }

        /// <summary>
        /// Unsubscribe from a project. Unsubscribing will work in context that it was originally created for.
        /// <remarks>User API key usage permitted.</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> UnsubscribeProject(string projectId)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("subscription.unsubscribe_project", new {project_id = projectId});
        }

        /// <summary>
        /// Subscribe to collection level notifications within a specified project.
        /// <remarks>Collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// <remarks>User API key usage permitted with context session or connection if subscribe permission is added through apikey.authorize() and read_data permission is added to specified collection through collection.authorize() or project.authorize().</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="collectionId">Collection id defining collection.</param>
        /// <param name="collectionKey">Collection key defining collection.</param>
        /// <param name="context">Context to subscribe within. </param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> SubscribeCollection(string projectId, string collectionId = null, string collectionKey = null,
            Context context = Context.Client)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("subscription.subscribe_collection",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    context = ContextEnumStringConverter.GetString(context)
                });
        }

        /// <summary>
        /// Unsubscribe from a collection within a specified project. Unsubscribing will work in context that it was originally created for.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// <remarks>User API key usage permitted.</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="collectionId">Collection id defining collection.</param>
        /// <param name="collectionKey">Collection key defining collection.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> UnsubscribeCollection(string projectId, string collectionId = null, string collectionKey = null)
        {
            if (projectId == null)
                throw new ArgumentNullException();

            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("subscription.unsubscribe_collection",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey
                });
        }

        /// <summary>
        /// Get API client subscriptions.
        /// <remarks>User API key usage permitted.</remarks>
        /// </summary>
        /// <param name="apiClientId">API client id defining client. If not present, gets subscriptions for current API client.</param>
        /// <param name="sessionId">Session id associated with API client. If present, gets subscriptions associated with specified API client's session.</param>
        /// <param name="uuid">Connection UUID. If present, gets subscriptions associated with specified API client's connection.</param>
        /// <returns>List of Subscription objects.</returns>
        public async Task<List<Subscription>> GetSubscriptions(string apiClientId = null, string sessionId = null,
            string uuid = null)
        {
            return await _syncanoClient.GetAsync<List<Subscription>>("subscription.get",
                new {api_client_id = apiClientId, session_id = sessionId, uuid}, "subscription");
        }

        /// <summary>
        /// Sends custom notification to API client through Sync Server. If uuid is specified - will only send to this specific instance.
        /// <remarks>User API key usage permitted if send_notification permission is added through apikey.authorize().</remarks>
        /// </summary>
        /// <param name="apiClientId">Destination API client id. If not specified, will query current API client's connections.</param>
        /// <param name="uuid">UUID of specified API client's connection. If not specified, will send a broadcast to all specified API client's connections.</param>
        /// <param name="additional">Any number of additional parameters will be sent as well into data structure.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> SendNotification(string apiClientId = null, string uuid = null, Dictionary<string, string> additional = null)
        {
            return _syncanoClient.GetAsync("notification.send", new {api_client_id = apiClientId, uuid, additional});
        }

        /// <summary>
        /// Get a history of notifications of current API client. History items are stored for 24 hours.
        /// <remarks>User API key usage permitted if subscribe permission is added through apikey.authorize().</remarks>
        /// </summary>
        /// <param name="sinceId">If specified, will only return data with id higher than since_id (newer).</param>
        /// <param name="sinceTime">String with date. If specified, will only return data with timestamp after specified value (newer).</param>
        /// <param name="limit">Maximum number of history items to get. Default and max: 100.</param>
        /// <param name="order">Order of data that will be returned.</param>
        /// <returns>List of History objects.</returns>
        public async Task<List<History>> GetHistory(string sinceId = null, DateTime? sinceTime = null, int limit = MaxLimit,
            DataObjectOrder order = DataObjectOrder.Ascending)
        {
            return
                await
                    _syncanoClient.GetAsync<List<History>>("notification.get_history",
                        new
                        {
                            since_id = sinceId,
                            since_time = sinceTime,
                            limit,
                            order = DataObjectOrderStringConverter.GetString(order)
                        }, "history");
        }

        /// <summary>
        /// Get currently connected API client connections up to a limit (max 100).
        /// </summary>
        /// <param name="apiClientId">API client id. If not specified, will get connections for current API client.</param>
        /// <param name="name">If specified, will only return connections of specified name.</param>
        /// <param name="sinceId">If specified, will only return data created after specified uuid (newer).</param>
        /// <param name="limit">Maximum number of API client connections to get. Default and max: 100.</param>
        /// <returns>List of Connection objects.</returns>
        public async Task<List<Connection>> GetConnections(string apiClientId = null, string name = null, string sinceId = null, int limit = MaxLimit)
        {
            if(limit > MaxLimit)
                throw new ArgumentException();

            return await _syncanoClient.GetAsync<List<Connection>>("connection.get", new {api_client_id = apiClientId, name, since_id = sinceId, limit},
                       "connection");
        }

        /// <summary>
        /// Get all connections from current instance up to a limit (max 100).
        /// </summary>
        /// <param name="name">If specified, will only return connections of specified name.</param>
        /// <param name="sinceId">If specified, will only return data created after specified uuid (newer).</param>
        /// <param name="limit">Maximum number of API client connections to get. Default and max: 100.</param>
        /// <returns>List of Connection objects.</returns>
        public async Task<List<Connection>> GetAllConnections(string name = null, string sinceId = null,
            int limit = MaxLimit)
        {
            if (limit > MaxLimit)
                throw new ArgumentException();

            return
                await
                    _syncanoClient.GetAsync<List<Connection>>("connection.get_all",
                        new {name, since_id = sinceId, limit}, "connection");
        }

        /// <summary>
        /// Updates specified API client connection info.
        /// </summary>
        /// <param name="uuid">Connection UUID.</param>
        /// <param name="name">New name to set.</param>
        /// <param name="state">New state to set.</param>
        /// <param name="apiClientId">API client id. If not specified, will update current API client connections.</param>
        /// <returns>Updated Collection object.</returns>
        public Task<Connection> UpdateConnection(string uuid, string name = null, string state = null,
            string apiClientId = null)
        {
            if(uuid == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync<Connection>("connection.update",
                new {uuid, name, state, api_client_id = apiClientId}, "connection");
        }
    }
}
