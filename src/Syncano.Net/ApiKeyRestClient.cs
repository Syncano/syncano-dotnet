using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncano.Net
{
    public class ApiKeyRestClient
    {
        private readonly SyncanoRestClient _restClient;

        public ApiKeyRestClient(SyncanoRestClient restClient)
        {
            _restClient = restClient;
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
            return await _restClient.GetAsync("role.get", "role", t => t.ToObject<List<Role>>());
        }

        public Task<string> StartSession(TimeZoneInfo timeZone = null)
        {
            return _restClient.PostAsync("apikey.start_session", new {timezone = WindowsTzToIanaTz(timeZone)}, "session_id",
                t => t.ToObject<string>());
        }

        public Task<ApiKey> New(string description, ApiKeyType type = ApiKeyType.Backend, string roleId = null)
        {
            if(description == null)
                throw new ArgumentNullException();

            if(type == ApiKeyType.User && roleId != null)
                throw new ArgumentException();

            return _restClient.PostAsync("apikey.new", new {description, type, role_id = roleId}, "apikey",
                t => t.ToObject<ApiKey>());
        }

        public async Task<List<ApiKey>> Get()
        {
            return await _restClient.GetAsync("apikey.get", "apikey", t => t.ToObject<List<ApiKey>>());
        }

        public Task<ApiKey> GetOne(string apiClientId = null)
        {
            return _restClient.GetAsync("apikey.get_one", new {api_client_id = apiClientId}, "apikey",
                t => t.ToObject<ApiKey>());
        }

        public Task<ApiKey> UpdateDescription(string description, string apiClientId = null)
        {
            if(description == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("apikey.update_description", new {description, api_client_id = apiClientId},
                "apikey", t => t.ToObject<ApiKey>());
        }

        public Task<bool> Authorize(string apiClientId, ApiKeyPermission permission)
        {
            if(apiClientId == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("apikey.authorize",
                new {api_client_id = apiClientId, permission = ApiKeyPermissionByStringConverter.GetString(permission)});
        }

        public Task<bool> Deauthorize(string apiClientId, ApiKeyPermission permission)
        {
            if (apiClientId == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("apikey.deauthorize",
                new { api_client_id = apiClientId, permission = ApiKeyPermissionByStringConverter.GetString(permission) });
        }

        public Task<bool> Delete(string apiClientId)
        {
            if(apiClientId == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("apikey.delete", new {api_client_id = apiClientId});
        }
    }
}
