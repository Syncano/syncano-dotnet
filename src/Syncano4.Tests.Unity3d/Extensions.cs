using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano4.Shared;

namespace Syncano4.Tests.Unity3d
{
    public static class Extensions
    {

        public static Task<IList<T>> ListAsync<T, K>(this SyncanoRepository<T, K> repo) where K : IArgs
        {
            return Task.FromResult(repo.List());
        }

        public static Task<T> GetAsync<T, K>(this SyncanoRepository<T, K> repo, string identifier) where K : IArgs
        {
            return Task.FromResult(repo.Get(identifier));
        }

        public static Task<T> AddAsync<T,K>(this SyncanoRepository<T,K> repo, K args) where K:IArgs
        {
            return Task.FromResult(repo.Add(args));
        }


        public static Task<IList<T>> ListAsync<T>(this SyncanoDataObjects<T> repo, int pageSize) where T : DataObject
        {
            return Task.FromResult((repo).List(pageSize));
        }


        public static Task<PageableResult<T>> PageableListAsync<T>(this SyncanoDataObjects<T> repo, int pageSize) where T : DataObject
        {
            return Task.FromResult((repo).PageableList(pageSize));
        }

        public static Task<PageableResult<T>> GetNextAsync<T>(this PageableResult<T> result)
        {
            return Task.FromResult(result.GetNext());
        }
       
    }
}
