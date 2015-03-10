using System.Collections.Generic;

#if dotNet 
using System.Threading.Tasks;
#endif


namespace Syncano4.Shared
{
    public class ClassDefinitions
    {
        private readonly string _link;
        private readonly ISyncanoHttpClient _httpClient;

        public ClassDefinitions(string link, ISyncanoHttpClient httpClient)
        {
            _link = link;
            _httpClient = httpClient;
        }


#if Unity3d
        public IList<SyncanoClass> Get()
        {
            return _httpClient.Get<SyncanoClass>(_link, null);
        }

       
#endif

#if dotNet
        public Task<IList<SyncanoClass>> GetAsync()
        {
            return _httpClient.GetAsync<SyncanoClass>(_link, null);
        }

      
#endif
    }
}