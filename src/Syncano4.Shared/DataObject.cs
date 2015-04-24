using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;



namespace Syncano4.Shared
{
    public class DataObject: IEquatable<DataObject>
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("revision")]
        public int Revision { get; private set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; private set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; private set; }
        
        [JsonProperty("links")]
        public Dictionary<string, string> Links { get; private set; }
        
        public bool Equals(DataObject other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Revision == other.Revision;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataObject) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id*397) ^ Revision;
            }
        }

#if DEBUG
        public override string ToString()
        {
            return string.Format("Id: {0}, Revision: {1}", Id, Revision);
        }
#endif

    }
}