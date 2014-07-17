using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net.Access;
using Syncano.Net.Http;

namespace Syncano.Net.Api
{
    public class ApiKeySyncanoClient
    {
        private readonly SyncanoHttpClient _httpClient;

        public ApiKeySyncanoClient(SyncanoHttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<List<Role>> GetRoles()
        {
            return await _httpClient.GetAsync<List<Role>>("role.get", "role");
        }

        public Task<string> StartSession(TimeZoneInfo timeZone = null)
        {
            return _httpClient.PostAsync<string>("apikey.start_session", new {timezone = WindowsTzToIanaTz(timeZone)}, "session_id");
        }

        public Task<ApiKey> New(string description, ApiKeyType type = ApiKeyType.Backend, string roleId = null)
        {
            if(description == null)
                throw new ArgumentNullException();

            if(type == ApiKeyType.User && roleId != null)
                throw new ArgumentException();

            return _httpClient.PostAsync<ApiKey>("apikey.new", new {description, type, role_id = roleId}, "apikey");
        }

        public async Task<List<ApiKey>> Get()
        {
            return await _httpClient.GetAsync<List<ApiKey>>("apikey.get", "apikey");
        }

        public Task<ApiKey> GetOne(string apiClientId = null)
        {
            return _httpClient.GetAsync<ApiKey>("apikey.get_one", new {api_client_id = apiClientId}, "apikey");
        }

        public Task<ApiKey> UpdateDescription(string description, string apiClientId = null)
        {
            if(description == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync<ApiKey>("apikey.update_description", new {description, api_client_id = apiClientId},
                "apikey");
        }

        public Task<bool> Authorize(string apiClientId, ApiKeyPermission permission)
        {
            if(apiClientId == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync("apikey.authorize",
                new {api_client_id = apiClientId, permission = ApiKeyPermissionByStringConverter.GetString(permission)});
        }

        public Task<bool> Deauthorize(string apiClientId, ApiKeyPermission permission)
        {
            if (apiClientId == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync("apikey.deauthorize",
                new { api_client_id = apiClientId, permission = ApiKeyPermissionByStringConverter.GetString(permission) });
        }

        public Task<bool> Delete(string apiClientId)
        {
            if(apiClientId == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync("apikey.delete", new {api_client_id = apiClientId});
        }
    }
}
