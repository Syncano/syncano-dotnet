using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Syncano4.Shared.Serialization;
#if Unity3d
using Syncano4.Unity3d;

#endif

namespace Syncano4.Shared
{
    public class SchemaMapping
    {
        public static List<FieldDef> GetSchema<T>()
        {
            return GetSchema(typeof (T));
        }

        public static List<FieldDef> GetSchema(Type type, bool includeSystemFields = false)
        {
            var fieldDefs = new List<FieldDef>();
            var properties = GetProperties(type);
            foreach (var propertyInfo in properties)
            {
                var syncanoFieldAttribute = propertyInfo.GetCustomAttribute<SyncanoFieldAttribute>();
                if (includeSystemFields || syncanoFieldAttribute.Ignore == false)
                {
                    fieldDefs.Add(new FieldDef()
                    {
                        Name = syncanoFieldAttribute.Name,
                        Type = GetSyncanoType(propertyInfo.PropertyType),
                        PropertyInfo = propertyInfo,
                        CanBeFiltered = syncanoFieldAttribute.CanBeFiltered,
                        CanBeOrdered = syncanoFieldAttribute.CanBeOrdered
                    });
                }
            }
            return fieldDefs;
        }

#if Unity3d
        public static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute<SyncanoFieldAttribute>() != null);
            return properties;
        }


#endif

#if dotNET
        public static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            var properties = type.GetRuntimeProperties().Where(p => p.GetCustomAttribute<SyncanoFieldAttribute>() != null);
            return properties;
        }
#endif

        private static FieldType GetSyncanoType(Type propertyType)
        {
            if (propertyType == typeof (int) || propertyType == typeof (long) || propertyType == typeof (short))
                return FieldType.Integer;

            if (propertyType == typeof (string))
                return FieldType.String;

            if (propertyType == typeof (bool))
                return FieldType.Boolean;

            if (propertyType == typeof (DateTime))
                return FieldType.Datetime;

            if (propertyType == typeof (float))
                return FieldType.Float;

            return FieldType.NotSet;
        }
    }
}