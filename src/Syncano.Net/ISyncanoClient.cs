using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Syncano.Net
{
    public interface ISyncanoClient
    {
        Task<T> GetAsync<T>(string methodName, string contentToken, Func<JToken, T> getResult);
        Task<T> GetAsync<T>(string methodName, object query, string contentToken, Func<JToken, T> getResult);
    }
}