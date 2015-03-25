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
            string instanceName = "UnityLibraryDemo" + DateTime.Now.Ticks; //just to be unique for test

            //use syncano
            var syncano = new Syncano(authKey);

            //add instance
            var instance = syncano.Instances.Add(new NewInstance() {Name = instanceName, Description = "My sample instance"});

            //instance created
            instance.Name.ShouldBe(instanceName);
        }


       /* [Fact]
        public void CreateSchema()
        {
            //given an existingInstance
            string existingInstance = TestFactory.CreateInstance();
            string authKey = "a1546d926e32a940a57cc6dc68a22fc40a3ae7f6";

            //use syncano
            var syncano = new Syncano(authKey);

            //get instance
            var instance = syncano.Instances.Get() (new NewInstance() { Name = instanceName, Description = "My sample instance" });

            //instance created
            instance.Name.ShouldBe(instanceName);
        }*/


       
        
    }

    public class TestFactory
    {
       static string _authKey = TestData.AccountKey;

        public static string CreateInstance()
        {
            
            string instanceName = "UnityLibraryDemo" + DateTime.Now.Ticks; //just to be unique for test

            var syncano = new Syncano(_authKey);
            var instance = syncano.Instances.Add(new NewInstance() { Name = instanceName, Description = "My sample instance" });

            return instance.Name;
        }

    }
}