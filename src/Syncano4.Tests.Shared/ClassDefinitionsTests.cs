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


        [Fact]
        public async Task Add()
        {
            //given
            var classDefintions = new ClassDefinitions("/v1/instances/testinstance2/classes/", GetClient());
            var schema = new List<SyncanoFieldSchema>()
            {
                new SyncanoFieldSchema() {Name = "myid", Type = SyncanoFieldType.Integer},
                new SyncanoFieldSchema() {Name = "name", Type = SyncanoFieldType.String},
                new SyncanoFieldSchema() {Name = "createdat", Type = SyncanoFieldType.Datetime},
                new SyncanoFieldSchema() {Name = "ischecked", Type = SyncanoFieldType.Boolean},
               new SyncanoFieldSchema() {Name = "float", Type = SyncanoFieldType.Float},
                new SyncanoFieldSchema() {Name = "longtext", Type = SyncanoFieldType.Text},
            };

            //when
            
            var classDef = await classDefintions.AddAsync("ClassUnitTest_" + Guid.NewGuid().ToString(), "generated in unittest", schema);

            //then
            classDef.Schema.ShouldBeSubsetOf(schema);
            classDef.Schema.Count.ShouldBe(schema.Count);

        }

      
    }
}