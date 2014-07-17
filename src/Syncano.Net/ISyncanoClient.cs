using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Syncano.Net
{
    public interface ISyncanoClient
    {
        Task<bool> GetAsync(string methodName, object parameters);

        Task<T> GetAsync<T>(string methodName, string contentToken, Func<JToken, T> getResult);
        Task<T> GetAsync<T>(string methodName, object parameters, string contentToken, Func<JToken, T> getResult);
        Task<T> PostAsync<T>(string projectNew, object parameters, string contentToken, Func<JToken, T> getResult);
    }
}