using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net.Access;

namespace Syncano.Net.Api
{
    public class AdministratorSyncanoClient
    {
        private readonly ISyncanoClient _syncanoClient;

        public AdministratorSyncanoClient(ISyncanoClient syncanoClient)
        {
            _syncanoClient = syncanoClient;
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
        /// Adds a new administrator to current instance (sends an invitation). Only Admin permission role can add new administrators.
        /// </summary>
        /// <param name="adminEmail">Email of administrator to add.</param>
        /// <param name="roleId">Initial role for current instance (see role.get()).</param>
        /// <param name="message">Message that will be sent along with invitation to instance.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> New(string adminEmail, string roleId, string message)
        {
            if(adminEmail == null || roleId == null || message == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("admin.new", new
            {
                admin_email = adminEmail,
                role_id = roleId,
                message
            });
        }

        /// <summary>
        /// Get the all administrators of the current instance.
        /// </summary>
        /// <returns>List of Admin objects.</returns>
        public async Task<List<Administrator>> Get()
        {
            return await _syncanoClient.GetAsync<List<Administrator>>("admin.get", "admin");
        }

        /// <summary>
        /// Gets admin info with specific id or email from the current instance.
        /// <remarks>Admin_id/admin_email parameter means that one can use either one of them - admin_id or admin_email.</remarks>
        /// </summary>
        /// <param name="adminId">Admin id.</param>
        /// <param name="adminEmail">Admin email.</param>
        /// <returns>Administrator object.</returns>
        public Task<Administrator> GetOne(string adminId = null, string adminEmail = null)
        {
            if(adminId == null && adminEmail == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync<Administrator>("admin.get_one", new {admin_id = adminId, admin_email = adminEmail}, "admin");
        }

        /// <summary>
        /// Updates specified admin's permission role. Only administrators whose role is defined as "Admin" or "Owner" can edit their instance's administrators.
        /// </summary>
        /// <param name="roleId">New admin's instance role id to set (see role.get()).</param>
        /// <param name="adminId">The admin id to update.</param>
        /// <param name="adminEmail">The admin email to update.</param>
        /// <returns>Updated Administrator object.</returns>
        public Task<Administrator> Update(string roleId, string adminId = null, string adminEmail = null)
        {
            if(adminId == null && adminEmail == null)
                throw new ArgumentNullException();

            if(roleId == null)
                throw new ArgumentNullException();

            return _syncanoClient.PostAsync<Administrator>("admin.update",
                new {admin_id = adminId, admin_email = adminEmail, role_id = roleId}, "admin");
        }

        /// <summary>
        /// Deletes specified administrator from current instance. Only administrators whose role is defined as "Admin" or "Owner" can edit their instance's administrators.
        /// <remarks>Admin_id/admin_email parameter means that one can use either one of them - admin_id or admin_email.</remarks>
        /// </summary>
        /// <param name="adminId">Admin id defining admin to delete.</param>
        /// <param name="adminEmail">Admin email defining admin to delete.</param>
        /// <returns></returns>
        public Task<bool> Delete(string adminId = null, string adminEmail = null)
        {
            if(adminId == null && adminEmail == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("admin.delete", new {admin_id = adminId, admin_email = adminEmail});
        }
    }
}
