using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Access;
using Xunit;

namespace Syncano.Net.Tests
{
    public class ApiKeyRestClientTests
    {
        private readonly Syncano _client;

        public ApiKeyRestClientTests()
        {
            _client = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task StartSession_WithNoTimezone()
        {
            //when
            var result = await _client.ApiKeys.StartSession();

            //then
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task StartSession_WithUtcTimezone()
        {
            //when
            var result = await _client.ApiKeys.StartSession(TimeZoneInfo.Utc);

            //then
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task StartSession_WithAllTimeZones()
        {
            //given
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            var sessionIds = new List<string>();

            //when
            foreach (var timeZoneInfo in timeZones)
            {
                try
                {
                    sessionIds.Add(await _client.ApiKeys.StartSession(timeZoneInfo));
                }
                catch(ArgumentException)
                { }

            }

            //then
            foreach (var sessionId in sessionIds)
            {
                sessionId.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task New_BackendType_CreatesNewApiKey()
        {
            //given
            var description = "apiKey description";

            //when
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.Backend, TestData.RoleId);

            //then
            apiKey.ShouldNotBeNull();
            apiKey.Description.ShouldEqual(description);

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task New_UserType_CreatesNewApiKey()
        {
            //given
            var description = "apiKey description";

            //when
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.User);

            //then
            apiKey.ShouldNotBeNull();
            apiKey.Description.ShouldEqual(description);

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task New_WithNullDescription_ThrowsException()
        {
            try
            {
                //when
                await _client.ApiKeys.New(null, roleId: TestData.RoleId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task New_WithRoleIdAndUserType_ThrowsException()
        {
            try
            {
                //when
                await _client.ApiKeys.New("description", ApiKeyType.User, TestData.RoleId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task Get_CreatesApiKeysList()
        {
            //when
            var result = await _client.ApiKeys.Get();

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(2);
            result.Any(a => a.ApiKeyValue == TestData.BackendAdminApiKey).ShouldBeTrue();
        }

        [Fact]
        public async Task GetOne_WithApiClientId_CreatesApiKeysList()
        {
            //when
            var result = await _client.ApiKeys.GetOne(TestData.BackendAdminApiId);

            //then
            result.ShouldNotBeNull();
            result.ApiKeyValue.ShouldEqual(TestData.BackendAdminApiKey);
            result.Type.ShouldEqual(ApiKeyType.Backend);
        }

        [Fact]
        public async Task GetOne_WithoutApiClientId_CreatesApiKeysList()
        {
            //when
            var result = await _client.ApiKeys.GetOne();

            //then
            result.ShouldNotBeNull();
            result.ApiKeyValue.ShouldEqual(TestData.BackendAdminApiKey);
            result.Type.ShouldEqual(ApiKeyType.Backend);
        }

        [Fact]
        public async Task Update_CreatesNewApiKey()
        {
            //given
            var description = "apiKey description";
            var newDescription = "new apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.Backend, TestData.RoleId);

            //when
            apiKey = await _client.ApiKeys.UpdateDescription(newDescription, apiKey.Id);

            //then
            apiKey.ShouldNotBeNull();
            apiKey.Description.ShouldEqual(newDescription);

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task Update_WithNullDescription_ThrowsException()
        {
            try
            {
                //when
                await _client.ApiKeys.UpdateDescription(null);
                throw new Exception("UpdateDescription should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Authorize_WithSendNotificationPermission()
        {
            //given
            var description = "apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.User);

            //when
            var result = await _client.ApiKeys.Authorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task Authorize_WithAddUserPermission()
        {
            //given
            var description = "apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.User);

            //when
            var result = await _client.ApiKeys.Authorize(apiKey.Id, ApiKeyPermission.AddUser);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task Authorize_WithAccessSyncPermission()
        {
            //given
            var description = "apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.User);

            //when
            var result = await _client.ApiKeys.Authorize(apiKey.Id, ApiKeyPermission.AccessSync);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task Authorize_WithSubscribePermission()
        {
            //given
            var description = "apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.User);

            //when
            var result = await _client.ApiKeys.Authorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task Authorize_WithNullApiKeyId_ThrowsException()
        {
            try
            {
                //when
                await _client.ApiKeys.Authorize(null, ApiKeyPermission.AccessSync);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Deauthorize_WithSendNotificationPermission()
        {
            //given
            var description = "apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.User);
            await _client.ApiKeys.Authorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //when
            var result = await _client.ApiKeys.Deauthorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task Deauthorize_WithAddUserPermission()
        {
            //given
            var description = "apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.User);
            await _client.ApiKeys.Authorize(apiKey.Id, ApiKeyPermission.AddUser);

            //when
            var result = await _client.ApiKeys.Deauthorize(apiKey.Id, ApiKeyPermission.AddUser);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task Deauthorize_WithAccessSyncPermission()
        {
            //given
            var description = "apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.User);
            await _client.ApiKeys.Authorize(apiKey.Id, ApiKeyPermission.AccessSync);

            //when
            var result = await _client.ApiKeys.Deauthorize(apiKey.Id, ApiKeyPermission.AccessSync);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task Deauthorize_WithSubscribePermission()
        {
            //given
            var description = "apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.User);
            await _client.ApiKeys.Authorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //when
            var result = await _client.ApiKeys.Deauthorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.ApiKeys.Delete(apiKey.Id);
        }

        [Fact]
        public async Task Deauthorize_WithNullApiKeyId_ThrowsException()
        {
            try
            {
                //when
                await _client.ApiKeys.Deauthorize(null, ApiKeyPermission.AccessSync);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Delete_BackendType_CreatesNewApiKey()
        {
            //given
            var description = "apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.Backend, TestData.RoleId);

            //when
            var result = await _client.ApiKeys.Delete(apiKey.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_UserType_CreatesNewApiKey()
        {
            //given
            var description = "apiKey description";
            var apiKey = await _client.ApiKeys.New(description, ApiKeyType.User);

            //when
            var result = await _client.ApiKeys.Delete(apiKey.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_WithNullApiKey_ThrowsException()
        {
            try
            {
                //when
                await _client.ApiKeys.Delete(null);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }
    }
}
