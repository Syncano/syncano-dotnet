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
            return _httpClient.Get<Instance>("instances", null);
        }
#endif

#if dotNet 
        public Task<IList<Instance>> GetAsync()
        {
           return _httpClient.GetAsync<Instance>("instances", null);
        }
#endif

    }
}