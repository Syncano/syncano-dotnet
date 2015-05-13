using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
#if Unity3d
using Syncano4.Unity3d;
#endif

namespace Syncano4.Shared.Serialization
{
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

        public static T DeserializeObject<T>(string json)
        {
            var jsonSettings = new JsonSerializerSettings();

            if (typeof (T).GetTypeInfo().IsSubclassOf(typeof (DataObject)))
                jsonSettings.ContractResolver = new SyncanoJsonContractResolver();

            return JsonConvert.DeserializeObject<T>(json, jsonSettings);
        }
    }
}
