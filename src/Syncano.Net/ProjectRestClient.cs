using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncano.Net
{
    public class ProjectRestClient
    {
        private readonly SyncanoRestClient _restClient;

        public ProjectRestClient(SyncanoRestClient restClient)
        {
            _restClient = restClient;
        }

        public Task<Project> New(string name, string description = null)
        {
            if(name == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("project.new", new { name = name, description = description }, "project",
                t => t.ToObject<Project>());
        }
        
        public Task<List<Project>> Get()
        {
            return _restClient.GetAsync("project.get", "project", t => t.ToObject<List<Project>>());
        }

        public Task<Project> GetOne(string projectId)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("project.get_one", new { project_id = projectId }, "project", t => t.ToObject<Project>());
        }

        public Task<Project> Update(string projectId, string name = null, string description = null)
        {
            if(projectId == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("project.update", new { project_id = projectId, name = name, description = description },
                "project", t => t.ToObject<Project>());
        }

        public Task<bool> Authorize(string apiClientId, Permissions permission, string projectId)
        {
            string permissionString = PermissionsParser.GetString(permission);
            return _restClient.GetAsync("project.authorize",
                new {api_client_id = apiClientId, permission = permissionString, project_id = projectId});
        }

        public Task<bool> Deauthorize(string apiClientId, Permissions permission, string projectId)
        {
            string permissionString = PermissionsParser.GetString(permission);
            return _restClient.GetAsync("project.deauthorize",
                new { api_client_id = apiClientId, permission = permissionString, project_id = projectId });
        }

        public Task<bool> Delete(string projectId)
        {
            return _restClient.GetAsync("project.delete", new { project_id = projectId });
        }
    }
}