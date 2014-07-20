using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Should.Core.Exceptions;
using Syncano.Net.Api;
using Syncano.Net.Http;
using SyncanoSyncServer.Net;

namespace Syncano.Net.Tests
{
    public class SyncanoClientsProvider:IEnumerable<ISyncanoClient>
    {

        private SyncServerClient _syncServerClient;

        public async Task PrepareSyncClient()
        {
            _syncServerClient = new SyncServerClient();

            await _syncServerClient.Connect();
            await _syncServerClient.Login(TestData.BackendAdminApiKey, TestData.InstanceName);
        }


        public IEnumerator<ISyncanoClient> GetEnumerator()
        {
            yield return new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey);

            if (_syncServerClient == null)
            {
               if(!PrepareSyncClient().Wait(50000))
                   throw new AssertException("Failed to initialize Syncano Sync Server client");
            }

            yield return  _syncServerClient;
        }

        public static IEnumerable<object[]> ProjectSyncanoClients
        {
            get
            {
                foreach (var eachClient in new SyncanoClientsProvider())
                {
                    yield return new object[] { new ProjectSyncanoClient(eachClient) };
                }
            }
        }

        public static IEnumerable<object[]> UserSyncanoClients
        {
            get
            {
                foreach (var eachClient in new SyncanoClientsProvider())
                {
                    yield return new object[] { new UserSyncanoClient(eachClient) };
                }
            }
        }

        public static IEnumerable<object[]> FolderSyncanoClients
        {
            get
            {
                foreach (var eachClient in new SyncanoClientsProvider())
                {
                    yield return new object[] { new FolderSyncanoClient(eachClient) };
                }
            }
        }

        public static IEnumerable<object[]> CollectionSyncanoClients
        {
            get
            {
                foreach (var eachClient in new SyncanoClientsProvider())
                {
                    yield return new object[] { new CollectionSyncanoClient(eachClient) };
                }
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}