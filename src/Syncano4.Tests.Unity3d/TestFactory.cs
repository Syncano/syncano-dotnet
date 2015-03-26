using System;
using Syncano4.Shared;
using Syncano4.Tests.Shared;

namespace Syncano4.Tests.Unity3d
{
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

        public static string CreateClass(string existingInstance, Type type)
        {
            var instanceResources = Syncano.Using(_authKey).ResourcesFor(existingInstance);

            var classDef = instanceResources.Schema.Add(
                new NewClass(type.Name,
                    new FieldDef() {Name = "order", Type = FieldType.Integer},
                    new FieldDef() {Name = "name", Type = FieldType.String})
                );

            return classDef.Name;
        }
    }
}