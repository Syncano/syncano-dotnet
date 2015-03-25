using System.Collections.Generic;

#if dotNET 
using System.Threading.Tasks;
#endif

namespace Syncano4.Shared
{
    public class SyncanoInstances : SyncanoRepository<Instance, NewInstance>
    {

        public SyncanoInstances(ISyncanoHttpClient httpClient)
            : base("/v1/instances/", httpClient)
        {
        }
 
        
    }
}