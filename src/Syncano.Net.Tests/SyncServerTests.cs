using System.Threading.Tasks;
using Should;
using SyncanoSyncServer.Net;
using Xunit;

namespace Syncano.Net.Tests
{
    public class SyncServerTests
    {
        [Fact]
        public async Task Spike()
        {
            var sync = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
            await sync.Start();
            var projects = await sync.Projects.Get();

            projects.ShouldNotBeNull();


        }
    }
}