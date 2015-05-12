using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Syncano4.Shared.Serialization;


namespace Syncano4.Shared
{
    public class DataObject: IEquatable<DataObject>
    {
        [SyncanoField("id", Ignore = true)]
        public int Id { get; protected set; }

        [SyncanoField("revision", Ignore = true)]
        public int Revision { get; protected set; }
        
        [SyncanoField("created_at", Ignore = true)]
        public DateTime CreatedAt { get; protected set; }

        [SyncanoField("updated_at", Ignore = true)]
        public DateTime UpdatedAt { get; protected set; }
        
        [SyncanoField("links", Ignore = true)]
        public Dictionary<string, string> Links { get; protected set; }

        
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