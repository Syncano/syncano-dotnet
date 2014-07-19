using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net.Data;

namespace Syncano.Net.Api
{
    public class UserSyncanoClient
    {
        private readonly ISyncanoClient _syncanoClient;

        public UserSyncanoClient(ISyncanoClient syncanoClient)
        {
            _syncanoClient = syncanoClient;
        }

        public Task<string> Login(string userName, string password = null)
        {
            if (userName == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync<string>("user.login", new {user_name = userName, password}, "auth_key");
        }

        public Task<User> New(string userName, string password = null, string nick = null, string avatar = null)
        {
            if(userName == null)
                throw new ArgumentNullException();

            return _syncanoClient.PostAsync<User>("user.new", new
            {
                user_name = userName,
                nick,
                avatar,
                password
            }, "user");
        }

        public Task<List<User>> GetAll(string sinceId = null, long limit = 100)
        {
            return _syncanoClient.GetAsync<List<User>>("user_get_all", new {since_id = sinceId, limit}, "user");
        }

        public Task<List<User>> Get(UserQueryRequest request)
        {
            if(request.ProjectId == null)
                throw new ArgumentNullException();

            if (request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            var folders = request.Folders == null ? new List<string>() : new List<string>(request.Folders);
            if (folders.Count + (request.Folder != null ? 1 : 0) >
                DataObjectSyncanoClient.MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.Folder != null)
                folders.Add(request.Folder);

            return _syncanoClient.PostAsync<List<User>>("user.get",
                new
                {
                    project_id = request.ProjectId,
                    collection_id = request.CollectionId,
                    collection_key = request.CollectionKey,
                    state = request.State.ToString(),
                    folders = folders.ToArray(),
                    filter = request.Filter
                }, "user");
        }

        public Task<User> GetOne(string userId = null, string userName = null)
        {
            if(userId == null && userName == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync<User>("user.get_one", new {user_id = userId, user_name = userName}, "user");
        }

        public Task<User> Update(string userId = null, string userName = null, string nick = null,
            string avatar = null, string newPassword = null, string currentPassword = null)
        {
            return _syncanoClient.PostAsync<User>("user.update",
                new
                {
                    user_id = userId,
                    user_name = userName,
                    nick,
                    avatar,
                    new_password = newPassword,
                    current_password = currentPassword
                }, "user");
        }

        public Task<int> Count(UserQueryRequest request)
        {
            if (request.CollectionId != null && request.CollectionKey != null)
                throw new ArgumentException();

            var folders = request.Folders == null ? new List<string>() : new List<string>(request.Folders);
            if (folders.Count + (request.Folder != null ? 1 : 0) >
                DataObjectSyncanoClient.MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.Folder != null)
                folders.Add(request.Folder);

            return _syncanoClient.PostAsync<int>("user.count",
                new
                {
                    project_id = request.ProjectId,
                    collection_id = request.CollectionId,
                    collection_key = request.CollectionKey,
                    state = request.State.ToString(),
                    folders = folders.ToArray(),
                    filter = request.Filter
                }, "count");
        }

        public Task<bool> Delete(string userId = null, string userName = null)
        {
            if(userId == null && userName == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("user.delete", new {user_id = userId, user_name = userName});
        }
    }
}
