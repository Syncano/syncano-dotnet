using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#if dotNET
using System.Threading.Tasks;

#endif

namespace Syncano4.Shared
{
    public class SyncanoRepository<T, K>
    {
        private readonly Func<ILazyLinkProvider, string> _getLink;
        private readonly ISyncanoHttpClient _httpClient;
        private ILazyLinkProvider _instanceLazyLinkProvider;
        private SyncanoSerializer _syncanoSerializer = new SyncanoSerializer();

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

        public T Get(string key)
        {
            return _httpClient.Get<T>(string.Format("{0}{1}/", _getLink(GetLazyLinkProvider()), key));
        }

        public T Get(int key)
        {
            return Get(key.ToString(CultureInfo.InvariantCulture));
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
            return _httpClient.Post<T>(_getLink(GetLazyLinkProvider()), _syncanoSerializer.ToDictionary(addArgs));
        }

        public T Update(string key, T objectToUpdate)
        {
            return _httpClient.Post<T>(string.Format("{0}{1}/", _getLink(GetLazyLinkProvider()), key), _syncanoSerializer.ToDictionary(objectToUpdate))
                ;
        }

        public T Patch(int key, T objectToUpdate)
        {
            return Patch(key.ToString(CultureInfo.InvariantCulture), objectToUpdate);
        }


        public T Patch(string key, T objectToUpdate)
        {
            return _httpClient.Patch<T>(string.Format("{0}{1}/", _getLink(GetLazyLinkProvider()), key), _syncanoSerializer.ToDictionary(objectToUpdate))
                ;
        }

        public T Update(int key, T objectToUpdate)
        {
            return Update(key.ToString(CultureInfo.InvariantCulture), objectToUpdate);
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

        public async Task<T> GetAsync(string key)
        {
            return await _httpClient.GetAsync<T>(string.Format("{0}{1}/", _getLink(await GetLazyLinkProvider()), key));
        }

        public async Task<T>  GetAsync(int key)
        {
            return await GetAsync(key.ToString(CultureInfo.InvariantCulture));
        }


        public async Task<IList<T>> ListAsync()
        {
            return (await _httpClient.ListAsync<T>(_getLink(await GetLazyLinkProvider()), null)).Objects;
        }

        public async Task<IList<T>> ListAsync(IDictionary<string, object> parameters)
        {
            return (await _httpClient.ListAsync<T>(_getLink(await GetLazyLinkProvider()), parameters)).Objects;
        }

        public async Task<SyncanoResponse<T>> PageableListAsync(IDictionary<string, object> parameters)
        {
            return await _httpClient.ListAsync<T>(_getLink(await GetLazyLinkProvider()), parameters);
        }

        public async Task<T> AddAsync(K addArgs)
        {
            return await _httpClient.PostAsync<T>(_getLink(await GetLazyLinkProvider()), _syncanoSerializer.ToDictionary(addArgs));
        }

        public async Task<T> UpdateAsync(string key, T objectToUpdate)
        {
            return await _httpClient.PostAsync<T>(string.Format("{0}{1}/",_getLink(await GetLazyLinkProvider()),key), _syncanoSerializer.ToDictionary(objectToUpdate));
        }

        public async Task<T> UpdateAsync(int key, T objectToUpdate)
        {
            return await UpdateAsync(key.ToString(CultureInfo.InvariantCulture), objectToUpdate);
        }

        public async Task<T> PatchAsync(string key, T objectToUpdate)
        {
            return await _httpClient.PatchAsync<T>(string.Format("{0}{1}/", _getLink(await GetLazyLinkProvider()), key), _syncanoSerializer.ToDictionary(objectToUpdate));
        }

        public async Task<T> PatchAsync(int key, T objectToUpdate)
        {
            return await PatchAsync(key.ToString(CultureInfo.InvariantCulture), objectToUpdate);
        }

#endif
    }
}