using System.Collections.Generic;
#if dotNET
using Syncano4.Net;
using System.Threading.Tasks;
#endif

namespace Syncano4.Shared
{
    public interface ILazyLinkProvider
    {
        Dictionary<string, string> Links { get; }

#if dotNET
        Task Initialize();
#endif

#if Unity3d
        void Initialize();
#endif

    }
}