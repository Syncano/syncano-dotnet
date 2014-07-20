using System;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Xunit;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class AdministratorSyncanoClientTests
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

        [Theory(Skip = "Permission denied."), PropertyData("AdministratorSyncanoClients", PropertyType = typeof (SyncanoClientsProvider))]
        public async Task New_CreatesNewAdmin(AdministratorSyncanoClient client)
        {
            //given
            var email = "admin@email.com";

            //when
            var result = await client.New(email, "4", "Invite message");

            //then
            result.ShouldBeTrue();

            //cleanup
            var admin = await client.GetOne(adminEmail: email);
            await client.Delete(adminId: admin.Id);
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullAdminEmail_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.New(null, TestData.RoleId, "invitation message");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullRoleIdl_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.New("sample@email.com", null, "invitation message");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullMessage_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.New("sample@email.com", TestData.RoleId, null);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithInvalidRoleId_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.New("sample@email.com", "99", "invitation message");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_GetsListOfAdministrator(AdministratorSyncanoClient client)
        {
            //when
            var result = await client.Get();

            //then
            result.ShouldNotBeEmpty();
            result.Count.ShouldEqual(2);
            result.Any(a => a.Role.Id == "1").ShouldBeTrue();
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByAdminId_CreatesAdministrator(AdministratorSyncanoClient client)
        {
            //when
            var result = await client.GetOne(TestData.AdminId);

            //then
            result.ShouldNotBeNull();
            result.Email.ShouldEqual(TestData.AdminEmail);
        }

        [Fact(Skip = "Syncano admin permissionDenied error")]
        public async Task GetOne_ByAdminEmail_CreatesAdministrator(AdministratorSyncanoClient client)
        {
            //when
            var result = await client.GetOne(adminEmail: TestData.AdminEmail);

            //then
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(TestData.AdminId);
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithNullAdminIdAndEmail_ThrowsException(AdministratorSyncanoClient client)
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

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithInvalidAdminId_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne("9999");
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithInvalidAdminEmail_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(adminEmail: "abcde@email.com");
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNullAdminIdAndEmail_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(TestData.RoleId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidAdminId_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(TestData.RoleId, "99");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidAdminEmail_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(TestData.RoleId, adminEmail: "sample@email.com");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNullRoleId_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(null, TestData.AdminId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidRoldeId_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.Update("99", TestData.AdminId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithNullAdminIdAndEmail_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete();
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithInvalidAdminId_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete("9999");
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithInvalidAdminEmail_ThrowsException(AdministratorSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(adminEmail:"abcde@email.com");
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory(Skip = "Permission denied."), PropertyData("AdministratorSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_DeletesAdmin(AdministratorSyncanoClient client)
        {
            //given
            var email = "admin@email.com";
            await client.New(email, "4", "Invite message");

            //when
            var result = await client.Delete(adminEmail: email);

            //then
            result.ShouldBeTrue();
        }
    }
}
