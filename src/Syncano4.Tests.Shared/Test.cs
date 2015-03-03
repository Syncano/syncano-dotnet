using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Should;
using Syncano4.Shared;

#if Unity3d
    using Syncano4.Unity3d;
    using Syncano4.Tests.Unity3d;
#endif

#if dotNET
    using Syncano4.Net;
#endif

using Xunit;

namespace Syncano4.Tests.Shared
{
    public class Tests
    {

        public ISyncanoHttpClient GetClient()
        {
            return new SyncanoHttpClient(TestData.AccountKey);
        }


        [Fact]
        public async Task Get()
        {
            var i = new SyncanoInstances(GetClient());
            var instances = await i.GetAsync();

            instances.ShouldNotBeEmpty();
        }
    }
}
