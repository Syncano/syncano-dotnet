using System;

namespace Syncano4.Shared.Serialization
{
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