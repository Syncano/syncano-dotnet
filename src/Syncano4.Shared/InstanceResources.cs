namespace Syncano4.Shared
{
    public class InstanceResources
    {
        private readonly ISyncanoHttpClient _httpClient;
        private readonly string _instanceName;

        public string InstanceName
        {
            get { return _instanceName; }
        }

        private readonly InstanceLazyLinkProvider _linkProvider;

        public InstanceResources(Syncano syncano, ISyncanoHttpClient httpClient, string instanceName)
        {
            _httpClient = httpClient;
            _instanceName = instanceName;
            _linkProvider = new InstanceLazyLinkProvider(syncano, httpClient, instanceName);
        }

        public ClassDefinitions Schema
        {
            get { return new ClassDefinitions(i => i.Links["classes"], _linkProvider, _httpClient); }
        }


        public SyncanoDataObjects<T> Objects<T>() where T : DataObject
        {
            return new SyncanoDataObjects<T>(this, typeof(T).Name, _httpClient);
        }
    }
}