using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncano.Net
{
    public class DataObjectRestClient
    {
        private readonly SyncanoRestClient _restClient;

        public DataObjectRestClient(SyncanoRestClient restClient)
        {
            _restClient = restClient;
        }
    }
}
