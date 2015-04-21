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
        private readonly Func<ILazyLinkProvider, string> _getLink  ;
        private readonly ISyncanoHttpClient _httpClient;
        private ILazyLinkProvider _instanceLazyLinkProvider;

        public SyncanoRepository(Func<ILazyLinkProvider, string> getLink, ILazyLinkProvider instanceLazyLinkProvider, ISyncanoHttpClient httpClient)
        {
            _getLink = getLink;
            _httpClient = httpClient;
            _instanceLazyLinkProvider = instanceLazyLinkProvider;
        }

        protected ISyncanoHttpClient HttpClient
        {
            get { return _httpClient; }
        }


#if Unity3d

        public T Get(string identifier)
        {
            return _httpClient.Get<T>(string.Format("{0}{1}/", _getLink(GetLazyLinkProvider()), identifier));
        }


        public IList<T> List()
        {
            return List(null);
        }

        public IList<T> List(IDictionary<string, object> parameters)
        {
            return _httpClient.List<T>(_getLink(GetLazyLinkProvider()), parameters).Objects;
        }

        private ILazyLinkProvider GetLazyLinkProvider()
        {
            if (_instanceLazyLinkProvider == null)
                return null;

            _instanceLazyLinkProvider.Initialize();
            return _instanceLazyLinkProvider;
        }

        public SyncanoResponse<T> PageableList(IDictionary<string, object> parameters)
        {
            return _httpClient.List<T>(_getLink(GetLazyLinkProvider()), parameters);
        }

        public T Add(K addArgs)
        {
            return _httpClient.Post<T>(_getLink(GetLazyLinkProvider()), addArgs.ToDictionary());
        }


#endif

#if dotNET
        private async Task<ILazyLinkProvider> GetLazyLinkProvider()
        {
         if (_instanceLazyLinkProvider == null)
                return null;

            await _instanceLazyLinkProvider.Initialize();
            return _instanceLazyLinkProvider;
        }

        public async Task<T> GetAsync(string identifier)
        {
            return await _httpClient.GetAsync<T>(string.Format("{0}{1}/", _getLink(await GetLazyLinkProvider()), identifier));
        }
        

        public async Task<IList<T>> ListAsync()
        {
            return (await _httpClient.ListAsync<T>(_getLink(await GetLazyLinkProvider()), null)).Objects;
        }

          public async Task<IList<T>> ListAsync(IDictionary<string,object> parameters)
        {
            return (await _httpClient.ListAsync<T>(_getLink(await GetLazyLinkProvider()), parameters)).Objects;
        }

          public async  Task<SyncanoResponse<T>> PageableListAsync(IDictionary<string, object> parameters)
          {
              return await _httpClient.ListAsync<T>(_getLink(await GetLazyLinkProvider()), parameters);
          }

          public async Task<T> AddAsync(K addArgs)
        {
            return await _httpClient.PostAsync<T>(_getLink(await GetLazyLinkProvider()), addArgs.ToDictionary());
        }


#endif
    }
}