using System;
using System.Collections.Generic;
using System.Text;

#if dotNet
using System.Threading.Tasks;

#endif

namespace Syncano4.Shared
{
    public interface IArgs
    {
        IDictionary<string, object> ToDictionary();
    }

    public class SyncanoRepository<T, K> where K : IArgs
    {

         private readonly string _link;
        private readonly ISyncanoHttpClient _httpClient;

        public SyncanoRepository(string link, ISyncanoHttpClient httpClient)
        {
            _link = link;
            _httpClient = httpClient;
        }

        protected ISyncanoHttpClient HttpClient
        {
            get { return _httpClient; }
        }


#if Unity3d
        public IList<T> Get()
        {
            return Get(null);
        }

        public IList<T> Get(IDictionary<string,object> parameters)
        {
            return _httpClient.Get<T>(_link, parameters);
        }

        public T Add(K addArgs)
        {
            return _httpClient.Post<T>(_link, addArgs.ToDictionary());
        }


#endif

#if dotNet
        public Task<IList<T>> GetAsync()
        {
            return _httpClient.GetAsync<T>(_link, null);
        }

          public Task<IList<T>> GetAsync(IDictionary<string,object> parameters)
        {
            return _httpClient.GetAsync<T>(_link, parameters);
        }

        public Task<T> AddAsync(K addArgs)
        {
            return _httpClient.PostAsync<T>(_link, addArgs.ToDictionary());
        }


#endif
    }
}
