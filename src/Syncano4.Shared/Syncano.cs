using System;
using System.Collections.Generic;
using System.Text;

#if Unity3d
using Syncano4.Unity3d;
#endif

#if dotNET
using Syncano4.Net;
#endif

namespace Syncano4.Shared
{
    public class Syncano
    {
        private readonly string _authkey;
        private readonly ISyncanoHttpClient _httpClient;
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

        public SyncanoInstances Instances { get { return new SyncanoInstances(_httpClient);} }
    }
}
