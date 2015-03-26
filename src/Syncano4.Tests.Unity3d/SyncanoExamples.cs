using System;
using Shouldly;
using Syncano4.Shared;
using Syncano4.Tests.Shared;
using Xunit;

namespace Syncano4.Tests.Unity3d
{
    public class SyncanoExamples
    {
        [Fact]
        public void CreateInstance()
        {
            //given
            string authKey = "a1546d926e32a940a57cc6dc68a22fc40a3ae7f6";
            string instanceNameToCreate = "unity3d_demo_" + DateTime.Now.Ticks; //just to be unique for test

            //add instanace using instance administration
            var newInstance = Syncano.Using(authKey)
                                        .Administration
                                        .Instances.Add(new NewInstance() { Name = instanceNameToCreate, Description = "My sample instance" });

            //instance created
            newInstance.Name.ShouldBe(instanceNameToCreate);
        }
        
        [Fact]
        public void CreateSchema()
        {
            //given an existingInstance
            string existingInstance = TestFactory.CreateInstance();
            string authKey = "a1546d926e32a940a57cc6dc68a22fc40a3ae7f6";

            //switch to instance 
            var instanceResources = Syncano.Using(authKey).ResourcesFor(existingInstance);

            //create class with two simple properties.  (at this moment manual mapping is required - this will change in the near future )
             var classDef = instanceResources.Schema.Add(
                new NewClass("sample_class", 
                    new FieldDef() {Name = "order", Type = FieldType.Integer},
                    new FieldDef() {Name = "name", Type = FieldType.String})
                    );

            //verify
            classDef.Name.ShouldBe("sample_class");
            classDef.Schema.ShouldContain(new FieldDef() { Name = "order", Type = FieldType.Integer });
            classDef.Schema.ShouldContain(new FieldDef() { Name = "name", Type = FieldType.String });
        }
    }

    public class TestFactory
    {
        private static string _authKey = TestData.AccountKey;

        public static string CreateInstance()
        {
            string instanceName = "UnityLibraryDemo" + DateTime.Now.Ticks; //just to be unique for test

            var syncano = new Syncano(_authKey);
            var instance = syncano.Administration.Instances.Add(new NewInstance() {Name = instanceName, Description = "My sample instance"});

            return instance.Name;
        }
    }
}