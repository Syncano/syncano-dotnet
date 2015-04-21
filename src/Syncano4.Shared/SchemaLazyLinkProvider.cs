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
    public class SchemaLazyLinkProvider : ILazyLinkProvider
    {
        private readonly string _objectName;
        private readonly InstanceResources _instanceResoureces;
        private SyncanoClass _syncanoClass;

        public SchemaLazyLinkProvider(InstanceResources instanceResoureces, string objectName)
        {
            _instanceResoureces = instanceResoureces;
            _objectName = objectName;
        }

#if Unity3d
        public void Initialize()
        {
        if(_syncanoClass == null)
            _syncanoClass = _instanceResoureces.Schema.Get(_objectName);
            
        }
#endif

#if dotNET
        public async Task Initialize()
        {
            if (_syncanoClass == null)
                _syncanoClass = await _instanceResoureces.Schema.GetAsync(_objectName);
        }
#endif

        public Dictionary<string, string> Links
        {
            get { return _syncanoClass.Links; }
        }
    }
}