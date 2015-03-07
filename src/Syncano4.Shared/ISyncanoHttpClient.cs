#if dotNet
    using System.Threading.Tasks;
#endif

using System.Collections.Generic;
namespace Syncano4.Shared
{
    public interface ISyncanoHttpClient
    {

#if Unity3d
        string Get(string methodName, object parameters);
        IList<T> Get<T>(string methodName, object parameters);
#endif


#if dotNet 
        Task<string> GetAsync(string methodName, object parameters);
             Task<IList<T>> GetAsync<T>(string methodName, object parameters);

#endif




    }
           
}