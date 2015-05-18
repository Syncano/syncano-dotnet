#if dotNET
using System.Threading.Tasks;
#endif
using System.Collections.Generic;

namespace Syncano4.Shared
{
    public interface ISyncanoHttpClient
    {
#if Unity3d
        string Get(string methodName, IDictionary<string,object> parameters);

        T Get<T>(string link);

        void Delete(string link);

        SyncanoResponse<T> List<T>(string link, IDictionary<string, object> parameters);

        T Post<T>(string instances, IRequestContent requestContent);

        T Patch<T>(string instances, IRequestContent requestContent);
#endif


#if dotNET
        Task<string> GetAsync(string methodName, IDictionary<string, object> parameters);

        Task DeleteAsync(string link);

        Task<SyncanoResponse<T>> ListAsync<T>(string methodName, IDictionary<string, object> parameters);

        Task<T> GetAsync<T>(string link);

        Task<T> PostAsync<T>(string instances, IRequestContent requestContent);

        Task<T> PatchAsync<T>(string instances, IRequestContent requestContent);

#endif
    }
}