using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Syncano4.Shared
{
    public class SyncanoClass
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; }

        
        [JsonProperty("objects_count")]
        public string ObjectsCount { get; set; }

        [JsonProperty("schema")]
        public IList<SyncanoFieldSchema> Schema { get; set; }
    }

    public class CamelCaseStringEnumConverter : StringEnumConverter
    {
        public CamelCaseStringEnumConverter()
        {
            this.CamelCaseText = true;
        }
    }

    

    public class SyncanoFieldSchema : IEquatable<SyncanoFieldSchema>
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(CamelCaseStringEnumConverter))]
        public SyncanoFieldType Type { get; set; }


        [JsonProperty("name")]
        public string Name { get; set; }





        [JsonProperty("target", NullValueHandling=NullValueHandling.Ignore) ]
        public string Target { get; set; }

        public bool Equals(SyncanoFieldSchema other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && Type == other.Type && string.Equals(Target, other.Target);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SyncanoFieldSchema) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) Type;
                hashCode = (hashCode*397) ^ (Target != null ? Target.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Target: {1}, Type: {2}", Name, Target, Type.ToString().ToLower());
        }
    }

    public enum SyncanoFieldType
    {
        Text, String, Integer, Float, Boolean, Datetime, File, Reference
    }
}