using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Xunit;

namespace Syncano.Net.Tests
{
    public static class TestData
    {
        public const string InstanceName = "icy-brook-267066";

        public const string BackendAdminApiKey = "f020f3a62b2ea236100a732adcf60cb98683e2e5";
        
    }

    public class SyncanoRestClientTests  : IDisposable
    {
        private SyncanoRestClient _client;
        

        public SyncanoRestClientTests()
        {
            _client = new SyncanoRestClient(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task StartSession_WhenValidInstanceAndKey_CreatesSessionId()
        {
            //when 
            var response = await _client.StartSession();
         
            //then
            response.Result.ShouldEqual("OK");
            response.SessionId.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetProjects()
        {

            //when
            var response = await _client.GetProjects();

            //then
            response.ShouldNotBeEmpty();
            response.Any(p => p.Name  == "Default").ShouldBeTrue();
        }

        public void Dispose()
        {
            
        }
    }
}