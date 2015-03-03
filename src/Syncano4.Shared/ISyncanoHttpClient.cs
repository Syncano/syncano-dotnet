#if dotNet
    using System.Threading.Tasks;
#endif

namespace Syncano4.Shared
{
    public interface ISyncanoHttpClient
    {

#if Unity3d
        string Get(string methodName, object parameters);
#endif


#if dotNet 
        Task<string> GetAsync(string methodName, object parameters);
#endif
    }
           
}