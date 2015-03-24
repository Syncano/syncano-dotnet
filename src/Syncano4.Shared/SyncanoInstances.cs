using System.Collections.Generic;

#if dotNet 
using System.Threading.Tasks;
#endif

namespace Syncano4.Shared
{
    public class SyncanoInstances : SyncanoRepository<Instance, CreateInstanceArgs>
    {

        public SyncanoInstances(ISyncanoHttpClient httpClient)
            : base("/v1/instances/", httpClient)
        {
        }
 

        
    }
}