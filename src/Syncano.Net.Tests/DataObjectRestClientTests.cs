using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncano.Net.Tests
{
    public class DataObjectRestClientTests : IDisposable
    {
        private Syncano _client;

        public DataObjectRestClientTests()
        {
            _client = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        public void Dispose()
        {
        }
    }
}
