using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net.Access;
using Syncano.Net.Http;

namespace Syncano.Net.Api
{
    public class AdministratorSyncanoClient
    {
        private readonly SyncanoHttpClient _httpClient;

        public AdministratorSyncanoClient(SyncanoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _httpClient.GetAsync("role.get", "role", t => t.ToObject<List<Role>>());
        }

        public Task<bool> New(string adminEmail, string roleId, string message)
        {
            if(adminEmail == null || roleId == null || message == null)
                throw new ArgumentNullException();

            return _httpClient.PostAsync("admin.new", new
            {
                admin_email = adminEmail,
                role_id = roleId,
                message
            });
        }

        public async Task<List<Administrator>> Get()
        {
            return await _httpClient.GetAsync("admin.get", "admin", t => t.ToObject<List<Administrator>>());
        }

        public Task<Administrator> GetOne(string adminId = null, string adminEmail = null)
        {
            if(adminId == null && adminEmail == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync("admin.get_one", new {admin_id = adminId, admin_email = adminEmail}, "admin",
                t => t.ToObject<Administrator>());
        }

        public Task<Administrator> Update(string roleId, string adminId = null, string adminEmail = null)
        {
            if(adminId == null && adminEmail == null)
                throw new ArgumentNullException();

            if(roleId == null)
                throw new ArgumentNullException();

            return _httpClient.PostAsync("admin.update",
                new {admin_id = adminId, admin_email = adminEmail, role_id = roleId}, "admin",
                t => t.ToObject<Administrator>());
        }

        public Task<bool> Delete(string adminId = null, string adminEmail = null)
        {
            if(adminId == null && adminEmail == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync("admin.delete", new {admin_id = adminId, admin_email = adminEmail});
        }
    }
}
