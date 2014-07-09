using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Should;
using Xunit;

namespace Syncano.Net.Tests
{
    public class SyncanoRestClientTests
    {
        [Fact]
        public async Task StartSession()
        {
            var response = await new SyncanoRestClient().StartSession("icy-brook-267066", "2347b3ee5d7a9b13622e2812e34aaa7351dc7cbb");
         
            response.Result.ShouldEqual("OK");
            response.SessionId.ShouldNotBeNull();
        }
    }
}