using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Shouldly;
using Syncano4.Shared;
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
                .Instances.Add(new NewInstance() {Name = instanceNameToCreate, Description = "My sample instance"});

            //instance created
            newInstance.Name.ShouldBe(instanceNameToCreate);
        }


        /// <summary>
        /// sample object to store in syncano
        /// </summary>
        public class SampleObject : DataObject
        {
            [JsonProperty("order")]
            public int Order { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
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
            var classDef = instanceResources.Schema.Add(NewClass.From<SampleObject>());
                

            //verify
            classDef.Name.ShouldBe("SampleObject",Case.Insensitive);
            classDef.Schema.ShouldContain(new FieldDef() {Name = "order", Type = FieldType.Integer});
            classDef.Schema.ShouldContain(new FieldDef() {Name = "name", Type = FieldType.String});
        }

       


        [Fact]
        public void AddObject()
        {
            //given an existingInstance
            string authKey = "a1546d926e32a940a57cc6dc68a22fc40a3ae7f6";
            string existingInstance = TestFactory.CreateInstance();
            string existingClass = TestFactory.CreateClass(existingInstance, typeof (SampleObject));

            //switch to instance 
            var instanceResources = Syncano.Using(authKey).ResourcesFor(existingInstance);

            //create some object
            var sampleObject = new SampleObject() {Name = "Demo object 1", Order = 1};

            //add object to instance
            var addedObject = instanceResources.Objects<SampleObject>().Add(sampleObject);

            //list objects
            var objectList = instanceResources.Objects<SampleObject>().List(pageSize: 5);

            //verify
            addedObject.Id.ShouldBeGreaterThan(0);
            addedObject.CreatedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            addedObject.Name.ShouldBe("Demo object 1");

            objectList.Count.ShouldBe(1);
        }
    }
}