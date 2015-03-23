using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
    public class ClassDefinitionsTests
    {
        public ISyncanoHttpClient GetClient()
        {
            return new SyncanoHttpClient(TestData.AccountKey);
        }


        [Fact]
        public async Task Get()
        {
            //given
            var classDefintions = new ClassDefinitions("/v1/instances/testinstance2/classes/", GetClient());

            //when
            var classes = await classDefintions.GetAsync();

            //then
            classes.ShouldAllBe(c => c.Name != null);
        }

        public class TestObject
        {
            [JsonProperty("myid")]
            public long MyId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("createdat")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty("ischecked")]
            public bool IsChecked { get; set; }

            [JsonProperty("float")]
            public float Float { get; set; }

            [JsonProperty("longtext")]
            public string LongText { get; set; }

            public static List<SyncanoFieldSchema> GetSchema()
            {
                return new List<SyncanoFieldSchema>()
                {
                    new SyncanoFieldSchema() {Name = "myid", Type = SyncanoFieldType.Integer},
                    new SyncanoFieldSchema() {Name = "name", Type = SyncanoFieldType.String},
                    new SyncanoFieldSchema() {Name = "createdat", Type = SyncanoFieldType.Datetime},
                    new SyncanoFieldSchema() {Name = "ischecked", Type = SyncanoFieldType.Boolean},
                    new SyncanoFieldSchema() {Name = "float", Type = SyncanoFieldType.Float},
                    new SyncanoFieldSchema() {Name = "longtext", Type = SyncanoFieldType.Text},
                };
            }
        }


        [Fact]
        public async Task Add()
        {
            //given
            var classDefintions = new ClassDefinitions("/v1/instances/testinstance2/classes/", GetClient());
            var schema = TestObject.GetSchema();

            //when

            var classDef = await classDefintions.AddAsync("ClassUnitTest_" + Guid.NewGuid().ToString(), "generated in unittest", schema);

            //then
            classDef.Schema.ShouldBeSubsetOf(schema);
            classDef.Schema.Count.ShouldBe(schema.Count);
        }


        [Fact]
        public async Task AddObject()
        {
            //given
            var classDefintions = new ClassDefinitions("/v1/instances/testinstance2/classes/", GetClient());
            var classDef = await classDefintions.AddAsync("ClassUnitTest_" + Guid.NewGuid().ToString(), "generated in unittest", TestObject.GetSchema());
            var objects = new SyncanoDataObjects(classDef,GetClient());

            //when
            var expectedObject  = new TestObject() { Name = "Name 1", CreatedAt = DateTime.UtcNow};
            var newTestObject = await objects.AddAsync(expectedObject);
            
            //then

            newTestObject.CreatedAt.ShouldBe(expectedObject.CreatedAt,TimeSpan.FromTicks(10));
            newTestObject.Name.ShouldBe(expectedObject.Name);
        }


        [Fact]
        public async Task ListObjects()
        {
            //given
            var classDefintions = new ClassDefinitions("/v1/instances/testinstance2/classes/", GetClient());
            var classDef = await classDefintions.AddAsync("ClassUnitTest_" + Guid.NewGuid().ToString(), "generated in unittest", TestObject.GetSchema());
            var objects = new SyncanoDataObjects(classDef, GetClient());

            for (int i = 0; i < 20; i++)
            {
               await objects.AddAsync(new TestObject() { Name = "Name " + i, CreatedAt = DateTime.UtcNow, MyId = i});
            }

            //when
            var list = await objects.GetAsync<TestObject>(pageSize: 10);

            //then
            list.Count.ShouldBe(10);
            list.ShouldAllBe(t => t.MyId < 10);

        }
    }
}
