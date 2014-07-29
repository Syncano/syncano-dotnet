using System;
using System.Threading.Tasks;

namespace Syncano.Net
{
    /// <summary>
    /// Interface defining possible methods of communication with Syncano.
    /// </summary>
    public interface ISyncanoClient
    {
        /// <summary>
        /// Method of sending messages to Syncano.
        /// </summary>
        /// <param name="methodName">Name of Syncano Rest Api method.</param>
        /// <param name="parameters">Object containg proper parameters.</param>
        /// <returns>Boolean value indicating operation success.</returns>
        Task<bool> GetAsync(string methodName, object parameters);

        /// <summary>
        /// Method of sending messages to Syncano.
        /// </summary>
        /// <typeparam name="T">Type to retrieve from Syncano.</typeparam>
        /// <param name="methodName">Name of Syncano Rest Api method.</param>
        /// <param name="contentToken">Token of response message marking object to retrieve.</param>
        /// <returns>Retrived object.</returns>
        Task<T> GetAsync<T>(string methodName, string contentToken);

        /// <summary>
        /// Method of sending messages to Syncano.
        /// </summary>
        /// <typeparam name="T">Type to retrieve from Syncano.</typeparam>
        /// <param name="methodName">Name of Syncano Rest Api method.</param>
        /// <param name="parameters">Object containg proper parameters.</param>
        /// <param name="contentToken">Token of response message marking object to retrieve.</param>
        /// <returns>Retrived object.</returns>
        Task<T> GetAsync<T>(string methodName, object parameters, string contentToken);

        /// <summary>
        /// Method of posting messages to Syncano.
        /// </summary>
        /// <typeparam name="T">Type to retrieve from Syncano.</typeparam>
        /// <param name="methodName">Name of Syncano Rest Api method.</param>
        /// <param name="parameters">Object containg proper parameters.</param>
        /// <param name="contentToken">Token of response message marking object to retrieve.</param>
        /// <returns>Retrived object.</returns>
        Task<T> PostAsync<T>(string methodName, object parameters, string contentToken);

        /// <summary>
        /// Sets user AuthKey. It is called by Login method.
        /// </summary>
        /// <param name="authKey">AuthKey string.</param>
        void SetUserContext(string authKey);

        /// <summary>
        /// Sets Session Id. It is called by StartSession.
        /// </summary>
        /// <param name="sessionId">Session Id string.</param>
        void SetSessionContext(string sessionId);
    }
}