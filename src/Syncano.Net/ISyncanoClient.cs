using System.Threading.Tasks;

namespace Syncano.Net
{
    /// <summary>
    /// Interface defining possible methods of communication with Syncano.
    /// </summary>
    public interface ISyncanoClient
    {
        Task<bool> GetAsync(string methodName, object parameters);

        Task<T> GetAsync<T>(string methodName, string contentToken);
        Task<T> GetAsync<T>(string methodName, object parameters, string contentToken);
        Task<T> PostAsync<T>(string projectNew, object parameters, string contentToken);
    }
}