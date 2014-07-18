using System;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Xunit;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class UserSyncanoClientTests
    {
        private readonly Syncano _syncanoClient;

        public UserSyncanoClientTests()
        {
            _syncanoClient = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";

            //when
            var user = await client.New(name, password);

            //then
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(name);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNick_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string nick = "newUserNick";
            const string password = "abcde123";

            //when
            var user = await client.New(name, password, nick);

            //then
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(name);
            user.Nick.ShouldEqual(nick);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithPassword_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";

            //when
            var user = await client.New(name, password: password);

            //then
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(name);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Login_CreatesNewAuthKey(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var user = await client.New(name, password);

            //when
            var result = await client.Login(user.Name, password);

            //then
            result.ShouldNotBeNull();

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Login_WithNick_CreatesNewAuthKey(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string nick = "newUserNick";
            const string password = "abcde123";
            var user = await client.New(name, password, nick);

            //when
            var result = await client.Login(user.Name, password);

            //then
            result.ShouldNotBeNull();

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Login_WithPassword_CreatesNewAuthKey(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var user = await client.New(name, password);

            //when
            var result = await client.Login(user.Name, password);

            //then
            result.ShouldNotBeNull();

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByUserId_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var user = await client.New(name, password);

            //when
            var result = await client.GetOne(user.Id);

            //then
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(user.Id);
            result.Name.ShouldEqual(name);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByUserKey_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var user = await client.New(name, password);

            //when
            var result = await client.GetOne(userName: user.Name);

            //then
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(user.Id);
            result.Name.ShouldEqual(name);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_CreatesNewUserObject(UserSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne();
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
            
        }

        [Fact(Skip = "Avatar to big for tcp")]
        //[Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithAvatar_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var avatar = TestData.ImageToBase64("sampleImage.jpg");

            //when
            var user = await client.New(name, password, avatar: avatar);

            //then
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(name);
            user.Avatar.ShouldNotBeNull();

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_GetsNumberOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.Folder = TestData.FolderName;
            var dataObject = await _syncanoClient.DataObjects.New(dataRequest);
            
            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;
            await _syncanoClient.DataObjects.Delete(deleteRequest);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_NewName_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            const string newName = "evenNewerUserName";
            var user = await client.New(name, password);

            //when
            var newUser = await client.Update(user.Id, newName);

            //then
            newUser.ShouldNotBeNull();
            newUser.Id.ShouldEqual(user.Id);
            newUser.Name.ShouldEqual(newName);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof (SyncanoClientsProvider))]
        public async Task Count_GetsNumberOfUsers(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();

            //when
            var result = await client.Count(request);

            //then
            result.ShouldEqual(2);
        }

        

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserId_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var user = await client.New(name, password);

            //when
            var result = await client.Delete(user.Id);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserId_WithNick_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string nick = "newUserNick";
            const string password = "abcde123";
            var user = await client.New(name, password, nick);

            //when
            var result = await client.Delete(user.Id);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserId_WithPassword_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var user = await client.New(name, password);

            //when
            var result = await client.Delete(user.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact(Skip = "Avatar to big for tcp")]
        //[Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserId_WithAvatar_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var avatar = TestData.ImageToBase64("sampleImage.jpg");
            var user = await client.New(name, password, avatar: avatar);

            //cleanup
            var result = await client.Delete(user.Id);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserName_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var user = await client.New(name, password);

            //when
            var result = await client.Delete(userName: user.Name);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserName_WithNick_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string nick = "newUserNick";
            const string password = "abcde123";
            var user = await client.New(name, password, nick);

            //when
            var result = await client.Delete(userName: user.Name);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserName_WithPassword_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var user = await client.New(name, password);

            //when
            var result = await client.Delete(userName: user.Name);

            //then
            result.ShouldBeTrue();
        }

        [Fact(Skip = "Avatar to big for tcp")]
        //[Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserName_WithAvatar_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var avatar = TestData.ImageToBase64("sampleImage.jpg");
            var user = await client.New(name, password, avatar: avatar);

            //cleanup
            var result = await client.Delete(userName: user.Name);

            //then
            result.ShouldBeTrue();
        }

    }
}
