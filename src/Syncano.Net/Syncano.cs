using Syncano.Net.Api;
using Syncano.Net.Http;

namespace Syncano.Net
{
    /// <summary>
    /// Main class of Syncano.Net. Provides api to perform all possible http operations on your Syncano instance.
    /// </summary>
    public class Syncano
    {
        private readonly SyncanoHttpClient _httpClient;

        /// <summary>
        /// Creates Syncano object.
        /// </summary>
        /// <param name="instanceName">Name of Syncano instance.</param>
        /// <param name="apiKey">Api key of syncano instance.</param>
        public Syncano(string instanceName, string apiKey)
        {
            _httpClient = new SyncanoHttpClient(instanceName, apiKey);
        }

        /// <summary>
        /// Provides access to Project api.
        /// </summary>
        public ProjectSyncanoClient Projects { get { return new ProjectSyncanoClient(_httpClient);} }

        /// <summary>
        /// Provides access to Folder api.
        /// </summary>
        public FolderSyncanoClient Folders { get { return  new FolderSyncanoClient(_httpClient);} }

        /// <summary>
        /// Provides access to Collection api.
        /// </summary>
        public CollectionSyncanoClient Collections { get { return new CollectionSyncanoClient(_httpClient);} }

        /// <summary>
        /// Provides access to DataObject api.
        /// </summary>
        public DataObjectSyncanoClient DataObjects { get { return new DataObjectSyncanoClient(_httpClient);} }

        /// <summary>
        /// Provides access to Administrator api.
        /// </summary>
        public AdministratorSyncanoClient Administrators { get { return new AdministratorSyncanoClient(_httpClient);} }

        /// <summary>
        /// Provides access to ApiKey api.
        /// </summary>
        public ApiKeySyncanoClient ApiKeys { get { return  new ApiKeySyncanoClient(_httpClient);} }

        /// <summary>
        /// Provides access to Users api.
        /// </summary>
        public UserSyncanoClient Users { get { return new UserSyncanoClient(_httpClient);} }

    }
}