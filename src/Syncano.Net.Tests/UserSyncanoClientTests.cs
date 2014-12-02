using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Syncano.Net.Data;
using Syncano.Net.DataRequests;
using Syncano.Net.Http;
using SyncanoSyncServer.Net;
using Xunit;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class UserSyncanoClientTests
    {
        private readonly DataObjectSyncanoClient _dataClient;

        public UserSyncanoClientTests()
        {
            var syncanoClient = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
            _dataClient = syncanoClient.DataObjects;
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Login_CreatesNewAuthKey(UserSyncanoClient client)
        {
            //when
            var result = await client.Login(TestData.UserName, TestData.UserPassword);

            //then
            result.ShouldNotBeNull();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Login_WithNullPassword_CreatesNewAuthKey(UserSyncanoClient client)
        {
            try
            {
                await client.Login("userName", null);
                throw new Exception("Login should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Login_WithInvalidPasswordOverHttp_ThrowsException()
        {
            //given
            var httpClient = new UserSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.UserApiKey));

            try
            {
                //when
                await httpClient.Login(TestData.UserName, "abcde");
                throw new Exception("Login should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Login_WithInvalidPasswordOverTcp_ThrowsException()
        {
            //given
            var syncClient = new SyncServer(TestData.InstanceName, TestData.UserApiKey);
            await syncClient.Start();

            try
            {
                //when
                await syncClient.Users.Login(TestData.UserName, "abcde");
                throw new Exception("Login should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Login_WithInvalidName_CreatesNewAuthKey(UserSyncanoClient client)
        {
            try
            {
                await client.Login("abcde", TestData.UserPassword);
                throw new Exception("Login should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Login_WithNullName_CreatesNewAuthKey(UserSyncanoClient client)
        {
            try
            {
                await client.Login(null, TestData.UserPassword);
                throw new Exception("Login should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";

            //when
            var user = await client.New(name);

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

            //when
            var user = await client.New(name, nick: nick);

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
            var user = await client.New(name, password);

            //then
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(name);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithAvatar_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            string avatar = TestData.ImageToBase64("smallSampleImage.png");

            //when
            var user = await client.New(name, avatar: avatar);

            //then
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(name);
            user.Avatar.ShouldNotBeNull();

            //cleanup
            await client.Delete(user.Id);
        }

        [Fact]
        public async Task New_WithAvatarTooBigForTcp_ThrowsException()
        {
            //given
            var syncClient = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
            await syncClient.Start();
            const string name = "newUserName";
            string avatar = TestData.ImageToBase64("sampleImage.jpg");

            try
            {
                //when
                var user = await syncClient.Users.New(name, avatar: avatar);
                throw new Exception("New should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
            
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof (SyncanoClientsProvider))]
        public async Task New_WithNullName_ThrowsException(UserSyncanoClient client)
        {
            try
            {
                //when
                await client.New(null);
                throw new Exception("New should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof (SyncanoClientsProvider))]
        public async Task GetAll_GetsListOfUsers(UserSyncanoClient client)
        {
            //when
            var result = await client.GetAll();

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldEqual(2);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetAll_WithLimit_GetsListOfUsers(UserSyncanoClient client)
        {
            //when
            var result = await client.GetAll(limit: 1);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldEqual(1);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetAll_WithSinceId_GetsListOfUsers(UserSyncanoClient client)
        {
            //when
            var result = await client.GetAll(TestData.OldUserId);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldEqual(1);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetAll_WithTooBigLimit_ThrowsException(UserSyncanoClient client)
        {
            try
            {
                //when
                await client.GetAll(limit: UserSyncanoClient.MaxLimit + 1);
                throw new Exception("GetAll should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof (SyncanoClientsProvider))]
        public async Task Get_ByCollectionId_GetsListOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            await _dataClient.New(dataRequest);

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

            await _dataClient.Delete(deleteRequest);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_ByCollectionKey_GetsListOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionKey = TestData.CollectionKey;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithModeratedState_GetsListOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.State = DataObjectState.Moderated;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Moderated;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithPendingState_GetsListOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.State = DataObjectState.Pending;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Pending;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithSingleFolderName_GetsListOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.Folder = TestData.FolderName;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Folder = TestData.FolderName;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithFolderListName_GetsListOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.Folder = TestData.FolderName;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Folders = new List<string> {TestData.FolderName};

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithImageContentFilter_GetsListOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.ImageBase64 = TestData.ImageToBase64("sampleImage.jpg");
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Filter = DataObjectContentFilter.Image;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithTextContentFilter_GetsListOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.Text = "sample text content";
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Filter = DataObjectContentFilter.Text;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithNullProjectId_ThrowsException(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithInvalidProjectId_ThrowsException(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();
            request.ProjectId = "abcde";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithInvalidCollectionId_ThrowsException(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = "abcde";

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithInvalidCollectionKey_ThrowsException(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = "abcde";

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithNullCollectionIdAndCollectionKey_ThrowsException(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact( /* Skip = "Not authorized.")*/ )]
        public async Task GetOne_GetsUserObject_OverHttp()
        {
            //given
            var httpClient =
                new UserSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.UserApiKey));
            await httpClient.Login(TestData.UserName, TestData.UserPassword);

            //when
            var result = await httpClient.GetOne();

            //then
            result.ShouldNotBeNull();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByUserId_GetsUserObject(UserSyncanoClient client)
        {
            //when
            var result = await client.GetOne(TestData.UserId);

            //then
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(TestData.UserId);
            result.Name.ShouldEqual(TestData.UserName);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByUserName_GetsUserObject(UserSyncanoClient client)
        {
            //when
            var result = await client.GetOne(userName: TestData.UserName);

            //then
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(TestData.UserId);
            result.Name.ShouldEqual(TestData.UserName);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_NewName_UpdatesUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            const string newName = "evenNewerUserName";
            var user = await client.New(name, password);

            //when
            var updatedUser = await client.Update(user.Id, newName, currentPassword: password);

            //then
            updatedUser.ShouldNotBeNull();
            updatedUser.Id.ShouldEqual(user.Id);
            updatedUser.Name.ShouldEqual(newName);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_NewNick_UpdatesUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            const string newNick = "newNick";
            var user = await client.New(name, password);

            //when
            var updatedUser = await client.Update(user.Id, nick: newNick, currentPassword: password);

            //then
            updatedUser.ShouldNotBeNull();
            updatedUser.Id.ShouldEqual(user.Id);
            updatedUser.Nick.ShouldEqual(newNick);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_NewPassword_UpdatesUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            const string newPassword = "qwerty123";
            var user = await client.New(name, password);

            //when
            var updatedUser = await client.Update(user.Id, newPassword: newPassword, currentPassword: password);

            //then
            updatedUser.ShouldNotBeNull();
            updatedUser.Id.ShouldEqual(user.Id);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_NewAvatar_UpdatesUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var user = await client.New(name, password);

            //when
            var updatedUser = await client.Update(user.Id, avatar: TestData.ImageToBase64("smallSampleImage.png"), currentPassword: password);

            //then
            updatedUser.ShouldNotBeNull();
            updatedUser.Id.ShouldEqual(user.Id);
            updatedUser.Avatar.ShouldNotBeNull();

            //cleanup
            await client.Delete(user.Id);
        }

        [Fact]
        public async Task Update_NewAvatarTooBigForTcp_ThrowsException()
        {
            //given
            var syncClient = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
            await syncClient.Start();
            const string password = "abcde123";

            try
            {
                //when
                await syncClient.Users.Update("userId", avatar: TestData.ImageToBase64("smallSampleImage.png"), currentPassword: password);
                throw new Exception("Update should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }

            
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_DeleteAvatar_UpdatesUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var user = await client.New(name, password, avatar: TestData.ImageToBase64("smallSampleImage.png"));

            //when
            var updatedUser = await client.Update(user.Id, avatar: "", currentPassword: password);

            //then
            updatedUser.ShouldNotBeNull();
            updatedUser.Id.ShouldEqual(user.Id);
            updatedUser.Avatar.ShouldBeNull();

            //cleanup
            await client.Delete(user.Id);
        }

        [Fact( /* Skip = "Not authorized.")*/ )]
        public async Task Update_NoUserId_UpdatesUserObject_OverHttp()
        {
            //given
            var client = new UserSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.UserApiKey));
            const string name = "newUserName";
            const string password = "abcde123";
            const string newName = "evenNewerUserName";
            var user = await client.New(name, password);

            //when
            var newUser = await client.Update(currentPassword: password);

            //then
            newUser.ShouldNotBeNull();
            newUser.Id.ShouldEqual(user.Id);
            newUser.Name.ShouldEqual(newName);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory( /* Skip = "Syncano.Net.SyncanoExceptionError: TypeError: _count_users() got an unexpected keyword argument 'type'")*/ ), PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionId_GetsCountOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            //when
            var result = await client.Count(request);

            //then
            result.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory( /* Skip = "Syncano.Net.SyncanoExceptionError: TypeError: _count_users() got an unexpected keyword argument 'type'")*/ ), PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionKey_GetsCountOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionKey = TestData.CollectionKey;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;

            //when
            var result = await client.Count(request);

            //then
            result.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory( /* Skip = "Syncano.Net.SyncanoExceptionError: TypeError: _count_users() got an unexpected keyword argument 'type'")*/ ), PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithModeratedState_GetsCountOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.State = DataObjectState.Moderated;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Moderated;

            //when
            var result = await client.Count(request);

            //then
            result.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory( /* Skip = "Syncano.Net.SyncanoExceptionError: TypeError: _count_users() got an unexpected keyword argument 'type'")*/ ), PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithPendingState_GetsCountOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.State = DataObjectState.Pending;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Pending;

            //when
            var result = await client.Count(request);

            //then
            result.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory( /* Skip = "Syncano.Net.SyncanoExceptionError: TypeError: _count_users() got an unexpected keyword argument 'type'")*/ ), PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithSingleFolderName_GetsCountOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.Folder = TestData.FolderName;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Folder = TestData.FolderName;

            //when
            var result = await client.Count(request);

            //then
            result.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory( /* Skip = "Syncano.Net.SyncanoExceptionError: TypeError: _count_users() got an unexpected keyword argument 'type'")*/ ), PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithFolderListName_GetsCountOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.Folder = TestData.FolderName;
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Folders = new List<string> { TestData.FolderName };

            //when
            var result = await client.Count(request);

            //then
            result.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory( /* Skip = "Syncano.Net.SyncanoExceptionError: TypeError: _count_users() got an unexpected keyword argument 'type'")*/ ), PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithImageContentFilter_GetsCountOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.ImageBase64 = TestData.ImageToBase64("smallSampleImage.png");
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Filter = DataObjectContentFilter.Image;

            //when
            var result = await client.Count(request);

            //then
            result.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory( /* Skip = "Syncano.Net.SyncanoExceptionError: TypeError: _count_users() got an unexpected keyword argument 'type'")*/ ), PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithTextContentFilter_GetsCountOfUsers(UserSyncanoClient client)
        {
            //given
            var dataRequest = new DataObjectDefinitionRequest();
            dataRequest.ProjectId = TestData.ProjectId;
            dataRequest.CollectionId = TestData.CollectionId;
            dataRequest.Text = "sample text content";
            await _dataClient.New(dataRequest);

            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Filter = DataObjectContentFilter.Text;

            //when
            var result = await client.Count(request);

            //then
            result.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;

            await _dataClient.Delete(deleteRequest);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithNullProjectId_ThrowsException(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Count(request);
                throw new Exception("Count should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithInvalidProjectId_ThrowsException(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();
            request.ProjectId = "abcde";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Count(request);
                throw new Exception("Count should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithInvalidCollectionId_ThrowsException(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = "abcde";

            try
            {
                //when
                await client.Count(request);
                throw new Exception("Count should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithInvalidCollectionKey_ThrowsException(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = "abcde";

            try
            {
                //when
                await client.Count(request);
                throw new Exception("Count should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithNullCollectionIdAndCollectionKey_ThrowsException(UserSyncanoClient client)
        {
            //given
            var request = new UserQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = "not null";
            request.CollectionKey = "not null";

            try
            {
                //when
                await client.Count(request);
                throw new Exception("Count should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserId_DeletesUserObject(UserSyncanoClient client)
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
        public async Task Delete_ByUserId_WithNick_DeletesUserObject(UserSyncanoClient client)
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
        public async Task Delete_ByUserId_WithPassword_DeletesUserObject(UserSyncanoClient client)
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
        public async Task Delete_ByUserId_WithAvatar_DeletesUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var avatar = TestData.ImageToBase64("smallSampleImage.png");
            var user = await client.New(name, password, avatar: avatar);

            //cleanup
            var result = await client.Delete(user.Id);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserName_DeletesUserObject(UserSyncanoClient client)
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
        public async Task Delete_ByUserName_WithNick_DeletesUserObject(UserSyncanoClient client)
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
        public async Task Delete_ByUserName_WithPassword_DeletesUserObject(UserSyncanoClient client)
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
        public async Task Delete_ByUserName_WithAvatar_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            const string name = "newUserName";
            const string password = "abcde123";
            var avatar = TestData.ImageToBase64("smallSampleImage.png");
            var user = await client.New(name, password, avatar: avatar);

            //cleanup
            var result = await client.Delete(userName: user.Name);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof (SyncanoClientsProvider))]
        public async Task Delete_WithNullUserIdAndName_ThrowsException(UserSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete();
                throw new Exception("Delete should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

    }
}
