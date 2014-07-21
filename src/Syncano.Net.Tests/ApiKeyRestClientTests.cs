using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Access;
using Syncano.Net.Api;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class ApiKeyRestClientTests
    {
        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetRoles_GetsListOfRoles(AdministratorSyncanoClient client)
        {
            //when
            var result = await client.GetRoles();

            //then
            result.ShouldNotBeEmpty();
            result.Count.ShouldEqual(4);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task StartSession_WithNoTimezone(ApiKeySyncanoClient client)
        {
            //when
            var result = await client.StartSession();

            //then
            result.ShouldNotBeNull();
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task StartSession_WithUtcTimezone(ApiKeySyncanoClient client)
        {
            //when
            var result = await client.StartSession(TimeZoneInfo.Utc);

            //then
            result.ShouldNotBeNull();
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task StartSession_WithAllTimeZones(ApiKeySyncanoClient client)
        {
            //given
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            var sessionIds = new List<string>();

            //when
            foreach (var timeZoneInfo in timeZones)
            {
                try
                {
                    sessionIds.Add(await client.StartSession(timeZoneInfo));
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

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_BackendType_CreatesNewApiKey(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";

            //when
            var apiKey = await client.New(description, ApiKeyType.Backend, TestData.RoleId);

            //then
            apiKey.ShouldNotBeNull();
            apiKey.Description.ShouldEqual(description);

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_UserType_CreatesNewApiKey(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";

            //when
            var apiKey = await client.New(description, ApiKeyType.User);

            //then
            apiKey.ShouldNotBeNull();
            apiKey.Description.ShouldEqual(description);

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullDescription_ThrowsException(ApiKeySyncanoClient client)
        {
            try
            {
                //when
                await client.New(null, roleId: TestData.RoleId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithRoleIdAndUserType_ThrowsException(ApiKeySyncanoClient client)
        {
            try
            {
                //when
                await client.New("description", ApiKeyType.User, TestData.RoleId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithInvalidRoleId_ThrowsException(ApiKeySyncanoClient client)
        {
            try
            {
                //when
                await client.New("description", ApiKeyType.Backend, "9999");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_GetsApiKeysList(ApiKeySyncanoClient client)
        {
            //when
            var result = await client.Get();

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(2);
            result.Any(a => a.ApiKeyValue == TestData.BackendAdminApiKey).ShouldBeTrue();
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithApiClientId_CreatesApiKeysList(ApiKeySyncanoClient client)
        {
            //when
            var result = await client.GetOne(TestData.BackendAdminApiId);

            //then
            result.ShouldNotBeNull();
            result.ApiKeyValue.ShouldEqual(TestData.BackendAdminApiKey);
            result.Type.ShouldEqual(ApiKeyType.Backend);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithoutApiClientId_CreatesApiKeysList(ApiKeySyncanoClient client)
        {
            //when
            var result = await client.GetOne();

            //then
            result.ShouldNotBeNull();
            result.ApiKeyValue.ShouldEqual(TestData.BackendAdminApiKey);
            result.Type.ShouldEqual(ApiKeyType.Backend);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithInvalidApiKey_ThrowsException(ApiKeySyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne("abcde");
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_UpdatesApiKey(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var newDescription = "new apiKey description";
            var apiKey = await client.New(description, ApiKeyType.Backend, TestData.RoleId);

            //when
            apiKey = await client.UpdateDescription(newDescription, apiKey.Id);

            //then
            apiKey.ShouldNotBeNull();
            apiKey.Description.ShouldEqual(newDescription);

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNullDescription_ThrowsException(ApiKeySyncanoClient client)
        {
            try
            {
                //when
                await client.UpdateDescription(null);
                throw new Exception("UpdateDescription should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidApiKeyId_ThrowsException(ApiKeySyncanoClient client)
        {
            try
            {
                //when
                await client.UpdateDescription("description", "abcde123");
                throw new Exception("UpdateDescription should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithSendNotificationPermission(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var apiKey = await client.New(description, ApiKeyType.User);

            //when
            var result = await client.Authorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithAddUserPermission(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var apiKey = await client.New(description, ApiKeyType.User);

            //when
            var result = await client.Authorize(apiKey.Id, ApiKeyPermission.AddUser);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithAccessSyncPermission(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var apiKey = await client.New(description, ApiKeyType.User);

            //when
            var result = await client.Authorize(apiKey.Id, ApiKeyPermission.AccessSync);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithSubscribePermission(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var apiKey = await client.New(description, ApiKeyType.User);

            //when
            var result = await client.Authorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithNullApiKeyId_ThrowsException(ApiKeySyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize(null, ApiKeyPermission.AccessSync);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithSendNotificationPermission(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var apiKey = await client.New(description, ApiKeyType.User);
            await client.Authorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //when
            var result = await client.Deauthorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithAddUserPermission(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var apiKey = await client.New(description, ApiKeyType.User);
            await client.Authorize(apiKey.Id, ApiKeyPermission.AddUser);

            //when
            var result = await client.Deauthorize(apiKey.Id, ApiKeyPermission.AddUser);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithAccessSyncPermission(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var apiKey = await client.New(description, ApiKeyType.User);
            await client.Authorize(apiKey.Id, ApiKeyPermission.AccessSync);

            //when
            var result = await client.Deauthorize(apiKey.Id, ApiKeyPermission.AccessSync);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithSubscribePermission(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var apiKey = await client.New(description, ApiKeyType.User);
            await client.Authorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //when
            var result = await client.Deauthorize(apiKey.Id, ApiKeyPermission.SendNotification);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(apiKey.Id);
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithNullApiKeyId_ThrowsException(ApiKeySyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(null, ApiKeyPermission.AccessSync);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_BackendType_DeletesApiKey(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var apiKey = await client.New(description, ApiKeyType.Backend, TestData.RoleId);

            //when
            var result = await client.Delete(apiKey.Id);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_UserType_DeletesApiKey(ApiKeySyncanoClient client)
        {
            //given
            var description = "apiKey description";
            var apiKey = await client.New(description, ApiKeyType.User);

            //when
            var result = await client.Delete(apiKey.Id);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithNullApiKey_ThrowsException(ApiKeySyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(null);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }


        [Theory, PropertyData("ApiKeySyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithInvalidApiKey_ThrowsException(ApiKeySyncanoClient client)
        {
            try
            {
                //when
                await client.Delete("abcde123");
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }
    }
}
