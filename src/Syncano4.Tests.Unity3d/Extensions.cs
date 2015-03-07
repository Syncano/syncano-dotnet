using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano4.Shared;

namespace Syncano4.Tests.Unity3d
{
    public static class Extensions
    {
        public static Task<IList<Instance>> GetAsync(this SyncanoInstances instances)
        {
            return Task.FromResult(instances.Get());
        }
    }
}
