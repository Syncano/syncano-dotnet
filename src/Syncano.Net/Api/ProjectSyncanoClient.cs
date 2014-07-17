using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net.Data;

namespace Syncano.Net.Api
{
    public class ProjectSyncanoClient
    {
        private readonly ISyncanoClient _syncanoClient;

        public ProjectSyncanoClient(ISyncanoClient syncanoClient)
        {
            _syncanoClient = syncanoClient;
        }

        public Task<Project> New(string name, string description = null)
        {
            if(name == null)
                throw new ArgumentNullException();

            return _syncanoClient.PostAsync("project.new", new { name, description }, "project",
                t => t.ToObject<Project>());
        }
        
        public Task<List<Project>> Get()
        {
            return _syncanoClient.GetAsync("project.get", "project", t => t.ToObject<List<Project>>());
        }

        public Task<Project> GetOne(string projectId)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("project.get_one", new { project_id = projectId }, "project", t => t.ToObject<Project>());
        }

        public Task<Project> Update(string projectId, string name = null, string description = null)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.PostAsync("project.update", new { project_id = projectId, name, description },
                "project", t => t.ToObject<Project>());
        }

        public Task<bool> Authorize(string apiClientId, Permissions permission, string projectId)
        {
            if(apiClientId == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);
            return _syncanoClient.GetAsync("project.authorize",
                new {api_client_id = apiClientId, permission = permissionString, project_id = projectId});
        }

        public Task<bool> Deauthorize(string apiClientId, Permissions permission, string projectId)
        {
            if(apiClientId == null || projectId == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);
            return _syncanoClient.GetAsync("project.deauthorize",
                new { api_client_id = apiClientId, permission = permissionString, project_id = projectId });
        }

        public Task<bool> Delete(string projectId)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("project.delete", new { project_id = projectId });
        }
    }
}