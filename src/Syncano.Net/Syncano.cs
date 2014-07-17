using Syncano.Net.Api;
using Syncano.Net.Http;

namespace Syncano.Net
{
    public class Syncano
    {
        private readonly SyncanoHttpClient _httpClient;

        public Syncano(string instanceName, string apiKey)
        {
            _httpClient = new SyncanoHttpClient(instanceName, apiKey);
        }

        public ProjectSyncanoClient Projects { get { return new ProjectSyncanoClient(_httpClient);} }

        public FolderSyncanoClient Folders { get { return  new FolderSyncanoClient(_httpClient);} }

        public CollectionSyncanoClient Collections { get { return new CollectionSyncanoClient(_httpClient);} }

        public DataObjectSyncanoClient DataObjects { get { return new DataObjectSyncanoClient(_httpClient);} }

        public AdministratorSyncanoClient Administrators { get { return new AdministratorSyncanoClient(_httpClient);} }

        public ApiKeySyncanoClient ApiKeys { get { return  new ApiKeySyncanoClient(_httpClient);} }

    }
}