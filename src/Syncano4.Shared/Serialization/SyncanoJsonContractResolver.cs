using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Syncano4.Shared.Serialization
{
    public class SyncanoJsonContractResolver : DefaultContractResolver
    {
        private readonly bool _shouldIncludeSystemFields;
        private readonly bool _convertSyncanoDatetimes;

        public SyncanoJsonContractResolver(bool shouldIncludeSystemFields = true, bool convertSyncanoDatetimes = true)
        {
            _shouldIncludeSystemFields = shouldIncludeSystemFields;
            _convertSyncanoDatetimes = convertSyncanoDatetimes;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = new List<JsonProperty>();
            var fields = SchemaMapping.GetSchema(type, includeSystemFields: _shouldIncludeSystemFields);

            foreach (var eachField in fields)
            {
                properties.Add(CreateProperty(eachField.PropertyInfo, memberSerialization));
            }

            return properties;
        }


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            var syncanoFieldAttribute =
                (SyncanoFieldAttribute) member.GetCustomAttributes(typeof (SyncanoFieldAttribute), false).Single();

            if (property.PropertyType == typeof (DateTime) && !syncanoFieldAttribute.Ignore)
            {
                if (_convertSyncanoDatetimes)
                {
                    property.Converter = new SyncanoDatetimeConverter();
                    property.MemberConverter = new SyncanoDatetimeConverter();
                }
            }
            property.PropertyName = syncanoFieldAttribute.Name;
            property.HasMemberAttribute = true;
            property.Writable = true;
            return property;

        }
    }
}