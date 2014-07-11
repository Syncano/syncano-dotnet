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
        
        public Task<List<Project>> Get()
        {
            return _restClient.GetAsync("project.get", "project", t => t.ToObject<List<Project>>());

        }
    }
}