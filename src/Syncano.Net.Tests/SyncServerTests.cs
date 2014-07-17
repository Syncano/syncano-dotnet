using System;
using System.Collections;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Syncano.Net.Http;
using SyncanoSyncServer.Net;
using Xunit;
using Xunit.Extensions;
using System.Collections.Generic;

namespace Syncano.Net.Tests
{
    public class SyncServerTests //: IUseFixture<SyncClientProvider>
    {
        private static SyncServerClient _syncServerClient;

        [Theory, ClassData(typeof(ClientsToTest))]
        public async Task Spike(ISyncanoClient client)
        {
            var projectClient = new ProjectSyncanoClient(client);

            var projects = await projectClient.Get();

            projects.ShouldNotBeNull();


        }

        public static IEnumerable<object[]> Clients
        {
            get
            {
                yield return new object[] { new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey) };

                yield return new object[] { _syncServerClient };
            }
        }

        public void SetFixture(SyncClientProvider data)
        {
            _syncServerClient = data._syncServerClient;
        }
    }

    public class SyncClientProvider : IDisposable
    {
        public  SyncServerClient _syncServerClient;

        public SyncClientProvider()
        {
            _syncServerClient = new SyncServerClient();

            _syncServerClient.Connect().Wait(10000);
            _syncServerClient.Login(TestData.BackendAdminApiKey, TestData.InstanceName).Wait();
        }


        public void Dispose()
        {
            
        }
    }


    public class ClientsToTest:IEnumerable<object[]>
    {

        private SyncServerClient _syncServerClient;

        public async Task PrepareSyncClient()
        {
            _syncServerClient = new SyncServerClient();

            await _syncServerClient.Connect();
            await _syncServerClient.Login(TestData.BackendAdminApiKey, TestData.InstanceName);
        }


        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey)};

            if (_syncServerClient == null)
                PrepareSyncClient().Wait(10000);

            yield return new object[] { _syncServerClient };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}