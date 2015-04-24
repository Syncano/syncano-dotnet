using System.Collections.Generic;
using Newtonsoft.Json;

namespace Syncano4.Shared
{
    public class SyncanoSerializer
    {
        public IDictionary<string, object> ToDictionary(object objectToSerialize)
        {
            var jsonString = JsonConvert.SerializeObject(objectToSerialize);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            
            //temporary solution
            if (objectToSerialize is DataObject)
            {
                dictionary.Remove("id");
                dictionary.Remove("revision");
                dictionary.Remove("created_at");

                dictionary.Remove("updated_at");
                dictionary.Remove("links");
            }

            if (objectToSerialize is Instance)
            {
                dictionary.Remove("links");
                dictionary.Remove("created_at");
            }
            return dictionary;

        }

    }
}