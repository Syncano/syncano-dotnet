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

#if Unity3d
            _httpClient = new SyncanoHttpClient(authkey);
            #endif

#if dotNET
            _httpClient = new SyncanoHttpClient(authkey);
#endif
        }

        public Administration Administration
        {
            get { return new Administration(_httpClient); }
        }


#if Unity3d
        public InstanceResources ResourcesFor(string existingInstance)
        {
            var instance = this.Administration.Instances.Get(existingInstance);
            return new InstanceResources(_httpClient, instance);
        }
#endif

#if dotNET
        public async Task<InstanceResources> ResourcesFor(string existingInstance)
        {
            var instance = await this.Administration.Instances.GetAsync(existingInstance);
            return new InstanceResources(_httpClient, instance);
        }
#endif
    }

    public class InstanceResources
    {
        private readonly ISyncanoHttpClient _httpClient;
        private readonly Instance _instance;

        public InstanceResources(ISyncanoHttpClient httpClient, Instance instance)
        {
            _httpClient = httpClient;
            _instance = instance;
        }

        public ClassDefinitions Schema
        {
            get { return new ClassDefinitions(_instance.Links["classes"], _httpClient); }
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