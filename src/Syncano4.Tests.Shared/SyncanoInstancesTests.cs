using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncano4.Shared;
using Shouldly;
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
    public class SyncanoInstancesTests
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


            instances.ShouldAllBe(ins => ins.Name != null);
            instances.ShouldAllBe(ins => ins.CreatedAt > DateTime.Today.AddYears(-1));
        }

        [Fact]
        public async Task Add()
        {
            //given
            var i = new SyncanoInstances(GetClient());
            string name = "unittest" + Guid.NewGuid().ToString();

            //when
            var instance = await i.AddAsync(name, "desc of " + name);

            //then
            instance.Name.ShouldBe(name);
            instance.CreatedAt.ShouldBeGreaterThan(DateTime.UtcNow.AddMinutes(-1));
        }


        [Fact]
        public async Task Add_uniqnessviolated_throws()
        {
            //given
            var i = new SyncanoInstances(GetClient());
            string name = "unittest" + Guid.NewGuid().ToString();
            var instance = await i.AddAsync(name, "desc of " + name);

            //when
            Should.Throw<SyncanoException>(async () => { var newI = await i.AddAsync(name, "desc of " + name); });

         
        }
    }
}