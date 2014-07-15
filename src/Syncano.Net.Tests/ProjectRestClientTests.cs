using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Xunit;

namespace Syncano.Net.Tests
{
    public class ProjectRestClientTests : IDisposable
    {
        private Syncano _client;

        public ProjectRestClientTests()
        {
            _client = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task New_WithoutDescription_CreatesNewProjectObject()
        {
            //given
            string projectName = "NewProject test " + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();

            //when
            var project = await _client.Projects.New(projectName);

            //then
            project.ShouldNotBeNull();
            project.Id.ShouldNotBeNull();
            project.Name.ShouldEqual(projectName);
            project.Description.ShouldBeNull();

            //cleanup
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task New_WithDescription_CreatesNewProjectObject()
        {
            //given
            string projectName = "NewProject test " + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();
            string projectDescription = "qwerty";

            //when
            var project = await _client.Projects.New(projectName, projectDescription);

            //then
            project.ShouldNotBeNull();
            project.Id.ShouldNotBeNull();
            project.Name.ShouldEqual(projectName);
            project.Description.ShouldEqual(projectDescription);

            //cleanup
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task New_WithoutName_ThrowsException()
        {
            try
            {
                //when
                var project = await _client.Projects.New(null);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Get_CreatesProjectsList()
        {
            //when
            var response = await _client.Projects.Get();

            //then
            response.ShouldNotBeEmpty();
            response.Any(p => p.Name == TestData.ProjectName).ShouldBeTrue();
        }

        [Fact]
        public async Task Get_CreatesProjectsList_MultipleProjects()
        {
            //given
            var count = 10;
            for (int i = 0; i < count; ++i)
                await _client.Projects.New("Test project " + i);

            //when
            var response = await _client.Projects.Get();

            //then
            response.ShouldNotBeEmpty();
            response.Any(p => p.Name == TestData.ProjectName).ShouldBeTrue();
            response.Count.ShouldEqual(count+1);

            //cleanup
            foreach (var project in response)
            {
                if(project.Id != TestData.ProjectId)
                    await _client.Projects.Delete(project.Id);
            }
        }

        [Fact]
        public async Task GetOne_CreatesNewProjectObject()
        {
            //when
            var project = await _client.Projects.GetOne(TestData.ProjectId);

            //then
            project.Id.ShouldEqual(TestData.ProjectId);
            project.Name.ShouldEqual(TestData.ProjectName);
            project.Name.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetOne_WithoutProjectId_ThrowsException()
        {
            try
            {
                //when
                var result = await _client.Projects.GetOne(null);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task GetOne_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                var result = await _client.Projects.GetOne("abcde");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Update_CreatesProjectObjectWithNewValues()
        {
            //given
            string projectName = "UpdateProject test " + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();
            string projectNewName = "UpdateProject test new name" + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();
            string projectOldDescription = "qwerty";
            string projectNewDescription = "abc";
            var project = await _client.Projects.New(projectName, projectOldDescription);

            //when
            project = await _client.Projects.Update(project.Id, projectNewName, projectNewDescription);
            
            //then
            project.ShouldNotBeNull();
            project.Name.ShouldEqual(projectNewName);
            project.Description.ShouldEqual(projectNewDescription);

            //cleanup
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task Update_WithNoNewValues()
        {
            //given
            string projectName = "UpdateProject test " + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);

            //when
            project = await _client.Projects.Update(project.Id);

            //then
            project.ShouldNotBeNull();
            project.Name.ShouldEqual(projectName);
            project.Description.ShouldBeNull();

            //cleanup
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task Update_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                var result = await _client.Projects.Update("abc");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Update_WithNoProjectId_ThrowsException()
        {
            try
            {
                //when
                var result = await _client.Projects.Update(null);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Authorize_ReadDataPermission()
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);

            //when
            var result = await _client.Projects.Authorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);
            
            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Projects.Deauthorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task Authorize_CreateDataPermission()
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);

            //when
            var result = await _client.Projects.Authorize(TestData.UserApiClientId, Permissions.CreateData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Projects.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, project.Id);
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task Authorize_DeleteDataPermission()
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);

            //when
            var result = await _client.Projects.Authorize(TestData.UserApiClientId, Permissions.DeleteData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Projects.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, project.Id);
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task Authorize_DeleteOwnDataPermission()
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);

            //when
            var result = await _client.Projects.Authorize(TestData.UserApiClientId, Permissions.DeleteOwnData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Projects.Deauthorize(TestData.UserApiClientId, Permissions.DeleteOwnData, project.Id);
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task Authorize_ReadOwnDataPermission()
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);

            //when
            var result = await _client.Projects.Authorize(TestData.UserApiClientId, Permissions.ReadOwnData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Projects.Deauthorize(TestData.UserApiClientId, Permissions.ReadOwnData, project.Id);
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task Authorize_UpdateDataPermission()
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);

            //when
            var result = await _client.Projects.Authorize(TestData.UserApiClientId, Permissions.UpdateData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Projects.Deauthorize(TestData.UserApiClientId, Permissions.UpdateData, project.Id);
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task Authorize_UpdateOwnDataPermission()
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);

            //when
            var result = await _client.Projects.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Projects.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData, project.Id);
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task Authorize_WithNoUserApiKey_ThrowsException()
        {
            try
            {
                //when
                var result = await _client.Projects.Authorize(null, Permissions.CreateData, TestData.ProjectId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Authorize_WithInvalidUserApiKey_ThrowsException()
        {
            try
            {
                //when
                var result = await _client.Projects.Authorize("abcde", Permissions.CreateData, TestData.ProjectId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Deauthorize_WithReadDataPermissions()
        {
            //given
            string projectName = "Deauthorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);
            await _client.Projects.Authorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);

            //when
            var result = await _client.Projects.Deauthorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Projects.Delete(project.Id);
        }

        [Fact]
        public async Task Deauthorize_WithNoUserApiKey_ThrowsException()
        {
            try
            {
                //when
                var result = await _client.Projects.Deauthorize(null, Permissions.CreateData, TestData.ProjectId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Deauthorize_WithInvalidUserApiKey_ThrowsException()
        {
            try
            {
                //when
                var result = await _client.Projects.Deauthorize("abcde", Permissions.CreateData, TestData.ProjectId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Delete()
        {
            //given
            string projectName = "NewProject test " + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();
            string projectDescription = "qwerty";
            var project = await _client.Projects.New(projectName, projectDescription);

            //when
            var result = await _client.Projects.Delete(project.Id);

            //then
            result.ShouldBeTrue();
        }

        public void Dispose()
        {
        }
    }
}
