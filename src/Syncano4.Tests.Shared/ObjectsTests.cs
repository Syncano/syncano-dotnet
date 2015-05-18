using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Syncano4.Shared;
using Shouldly;
using Syncano4.Shared.Serialization;
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
    public class ObjectsTests
    {
        private InstanceResources _instanceResources;
        private SyncanoDataObjects<TestObject> _objectsRepository;

        public InstanceResources CreateInstance()
        {
            var syncano = Syncano.Using(TestData.AccountKey);
            var name = "UnitTest_" + DateTime.UtcNow.ToFileTime();
            syncano.Administration.Instances.AddAsync(new NewInstance() {Name = name}).Wait(TimeSpan.FromSeconds(20));

            return syncano.ResourcesFor(name);
        }

        
        public class TestObject : DataObject
        {
            [SyncanoField("myid", CanBeFiltered = true, CanBeOrdered = true)]
            public long MyId { get; set; }

            [SyncanoField("name")]
            public string Name { get; set; }

            [SyncanoField("current_time")]
            public DateTime CurrentTime { get; set; }

            [SyncanoField("ischecked")]
            public bool IsChecked { get; set; }

            [SyncanoField("float")]
            public float Float { get; set; }

            [SyncanoField("longtext")]
            public string LongText { get; set; }
            }

        public ObjectsTests()
        {
            _instanceResources = CreateInstance();
            _instanceResources.Schema.AddAsync(NewClass.From<TestObject>()).Wait(TimeSpan.FromSeconds(15));
            _objectsRepository = _instanceResources.Objects<TestObject>();
        }

        [Fact]
        public async Task AddObject()
        {
            //given
            var expectedObject = new TestObject() { Name = "Name 1", CurrentTime = DateTime.UtcNow };
            //when
            
            var newTestObject = await _objectsRepository.AddAsync(expectedObject);

            //then
            newTestObject.Id.ShouldBeGreaterThan(0);
            newTestObject.Revision.ShouldBeGreaterThan(0);
            newTestObject.UpdatedAt.ShouldBeGreaterThanOrEqualTo(DateTime.UtcNow.AddSeconds(-5));
            newTestObject.CreatedAt.ShouldBeGreaterThanOrEqualTo(DateTime.UtcNow.AddSeconds(-5));

            //newTestObject.CurrentTime.ShouldBe(expectedObject.CurrentTime, TimeSpan.FromTicks(10));
            newTestObject.Name.ShouldBe(expectedObject.Name);
        }



        [Fact]
        public async Task UpdateObject()
        {
            //given
            var expectedObject = new TestObject() { Name = "Name 1", CurrentTime = DateTime.UtcNow };
            var newTestObject = await _objectsRepository.AddAsync(expectedObject);
            //when
            newTestObject.Name = "UpdatedName";
            await _objectsRepository.UpdateAsync(newTestObject.Id, newTestObject);
            
            //then
            var updatedObject = await _objectsRepository.GetAsync(newTestObject.Id);

            updatedObject.Name.ShouldBe(newTestObject.Name);
            updatedObject.UpdatedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        }


        [Fact]
        public async Task ListObjects()
        {
            //given
            for (int i = 0; i < 20; i++)
            {
                await _objectsRepository.AddAsync(new TestObject() { Name = "Name " + i, CurrentTime = DateTime.UtcNow, MyId = i });
            }

            //when
            var list = await _objectsRepository.ListAsync(pageSize: 10);

            //then
            list.Count.ShouldBe(10);
            list.ShouldAllBe(t => t.MyId < 10);
        }


        [Fact]
        public async Task ListObjects_Pageable_GetNext()
        {
            //given
            for (int i = 0; i < 20; i++)
            {
                await _objectsRepository.AddAsync(new TestObject() { Name = "Name " + i, CurrentTime = DateTime.UtcNow, MyId = i });
            }

            //when
            var page1 = await _objectsRepository.PageableListAsync(pageSize: 10);
            var page2 = await page1.GetNextAsync();

            //then
            page1.Current.Count.ShouldBe(10);
            page1.Current.ShouldAllBe(t => t.MyId < 10);
            page1.HasPrevious.ShouldBe(false);
            page1.HasNext.ShouldBe(true);


            page2.HasPrevious.ShouldBe(true);
            page2.Current.Count.ShouldBe(10);
            page2.Current.ShouldAllBe(t => t.MyId < 20 && t.MyId >= 10);
        }

        [Fact]
        public async Task ListObjects_Pageable_GetNext_EmptyPage()
        {
            //given
            for (int i = 0; i < 10; i++)
            {
                await _objectsRepository.AddAsync(new TestObject() { Name = "Name " + i, CurrentTime = DateTime.UtcNow, MyId = i });
            }

            //when
            var page1 = await _objectsRepository.PageableListAsync(pageSize: 5);
            var page2 = await page1.GetNextAsync();
            var page3 = await page2.GetNextAsync();

            //then
            page3.Current.Count.ShouldBe(0);
        }


        [Fact]
        public async Task ListObjects_Pageable_GetPrevious()
        {
            //given
            for (int i = 0; i < 10; i++)
            {
                await _objectsRepository.AddAsync(new TestObject() { Name = "Name " + i, CurrentTime = DateTime.UtcNow, MyId = i });
            }

            //when
            var page1 = await _objectsRepository.PageableListAsync(pageSize: 5);
            var page2 = await page1.GetNextAsync();
            var page1again = await page2.GetPreviousAsync();

            //then
           page1.Current.ShouldBe(page1again.Current);
        }


        [Fact]
        public async Task DeleteObject()
        {
            //given
            var testObject =   await _objectsRepository.AddAsync(new TestObject() { Name = "Name", CurrentTime = DateTime.UtcNow, MyId = 1 });
            

            //when
            await _objectsRepository.DeleteAsync(testObject.Id.ToString());

            //then
            var deleteObject = await  _objectsRepository.GetAsync(testObject.Id);
            deleteObject.ShouldBe(null);
        }


        [Fact]
        public async Task Query_by_id_simple_equals()
        {
            //given
            for (int i = 0; i < 3; i++)
            {
                await _objectsRepository.AddAsync(new TestObject() { Name = "Name " + i, CurrentTime = DateTime.UtcNow, MyId = i });
            }

            //when
            var objects = await _objectsRepository.CreateQuery().Where(x => x.Id, 1).ToListAsync();

            //then

            objects.Current.Count.ShouldBe(1);
            objects.Current[0].Name.ShouldBe("Name 0");
            
        }

        [Fact]
        public async Task Query_simple_byfield_equals()
        {
            //given
            for (int i = 0; i < 3; i++)
            {
                await _objectsRepository.AddAsync(new TestObject() { Name = "Name " + i, CurrentTime = DateTime.UtcNow, MyId = i });
            }

            //when
            var objects = await _objectsRepository.CreateQuery().Where(x => x.MyId, 0).ToListAsync();

            //then

            objects.Current.Count.ShouldBe(1);
            objects.Current[0].Name.ShouldBe("Name 0");

        }

    }
}
