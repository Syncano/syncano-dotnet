using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net.Data;

namespace Syncano.Net.Api
{
    public class UserSyncanoClient
    {
        /// <summary>
        /// Max number of users per request.
        /// </summary>
        public const long MaxLimit = 100;

        private readonly ISyncanoClient _syncanoClient;

        public UserSyncanoClient(ISyncanoClient syncanoClient)
        {
            _syncanoClient = syncanoClient;
        }

        /// <summary>
        /// Logs in a user.
        /// <remarks>This method is intended for User API key usage.</remarks>
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">User's password.</param>
        /// <returns>Returns it's auth_key. Use auth_key in following requests.</returns>
        public Task<string> Login(string userName, string password)
        {
            if (userName == null || password == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync<string>("user.login", new {user_name = userName, password}, "auth_key");
        }

        /// <summary>
        /// Creates a new user.
        /// <remarks>User API key usage permitted if add_user permission is added through apikey.authorize().</remarks>
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">User's password.</param>
        /// <param name="nick">User's nickname.</param>
        /// <param name="avatar">User's avatar in Base64 format.</param>
        /// <returns>New User object.</returns>
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

        /// <summary>
        /// Gets all users from within instance.
        /// <remarks>To paginate and to get more data, use since_id.</remarks>
        /// </summary>
        /// <param name="sinceId">If specified, will only return users with id higher than since_id (newer).</param>
        /// <param name="limit">Number of users to be returned. Default and max value: 100</param>
        /// <returns>List of User objects.</returns>
        public Task<List<User>> GetAll(string sinceId = null, long limit = MaxLimit)
        {
            if(limit > MaxLimit)
                throw new ArgumentException();

            return _syncanoClient.GetAsync<List<User>>("user.get_all", new {since_id = sinceId, limit}, "user");
        }

        /// <summary>
        /// Gets users of specified criteria that are associated with Data Objects within specified collection.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// </summary>
        /// <param name="request">User query request object.</param>
        /// <returns>List of User objects.</returns>
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
                    filter = request.Filter == null ? null : request.Filter.ToString()
                }, "user");
        }

        /// <summary>
        /// Get one user.
        /// <remarks>User API key usage permitted. In this case, it can only be used to get currently associated user's data. All parameters are optional.</remarks>
        /// <remarks>The user_id/user_name parameter means that one can use either one of them - user_id or user_name.</remarks>
        /// </summary>
        /// <param name="userId">User id. User_id is automatically filled when used with User API key.</param>
        /// <param name="userName">User name.</param>
        /// <returns></returns>
        public Task<User> GetOne(string userId = null, string userName = null)
        {
            return _syncanoClient.GetAsync<User>("user.get_one", new {user_id = userId, user_name = userName}, "user");
        }

        /// <summary>
        /// Updates a specified user.
        /// <remarks>User API key usage permitted. In this case, it can only be used to update currently associated user's data. User_id is automatically filled with current user's id.</remarks>
        /// <remarks>The user_id/user_name parameter means that one can use either one of them - user_id or user_name.</remarks>
        /// </summary>
        /// <param name="userId">User id defining user. If both id and name are specified, will use id for getting user while user_name will be updated with provided new value. User_id is automatically filled when used with User API key.</param>
        /// <param name="userName">User name defining user. If both id and name are specified, will use id for getting user while user_name will be updated with provided new value.</param>
        /// <param name="nick">New user nickname.</param>
        /// <param name="avatar">User avatar. If specified as empty string - will instead delete current avatar.</param>
        /// <param name="newPassword">New user password.</param>
        /// <param name="currentPassword">Current password for confirmation. Required only when used with User API key and new_password is specified.</param>
        /// <returns>Updated User object.</returns>
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

        /// <summary>
        /// Count users of specified criteria.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// </summary>
        /// <param name="request">User query request object.</param>
        /// <returns>Count of users with specified criteria.</returns>
        public Task<int> Count(UserQueryRequest request)
        {
            if (request.ProjectId == null)
                throw new ArgumentNullException();

            if (request.CollectionId != null && request.CollectionKey != null)
                throw new ArgumentException();

            var folders = request.Folders == null ? new List<string>() : new List<string>(request.Folders);
            if (folders.Count + (request.Folder != null ? 1 : 0) >
                MaxLimit)
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
                    filter = request.Filter == null ? null : request.Filter.ToString()
                }, "count");
        }

        /// <summary>
        /// Deletes (permanently) specified user and all associated data objects.
        /// <remarks>The user_id/user_name parameter means that one can use either one of them - user_id or user_name.</remarks>
        /// <remarks>User API key usage permitted. In this case, it can only be used to update currently associated user. User_id is automatically filled with current user's id.</remarks>
        /// </summary>
        /// <param name="userId">User id defining user to delete. User_id is automatically filled when used with User API key.</param>
        /// <param name="userName">User name defining user to delete.</param>
        /// <returns></returns>
        public Task<bool> Delete(string userId = null, string userName = null)
        {
            if(userId == null && userName == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("user.delete", new {user_id = userId, user_name = userName});
        }
    }
}
