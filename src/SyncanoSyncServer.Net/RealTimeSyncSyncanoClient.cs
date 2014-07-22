using System;
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
                new {project_id = projectId, context = ContextStringConverter.GetString(context)});
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
                    context = ContextStringConverter.GetString(context)
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
    }
}
