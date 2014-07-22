using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncanoSyncServer.Net
{
    public class RealTimeSyncSyncanoClient
    {
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
        public Task<List<Subscription>> GetSubscription(string apiClientId = null, string sessionId = null,
            string uuid = null)
        {
            return _syncanoClient.GetAsync<List<Subscription>>("subscription.get",
                new {api_client_id = apiClientId, session_id = sessionId, uuid}, "subscription");
        }
    }
}
