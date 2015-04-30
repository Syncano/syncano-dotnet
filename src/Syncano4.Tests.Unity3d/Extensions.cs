using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Syncano4.Shared;

namespace Syncano4.Tests.Unity3d
{
    public static class Extensions
    {

        public static Task<IList<T>> ListAsync<T, K>(this SyncanoRepository<T, K> repo) 
        {
            return Task.FromResult(repo.List());
        }

        public static Task<T> GetAsync<T, K>(this SyncanoRepository<T, K> repo, string identifier)
        {
            return Task.FromResult(repo.Get(identifier));
        }

        public static Task<T> GetAsync<T,K>(this SyncanoRepository<T, K> repo, int identifier)
        {
            return Task.FromResult(repo.Get(identifier));
        }

        public static Task<T> AddAsync<T,K>(this SyncanoRepository<T,K> repo, K args) 
        {
            return Task.FromResult(repo.Add(args));
        }

        public static Task<T> UpdateAsync<T, K>(this SyncanoRepository<T, K> repo, string key, T objectToUpdate)
        {
            return Task.FromResult(repo.Update(key, objectToUpdate));
        }

        public static Task<T> PatchAsync<T, K>(this SyncanoRepository<T, K> repo, int key, T objectToUpdate)
        {
            return Task.FromResult(repo.Patch(key, objectToUpdate));
        }

        public static Task<T> PatchAsync<T, K>(this SyncanoRepository<T, K> repo, string key, T objectToUpdate)
        {
            return Task.FromResult(repo.Patch(key, objectToUpdate));
        }

        public static Task<T> UpdateAsync<T, K>(this SyncanoRepository<T, K> repo, int key, T objectToUpdate)
        {
            return Task.FromResult(repo.Update(key, objectToUpdate));
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

        public static Task<PageableResult<T>> GetPreviousAsync<T>(this PageableResult<T> result)
        {
            return Task.FromResult(result.GetPrevious());
        }

       
    }
}
