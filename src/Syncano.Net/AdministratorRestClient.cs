using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncano.Net
{
    public class AdministratorRestClient
    {
        private readonly SyncanoRestClient _restClient;

        public AdministratorRestClient(SyncanoRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _restClient.GetAsync("role.get", "role", t => t.ToObject<List<Role>>());
        }

        public Task<bool> New(string adminEmail, string roleId, string message)
        {
            if(adminEmail == null || roleId == null || message == null)
                throw new ArgumentNullException();

            return _restClient.PostAsync("admin.new", new
            {
                admin_email = adminEmail,
                role_id = roleId,
                message
            });
        }

        public async Task<List<Administrator>> Get()
        {
            return await _restClient.GetAsync("admin.get", "admin", t => t.ToObject<List<Administrator>>());
        }

        public Task<Administrator> GetOne(string adminId = null, string adminEmail = null)
        {
            if(adminId == null && adminEmail == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("admin.get_one", new {admin_id = adminId, admin_email = adminEmail}, "admin",
                t => t.ToObject<Administrator>());
        }

        public Task<Administrator> Update(string roleId, string adminId = null, string adminEmail = null)
        {
            if(adminId == null && adminEmail == null)
                throw new ArgumentNullException();

            if(roleId == null)
                throw new ArgumentNullException();

            return _restClient.PostAsync("admin.update",
                new {admin_id = adminId, admin_email = adminEmail, role_id = roleId}, "admin",
                t => t.ToObject<Administrator>());
        }

        public Task<bool> Delete(string adminId = null, string adminEmail = null)
        {
            if(adminId == null && adminEmail == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("admin.delete", new {admin_id = adminId, admin_email = adminEmail});
        }
    }
}
