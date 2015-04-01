using System;
using System.Collections.Generic;
using System.Text;

#if dotNET
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

        public T Get(string identifier)
        {
           return _httpClient.Get<T>(string.Format("{0}{1}/", _link, identifier));
        }


        public IList<T> List()
        {
            return List(null);
        }

        public IList<T> List(IDictionary<string, object> parameters)
        {
            return _httpClient.List<T>(_link, parameters).Objects;
        }

         public  SyncanoResponse<T> PageableList(IDictionary<string, object> parameters)
          {
              return _httpClient.List<T>(_link, parameters);
          }

        public T Add(K addArgs)
        {
            return _httpClient.Post<T>(_link, addArgs.ToDictionary());
        }


#endif

#if dotNET

        public Task<T> GetAsync(string identifier)
        {
            return _httpClient.GetAsync<T>(string.Format("{0}{1}/", _link, identifier));
        }
        

        public async Task<IList<T>> ListAsync()
        {
            return (await _httpClient.ListAsync<T>(_link, null)).Objects;
        }

          public async Task<IList<T>> ListAsync(IDictionary<string,object> parameters)
        {
            return (await _httpClient.ListAsync<T>(_link, parameters)).Objects;
        }

          public  Task<SyncanoResponse<T>> PageableListAsync(IDictionary<string, object> parameters)
          {
              return _httpClient.ListAsync<T>(_link, parameters);
          }

        public Task<T> AddAsync(K addArgs)
        {
            return _httpClient.PostAsync<T>(_link, addArgs.ToDictionary());
        }


#endif
    }
}