using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Shouldly;
using Syncano4.Shared;
using Syncano4.Shared.Serialization;
using Xunit;

namespace Syncano4.Tests.Unity3d
{
    public class SyncanoExamples
    {
        private const string TestAuthKey = "db79a6ac3a949e6a42cc0b60c3884b9bff7b7820";

        [Fact]
        public void CreateInstance()
        {
            //given
            string authKey = TestAuthKey;
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
            [SyncanoField("order", CanBeOrdered = true, CanBeFiltered = true)]
            public int Order { get; set; }

            [SyncanoField("name", CanBeOrdered = true, CanBeFiltered = true)]
            public string Name { get; set; }
        }



        [Fact]
        public void CreateSchema()
        {
            //given an existingInstance
            string existingInstance = TestFactory.CreateInstance();
            string authKey = TestAuthKey;

            //switch to instance 
            var instanceResources = Syncano.Using(authKey).ResourcesFor(existingInstance);

            //create class with two simple properties. 
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
            string authKey = TestAuthKey;
            string existingInstance = TestFactory.CreateInstance();
            string existingClass = TestFactory.CreateClass<SampleObject>(existingInstance);

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


        [Fact]
        public void FilterObjects()
        {
            //given an existingInstance
            string authKey = TestAuthKey;
            string existingInstance = TestFactory.CreateInstance();
            string existingClass = TestFactory.CreateClass<SampleObject>(existingInstance);

            //switch to instance 
            var instanceResources = Syncano.Using(authKey).ResourcesFor(existingInstance);

            //create some object
            for (int i = 0; i < 10; i++)
            {
                var sampleObject = new SampleObject() { Name = "Demo object " + i, Order = i };
                instanceResources.Objects<SampleObject>().Add(sampleObject);
            }
            
            
            //list objects
            var objectList = instanceResources.Objects<SampleObject>().CreateQuery()
                .Where(s => s.Order > 5)
                .Where(s => s.Order <= 7)
                .OrderByDescending(s => s.Name)
                .ToList();

            //verify
            objectList.Current.Count.ShouldBe(2);
            objectList.Current[0].Order.ShouldBe(7);
            objectList.Current[1].Order.ShouldBe(6);
        }
    }
}