using System;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Xunit;

namespace Syncano.Net.Tests
{
    public class AdministratorRestClientTests
    {
        private readonly Syncano _client;

        public AdministratorRestClientTests()
        {
            _client = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task New_WithNullAdminEmail_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.New(null, TestData.RoleId, "invitation message");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task New_WithNullRoleIdl_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.New("sample@email.com", null, "invitation message");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task New_WithNullMessage_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.New("sample@email.com", TestData.RoleId, null);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task New_WithInvalidRoleId_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.New("sample@email.com", "99", "invitation message");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetRoles_CreatesListOfRoles()
        {
            //when
            var result = await _client.Administrators.GetRoles();

            //then
            result.ShouldNotBeEmpty();
            result.Count.ShouldEqual(4);
        }

        [Fact]
        public async Task Get_CreatesListOfAdministrator()
        {
            //when
            var result = await _client.Administrators.Get();

            //then
            result.ShouldNotBeEmpty();
            result.Count.ShouldEqual(2);
            result.Any(a => a.Role.Id == "1").ShouldBeTrue();
        }

        [Fact]
        public async Task GetOne_ByAdminId_CreatesAdministrator()
        {
            //when
            var result = await _client.Administrators.GetOne(TestData.AdminId);

            //then
            result.ShouldNotBeNull();
            result.Email.ShouldEqual(TestData.AdminEmail);
        }

        [Fact(Skip = "Syncano admin permissionDenied error")]
        public async Task GetOne_ByAdminEmail_CreatesAdministrator()
        {
            //when
            var result = await _client.Administrators.GetOne(adminEmail: TestData.AdminEmail);

            //then
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(TestData.AdminId);
        }

        [Fact]
        public async Task GetOne_WithNullAdminIdAndEmail_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.GetOne();
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task GetOne_WithInvalidAdminId_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.GetOne("9999");
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetOne_WithInvalidAdminEmail_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.GetOne(adminEmail: "abcde@email.com");
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Update_WithNullAdminIdAndEmail_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.Update(TestData.RoleId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Update_WithInvalidAdminId_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.Update(TestData.RoleId, "99");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Update_WithAdminEmail_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.Update(TestData.RoleId, adminEmail: "sample@email.com");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Update_WithNullRoleId_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.Update(null, TestData.AdminId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Update_WithInvalidRoldeId_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.Update("99", TestData.AdminId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Delete_WithNullAdminIdAndEmail_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.Delete();
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Delete_WithInvalidAdminId_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.Delete("9999");
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Delete_WithInvalidAdminEmail_ThrowsException()
        {
            try
            {
                //when
                await _client.Administrators.Delete(adminEmail:"abcde@email.com");
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
