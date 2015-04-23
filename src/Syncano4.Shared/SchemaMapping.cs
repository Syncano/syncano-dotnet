using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

#if Unity3d
using Syncano4.Unity3d;
#endif

namespace Syncano4.Shared
{
    public class SchemaMapping
    {
        public static List<FieldDef> GetSchema<T>()
        {
            var fieldDefs = new List<FieldDef>();
            var properties = GetProperties<T>();
            foreach (var propertyInfo in properties)
            {
                fieldDefs.Add(new FieldDef() { Name = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>().PropertyName, Type = GetSyncanoType(propertyInfo.PropertyType) });
            }
            return fieldDefs;
        }

#if Unity3d
        private static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            var properties = typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(p => p.GetCustomAttribute<JsonPropertyAttribute>() != null);
            return properties;
        }


        
#endif

#if dotNET
        private static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            var properties = typeof (T).GetTypeInfo().DeclaredProperties.Where(p => p.GetCustomAttribute<JsonPropertyAttribute>() != null);
            return properties;
        }
#endif

        private static FieldType GetSyncanoType(Type propertyType)
        {
            if (propertyType == typeof(int))
                return FieldType.Integer;

            if (propertyType == typeof(string))
                return FieldType.String;
            
            if (propertyType == typeof(bool))
                return FieldType.Boolean;

            if (propertyType == typeof(DateTime))
                return FieldType.Datetime;

            if (propertyType == typeof(float))
                return FieldType.Float;

            throw new NotSupportedException(string.Format("Type: {0} is not supported by syncano", propertyType.FullName));
        }
    }
}
