using System;
using System.Collections.Generic;
using System.Text;
#if Unity3d
using Syncano4.Unity3d;

#endif
#if dotNET
using Syncano4.Net;
using System.Threading.Tasks;
#endif

namespace Syncano4.Shared
{
    public class Syncano
    {
        private readonly string _authkey;
        private readonly ISyncanoHttpClient _httpClient;

        public static Syncano Using(string authkey)
        {
            return new Syncano(authkey);
        }

        public Syncano(string authkey)
        {
            _authkey = authkey;
            _httpClient = new SyncanoHttpClient(authkey);
        }

        public Administration Administration
        {
            get { return new Administration(_httpClient); }
        }

        public InstanceResources ResourcesFor(string existingInstance)
        {
            return new InstanceResources(_httpClient, new LazyLinkProvider(this, _httpClient, existingInstance));
        }
    }

    public class LazyLinkProvider
    {
        private readonly Syncano _syncano;
        private readonly ISyncanoHttpClient _httpClient;
        private readonly string _instanceName;
        private Instance _instance;

        public LazyLinkProvider(Syncano syncano, ISyncanoHttpClient httpClient, string instanceName)
        {
            _syncano = syncano;
            _httpClient = httpClient;
            _instanceName = instanceName;
        }

#if Unity3d
        public void Initialize()
        {
            _instance = _syncano.Administration.Instances.Get(_instanceName);
        }
#endif

#if dotNET
          public async Task Initialize()
        {
            _instance = await  _syncano.Administration.Instances.GetAsync(_instanceName); ;


        }
#endif

        public Dictionary<string,string> Links
        {
            get { return _instance.Links; }
        }
    }

    public class InstanceResources
    {
        private readonly ISyncanoHttpClient _httpClient;
        private readonly LazyLinkProvider _lazyLinkProvider;

        public InstanceResources(ISyncanoHttpClient httpClient, LazyLinkProvider lazyLinkProvider)
        {
            _httpClient = httpClient;
            _lazyLinkProvider = lazyLinkProvider;
        }

        public ClassDefinitions Schema
        {
            get { return new ClassDefinitions(i => i.Links["classes"], _lazyLinkProvider, _httpClient); }
        }
        
        public SyncanoDataObjects<T> Objects<T>() where T : DataObject
        {
            //var classDef = this.Schema.Get(typeof (T).Name);
            return new SyncanoDataObjects<T>(null, _httpClient);
        }
    }

    public class Administration
    {
        private readonly ISyncanoHttpClient _httpClient;

        public Administration(ISyncanoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public SyncanoInstances Instances
        {
            get { return new SyncanoInstances(_httpClient); }
        }
    }
}