using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
#if dotNET
using System.Reflection;
#endif

#if Unity3d
using Syncano4.Unity3d;
#endif

namespace Syncano4.Shared.Serialization
{
    public class SyncanoJsonConverter
    {
        private const string DateFormatString = "yyyy-MM-ddTHH:mm:ss.ffffffZ";

        public static string Serialize(object o)
        {
            var jsonSettings = new JsonSerializerSettings() { ContractResolver = new SyncanoJsonContractResolver(shouldIncludeSystemFields:false, convertSyncanoDatetimes:false), DateFormatString = DateFormatString};
            return JsonConvert.SerializeObject(o, jsonSettings);
        }

        public static IDictionary<string, object> ToDictionary(object objectToSerialize)
        {
            if(objectToSerialize is DataObject)
                return new Dictionary<string, object>() { {"body", (Serialize(objectToSerialize)) } };
            else
            {
                var json = JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings() { DateFormatString = DateFormatString });
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
        }

        public static T DeserializeObject<T>(string json)
        {
            var jsonSettings = new JsonSerializerSettings();

            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof (DataObject)))
                jsonSettings.ContractResolver = new SyncanoJsonContractResolver(shouldIncludeSystemFields:true, convertSyncanoDatetimes:true);

            return JsonConvert.DeserializeObject<T>(json, jsonSettings);
        }
    }
}
