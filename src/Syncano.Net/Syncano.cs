namespace Syncano.Net
{
    public class Syncano
    {
        private readonly SyncanoRestClient _restClient;

        public Syncano(string instanceName, string apiKey)
        {
            _restClient = new SyncanoRestClient(instanceName, apiKey);
        }

        public ProjectRestClient Projects { get { return new ProjectRestClient(_restClient);} }

        public FolderRestClient Folders { get { return  new FolderRestClient(_restClient);} }

        public CollectionRestClient Collections { get { return new CollectionRestClient(_restClient);} }

        public DataObjectRestClient DataObjects { get { return new DataObjectRestClient(_restClient);} }

        public AdministratorRestClient Administrators { get { return new AdministratorRestClient(_restClient);} }

    }
}