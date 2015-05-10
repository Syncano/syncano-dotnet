using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Syncano4.Shared.Serialization
{
    public class SyncanoJsonContractResolver:DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
             var properties = new List<JsonProperty>();
            var fields   = SchemaMapping.GetSchema(type, includeSystemFields:true);

            foreach (var eachField in fields)
            {
                properties.Add(CreateProperty(eachField.PropertyInfo, memberSerialization));
            }

            return properties;
        }


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            var syncanoFieldAttribute = (SyncanoFieldAttribute) member.GetCustomAttributes(typeof (SyncanoFieldAttribute), false).Single();

            property.PropertyName = syncanoFieldAttribute.Name;

            return property;

        }
    }


    public class SyncanoJsonConverter
    {
        public static string Serialize(object o)
        {
            var jsonSettings = new JsonSerializerSettings() { ContractResolver = new SyncanoJsonContractResolver() };
            return JsonConvert.SerializeObject(o, jsonSettings);
        }

        public static IDictionary<string, object> ToDictionary(object objectToSerialize)
        {
            if(objectToSerialize is DataObject)
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(Serialize(objectToSerialize));
            else
            {
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(objectToSerialize));
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SyncanoFieldAttribute : Attribute
    {
        public SyncanoFieldAttribute(string name)
        {
            Name = name;
        }

        public SyncanoFieldAttribute()
        {
            
        }
        public string Name { get; set; }

        public bool Ignore { get; set; }

        public bool CanBeOrdered { get; set; }
        public bool CanBeFiltered { get; set; }


    }
}
