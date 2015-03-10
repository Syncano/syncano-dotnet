using System.Collections.Generic;

#if dotNet 
using System.Threading.Tasks;
#endif

namespace Syncano4.Shared
{
    public class SyncanoInstances
    {
        private readonly ISyncanoHttpClient _httpClient;

        public SyncanoInstances(ISyncanoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

      

#if Unity3d
        public IList<Instance> Get()
        {
            return _httpClient.Get<Instance>("/v1/instances/", null);
        }

        public Instance Add(string name, string description)
        {
            var parameters = new Dictionary<string, object>() { { "name", name }, { "description", description } };
            return _httpClient.Post<Instance>("/v1/instances/", parameters);
        }
#endif

#if dotNet
        public Task<IList<Instance>> GetAsync()
        {
           return _httpClient.GetAsync<Instance>("/v1/instances/", null);
        }

         public Task<Instance> AddAsync(string name, string description)
        {
            var parameters = new Dictionary<string, object>() { { "name", name }, { "description", description } };
            return _httpClient.PostAsync<Instance>("/v1/instances/", parameters);
        }
#endif

    }
}