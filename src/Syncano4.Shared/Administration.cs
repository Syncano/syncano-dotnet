namespace Syncano4.Shared
{
    public class Administration
    {
        private readonly ISyncanoHttpClient _httpClient;

        public Administration(ISyncanoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public SyncanoInstances Instances
        {
            get { return new SyncanoInstances(_httpClient); }
        }
    }
}