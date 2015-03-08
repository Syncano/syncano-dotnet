#if dotNet
    using System.Threading.Tasks;
#endif

using System.Collections.Generic;

namespace Syncano4.Shared
{
    public interface ISyncanoHttpClient
    {

#if Unity3d
        string Get(string methodName, IDictionary<string,object> parameters);
        IList<T> Get<T>(string methodName, IDictionary<string, object> parameters);

        T Post<T>(string instances, IDictionary<string, object> parameters);
#endif


#if dotNet 
        Task<string> GetAsync(string methodName, IDictionary<string,object> parameters);
             Task<IList<T>> GetAsync<T>(string methodName, IDictionary<string,object> parameters);


         Task<T> PostAsync<T>(string instances, IDictionary<string,object> parameters);

#endif



    }
}