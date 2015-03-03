using System;
using System.Collections.Generic;
using System.Text;

#if dotNet 
using System.Threading.Tasks;

#endif

namespace Syncano4.Shared
{
    public class Instance
    {
        public string Name { get; set; }
    }

    public class SyncanoClient
    {
        public SyncanoClient()
        {
            
        }
    }


    public class SyncanoInstances
    {
        private readonly ISyncanoHttpClient _httpClient;

        public SyncanoInstances(ISyncanoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

      

#if Unity3d
        public List<Instance> Get()
        {
            return new List<Instance>() { new Instance() { Name = _httpClient.Get("instances", null) } };
        }
#endif

#if dotNet 
        public async Task<List<Instance>> GetAsync()
        {
            return new List<Instance>() { new Instance() { Name = await _httpClient.GetAsync("instances", null).ConfigureAwait(false) } };
        }
#endif

    }
}