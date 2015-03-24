using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano4.Shared;

namespace Syncano4.Tests.Unity3d
{
    public static class Extensions
    {
 
        public static Task<IList<T>> GetAsync<T, K>(this SyncanoRepository<T, K> repo) where K : IArgs
        {
            return Task.FromResult(repo.Get());
        }

        public static Task<T> AddAsync<T,K>(this SyncanoRepository<T,K> repo, K args) where K:IArgs
        {
            return Task.FromResult(repo.Add(args));
        }


        public static Task<IList<T>> GetAsync<T>(this SyncanoDataObjects<T> repo, int pageSize) where T : DataObject
        {
            return Task.FromResult(repo.Get(pageSize));
        }
       
    }
}
