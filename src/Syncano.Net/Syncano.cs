namespace Syncano.Net
{
    public class Syncano
    {
        private SyncanoRestClient _restClient;

        public Syncano(string instanceName, string apiKey)
        {
            _restClient = new SyncanoRestClient(instanceName, apiKey);
        }

        public ProjectRestClient Projects { get { return new ProjectRestClient(_restClient);} }

    }
}