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
        public InstanceResources CreateInstance()
        {
            var syncano = Syncano.Using(TestData.AccountKey);
            var name = "UnitTest_" + DateTime.UtcNow.ToFileTime();
            syncano.Administration.Instances.AddAsync(new NewInstance() {Name = name}).Wait(TimeSpan.FromSeconds(20));

            return syncano.ResourcesFor(name);
        }


        [Fact]
        public async Task Get()
        {
            //given
            var classDefintions = CreateInstance().Schema;

            //when
            var classes = await classDefintions.ListAsync();

            //then
            classes.ShouldAllBe(c => c.Name != null);
        }

        public class TestObject : DataObject
        {
            [JsonProperty("myid")]
            public long MyId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("current_time")]
            public DateTime CurrentTime { get; set; }

            [JsonProperty("ischecked")]
            public bool IsChecked { get; set; }

            [JsonProperty("float")]
            public float Float { get; set; }

            [JsonProperty("longtext")]
            public string LongText { get; set; }

            public static List<FieldDef> GetSchema()
            {
                return new List<FieldDef>()
                {
                    new FieldDef() {Name = "myid", Type = FieldType.Integer},
                    new FieldDef() {Name = "name", Type = FieldType.String},
                    new FieldDef() {Name = "current_time", Type = FieldType.Datetime},
                    new FieldDef() {Name = "ischecked", Type = FieldType.Boolean},
                    new FieldDef() {Name = "float", Type = FieldType.Float},
                    new FieldDef() {Name = "longtext", Type = FieldType.Text},
                };
            }
        }


        [Fact]
        public async Task Add()
        {
            //given
            var classDefintions = CreateInstance().Schema;
            var schema = TestObject.GetSchema();

            //when

            var classDef = await classDefintions.AddAsync(new NewClass()
            {
                Name = "ClassUnitTest_" + Guid.NewGuid().ToString(),
                Description = "generated in unittest",
                Schema = schema
            });

            //then
            classDef.Schema.ShouldBeSubsetOf(schema);
            classDef.Schema.Count.ShouldBe(schema.Count);
        }

        [Fact]
        
        public async Task Update_description()
        {
            //given
            var classDefintions = CreateInstance().Schema;
            var schema = TestObject.GetSchema();

            var classDef = await classDefintions.AddAsync(new NewClass()
            {
                Name = "ClassUnitTest_" + Guid.NewGuid().ToString(),
                Description = "generated in unittest",
                Schema = schema
            });

            //when
            classDef.Description = "new description" + DateTime.UtcNow;
            var updatedClassDef = await classDefintions.PatchAsync(classDef.Name, classDef);

            //then
            updatedClassDef.Name.ShouldBe(classDef.Name);
            updatedClassDef.Description.ShouldBe(classDef.Description);
        }
                 

    }
}