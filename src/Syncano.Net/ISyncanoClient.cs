using System.Threading.Tasks;

namespace Syncano.Net
{
    public interface ISyncanoClient
    {
        Task<bool> GetAsync(string methodName, object parameters);

        Task<T> GetAsync<T>(string methodName, string contentToken);
        Task<T> GetAsync<T>(string methodName, object parameters, string contentToken);
        Task<T> PostAsync<T>(string projectNew, object parameters, string contentToken);
    }
}