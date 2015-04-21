using System.Text;
#if Unity3d
using Syncano4.Unity3d;

#endif
#if dotNET
using Syncano4.Net;
using System.Threading.Tasks;
#endif

namespace Syncano4.Shared
{
    public class Syncano
    {
        private readonly string _authkey;
        private readonly ISyncanoHttpClient _httpClient;

        public static Syncano Using(string authkey)
        {
            return new Syncano(authkey);
        }

        public Syncano(string authkey)
        {
            _authkey = authkey;
            _httpClient = new SyncanoHttpClient(authkey);
        }

        public Administration Administration
        {
            get { return new Administration(_httpClient); }
        }

        public InstanceResources ResourcesFor(string existingInstance)
        {
            return new InstanceResources(this, _httpClient, existingInstance);
        }
    }
}