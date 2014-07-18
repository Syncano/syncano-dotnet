using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Task<User> New(string userName, string nick = null, string avatar = null, string password = null)
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

        public Task<bool> Delete(string userId = null, string userName = null)
        {
            if(userId == null && userName == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("user.delete", new {user_id = userId, user_name = userName});
        }
    }
}
