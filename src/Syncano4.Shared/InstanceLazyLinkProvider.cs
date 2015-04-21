using System;
using System.Collections.Generic;
#if Unity3d
using Syncano4.Unity3d;

#endif
#if dotNET
using Syncano4.Net;
using System.Threading.Tasks;

#endif

namespace Syncano4.Shared
{
    public class InstanceLazyLinkProvider : ILazyLinkProvider
    {
        private readonly Syncano _syncano;
        private readonly ISyncanoHttpClient _httpClient;
        private readonly string _instanceName;
        private Instance _instance;

        public InstanceLazyLinkProvider(Syncano syncano, ISyncanoHttpClient httpClient, string instanceName)
        {
            _syncano = syncano;
            _httpClient = httpClient;
            _instanceName = instanceName;
        }

#if Unity3d
        public void Initialize()
        {
         if (_instance == null)
            {
            _instance = _syncano.Administration.Instances.Get(_instanceName);
            }
        }
#endif

#if dotNET
        public async Task Initialize()
        {
            //TODO add locking

            if (_instance == null)
            {
                _instance = await _syncano.Administration.Instances.GetAsync(_instanceName);
            }
        }
#endif

        public Dictionary<string, string> Links
        {
            get { return _instance.Links; }
        }
    }
}