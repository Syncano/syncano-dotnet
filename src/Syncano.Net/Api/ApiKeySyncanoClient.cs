using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net.Access;

namespace Syncano.Net.Api
{
    public class ApiKeySyncanoClient
    {
        private readonly ISyncanoClient _syncanoClient;

        public ApiKeySyncanoClient(ISyncanoClient syncanoClient)
        {
            _syncanoClient = syncanoClient;
        }

        private string WindowsTzToIanaTz(TimeZoneInfo timeZoneInfo)
        {
            if (timeZoneInfo == null)
                return null;

            if (timeZoneInfo == TimeZoneInfo.Utc)
                return "Etc/UTC";

            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;
            
            var tzid = tzdbSource.MapTimeZoneId(timeZoneInfo);

            if (tzid == null)
            {
                throw new ArgumentException("Time zones marked as old are not supported");
            }

            return tzdbSource.CanonicalIdMap[tzid];
        }

        /// <summary>
        /// Lists all permission roles of current instance.
        /// </summary>
        /// <returns>List of Role objects.</returns>
        public async Task<List<Role>> GetRoles()
        {
            return await _syncanoClient.GetAsync<List<Role>>("role.get", "role");
        }

        /// <summary>
        /// Logs in the API client and returns session_id for session id or cookie-based authentication. Session is valid for 2 hours and is automatically renewed whenever it is used.
        /// <remarks>User API key usage permitted.</remarks>
        /// </summary>
        /// <param name="timeZone">Sets default timezone for all subsequent requests using returned session_id.</param>
        /// <returns>Assigned session id. This can be passed for subsequent calls as authentication.</returns>
        public Task<string> StartSession(TimeZoneInfo timeZone = null)
        {
            return _syncanoClient.PostAsync<string>("apikey.start_session", new {timezone = WindowsTzToIanaTz(timeZone)}, "session_id");
        }

        /// <summary>
        /// Creates a new API client (for backend or user-aware usage) in current instance. Only Admin permission role can create new API clients.
        /// </summary>
        /// <param name="description">Description of new API client.</param>
        /// <param name="type">Type of new API client.</param>
        /// <param name="roleId">New API client's permission role id (see role.get()). Not used when creating User API key (type = user)</param>
        /// <returns>New User object.</returns>
        public Task<ApiKey> New(string description, ApiKeyType type = ApiKeyType.Backend, string roleId = null)
        {
            if(description == null)
                throw new ArgumentNullException();

            if(type == ApiKeyType.User && roleId != null)
                throw new ArgumentException();

            return _syncanoClient.PostAsync<ApiKey>("apikey.new", new {description, type = type.ToString(), role_id = roleId}, "apikey");
        }

        /// <summary>
        /// Get API clients. Only Admin permission role can view other API clients.
        /// </summary>
        /// <returns>List of ApiKey objects.</returns>
        public async Task<List<ApiKey>> Get()
        {
            return await _syncanoClient.GetAsync<List<ApiKey>>("apikey.get", "apikey");
        }

        /// <summary>
        /// Gets info of one specified API client. Only Admin permission role can view other API clients.
        /// </summary>
        /// <param name="apiClientId">API client id. If not specified, will use current API client.</param>
        /// <returns>ApiKey object.</returns>
        public Task<ApiKey> GetOne(string apiClientId = null)
        {
            return _syncanoClient.GetAsync<ApiKey>("apikey.get_one", new {api_client_id = apiClientId}, "apikey");
        }

        /// <summary>
        /// Updates specified API client's info.
        /// </summary>
        /// <param name="description">New API client's description to set.</param>
        /// <param name="apiClientId">API client id. If not specified, will update current API client.</param>
        /// <returns>Updated ApiKey object.</returns>
        public Task<ApiKey> UpdateDescription(string description, string apiClientId = null)
        {
            if(description == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync<ApiKey>("apikey.update_description", new {description, api_client_id = apiClientId},
                "apikey");
        }

        /// <summary>
        /// Adds permission to specified User API client. Requires Backend API key with Admin permission role.
        /// </summary>
        /// <param name="apiClientId">User API client id.</param>
        /// <param name="permission">User API client's permission to add.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Authorize(string apiClientId, ApiKeyPermission permission)
        {
            if(apiClientId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("apikey.authorize",
                new {api_client_id = apiClientId, permission = ApiKeyPermissionByStringConverter.GetString(permission)});
        }

        /// <summary>
        /// Removes permission from specified User API client. Requires Backend API key with Admin permission role.
        /// </summary>
        /// <param name="apiClientId">User API client id.</param>
        /// <param name="permission">User API client's permission to remove.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Deauthorize(string apiClientId, ApiKeyPermission permission)
        {
            if (apiClientId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("apikey.deauthorize",
                new { api_client_id = apiClientId, permission = ApiKeyPermissionByStringConverter.GetString(permission) });
        }

        /// <summary>
        /// Deletes specified API client. Only Admin permission role can delete API clients.
        /// </summary>
        /// <param name="apiClientId">API client id defining API client to delete.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Delete(string apiClientId)
        {
            if(apiClientId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("apikey.delete", new {api_client_id = apiClientId});
        }
    }
}
