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
        public  SyncanoInstances GetInstanceAdminstration()
        {
            return Syncano.Using(TestData.AccountKey).Administration.Instances;
        }

        


        [Fact]
        public async Task List()
        {
            var i =  GetInstanceAdminstration();
            var instances = await i.ListAsync();


            instances.ShouldAllBe(ins => ins.Name != null);
            instances.ShouldAllBe(ins => ins.CreatedAt > DateTime.Today.AddYears(-1));
            instances.ShouldAllBe(ins => ins.Links.Keys.Count > 0);
        }



        [Fact]
        public async Task Get()
        {
            //given
            var i =  GetInstanceAdminstration();
            string name = "unittest" + Guid.NewGuid().ToString();
            var instance = await i.AddAsync(new NewInstance() { Name = name, Description = "desc of " + name });

            //when
            var fetchedInstance = await i.GetAsync(instance.Name);

            //then
            fetchedInstance.Name.ShouldBe(instance.Name);
            fetchedInstance.CreatedAt.ShouldBe(instance.CreatedAt);
            fetchedInstance.Description.ShouldBe(instance.Description);


        }


        [Fact]
        public async Task Add()
        {
            //given
            var i =  GetInstanceAdminstration();
            string name = "unittest" + Guid.NewGuid().ToString();

            //when
            var instance = await i.AddAsync(new NewInstance() { Name = name, Description = "desc of " + name});

            //then
            instance.Name.ShouldBe(name);
            instance.CreatedAt.ShouldBeGreaterThan(DateTime.UtcNow.AddMinutes(-1));
        }


        [Fact]
        public async Task Add_uniqnessviolated_throws()
        {
            //given
            var i =  GetInstanceAdminstration();
            string name = "unittest" + Guid.NewGuid().ToString();
            var instance = await i.AddAsync(new NewInstance() { Name = name, Description = "desc of " + name });

            //when
            var ex = Should.Throw<SyncanoException>(async () => { var newI = await i.AddAsync(new NewInstance() {Name = name, Description = "desc of " + name}); });

            //then
            ex.Message.ShouldContain("Instance with this Name already exists");
          
        }



        [Fact(Skip = "post not supprote? change?")]
        public async Task Update()
        {
            //given
            var i = GetInstanceAdminstration();
            string name = "unittest" + Guid.NewGuid().ToString();
            var instance = await i.AddAsync(new NewInstance() { Name = name, Description = "desc of " + name });

            //when
            instance.Description = "Updated description";
            instance.Name = "newname" + Guid.NewGuid().ToString();

            var updatedInstance = await i.UpdateAsync(name, instance);

            //then
            updatedInstance.Name.ShouldBe(instance.Name);
            updatedInstance.Description.ShouldBe(instance.Description);
        }
    }
}