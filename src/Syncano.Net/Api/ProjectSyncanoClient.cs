using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net.Data;

namespace Syncano.Net.Api
{
    /// <summary>
    /// Class with Project management api.
    /// </summary>
    public class ProjectSyncanoClient
    {
        private readonly ISyncanoClient _syncanoClient;

        /// <summary>
        /// Creates ProjectSyncanoClient object.
        /// </summary>
        /// <param name="syncanoClient">Object implementing ISyncanoClient interface. Provides means for connecting to Syncano.</param>
        public ProjectSyncanoClient(ISyncanoClient syncanoClient)
        {
            _syncanoClient = syncanoClient;
        }

        /// <summary>
        /// Create a new project.
        /// </summary>
        /// <param name="name">New project's name.</param>
        /// <param name="description">New project's description.</param>
        /// <returns>New Project object.</returns>
        public Task<Project> New(string name, string description = null)
        {
            if(name == null)
                throw new ArgumentNullException();

            return _syncanoClient.PostAsync<Project>("project.new", new { name, description }, "project");
        }
        
        /// <summary>
        /// Get projects
        /// </summary>
        /// <remarks>User API key usage permitted. Returns only projects that have``read_data`` permission added through project.authorize().</remarks>
        /// <returns>List of Project objects.</returns>
        public Task<List<Project>> Get()
        {
            return _syncanoClient.GetAsync<List<Project>>("project.get", "project");
        }

        /// <summary>
        /// Get one project.
        /// </summary>
        /// <remarks>User API key usage permitted if read_data permission is added to specified folder through project.authorize().</remarks>
        /// <param name="projectId">Project id defining project.</param>
        /// <returns>Project object.</returns>
        public Task<Project> GetOne(string projectId)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync<Project>("project.get_one", new { project_id = projectId }, "project");
        }

        /// <summary>
        /// Update existing project.
        /// </summary>
        /// <param name="projectId">Existing project's id.</param>
        /// <param name="name">New name of specified project.</param>
        /// <param name="description">New description of specified project.</param>
        /// <returns>Updated Project object.</returns>
        public Task<Project> Update(string projectId, string name = null, string description = null)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.PostAsync<Project>("project.update", new { project_id = projectId, name, description },
                "project");
        }

        /// <summary>
        /// Adds container-level permission to specified User API client. Requires Backend API key with Admin permission role.
        /// </summary>
        /// <param name="apiClientId">User API client id.</param>
        /// <param name="permission">User API client's permission to add.</param>
        /// <param name="projectId">Project id defining project that permission will be added to.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Authorize(string apiClientId, Permissions permission, string projectId)
        {
            if(apiClientId == null || projectId == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);
            return _syncanoClient.GetAsync("project.authorize",
                new {api_client_id = apiClientId, permission = permissionString, project_id = projectId});
        }

        /// <summary>
        /// Removes container-level permission from specified User API client. Requires Backend API key with Admin permission role.
        /// </summary>
        /// <param name="apiClientId">User API client id.</param>
        /// <param name="permission">User API client's permission to remove.</param>
        /// <param name="projectId">Project id defining project that permission will be removed from.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Deauthorize(string apiClientId, Permissions permission, string projectId)
        {
            if(apiClientId == null || projectId == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);
            return _syncanoClient.GetAsync("project.deauthorize",
                new { api_client_id = apiClientId, permission = permissionString, project_id = projectId });
        }

        /// <summary>
        /// Delete (permanently) project with specified project_id.
        /// </summary>
        /// <param name="projectId">Project id defining project to be deleted.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Delete(string projectId)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("project.delete", new { project_id = projectId });
        }
    }
}