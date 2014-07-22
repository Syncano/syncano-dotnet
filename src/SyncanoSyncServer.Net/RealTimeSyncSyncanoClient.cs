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
    }
}
