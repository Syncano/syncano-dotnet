using System;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Syncano.Net.Data;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class ProjectSyncanoClientTests : IDisposable
    {
      
       
        [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithoutDescription_CreatesNewProjectObject(ProjectSyncanoClient client)
        {
            //given
            string projectName = "NewProject test " + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();

            //when
            var project = await client.New(projectName);

            //then
            project.ShouldNotBeNull();
            project.Id.ShouldNotBeNull();
            project.Name.ShouldEqual(projectName);
            project.Description.ShouldBeNull();

            //cleanup
            await client.Delete(project.Id);
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithDescription_CreatesNewProjectObject(ProjectSyncanoClient client)
        {
            //given
            string projectName = "NewProject test " + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();
            const string projectDescription = "qwerty";

            //when
            var project = await client.New(projectName, projectDescription);

            //then
            project.ShouldNotBeNull();
            project.Id.ShouldNotBeNull();
            project.Name.ShouldEqual(projectName);
            project.Description.ShouldEqual(projectDescription);

            //cleanup
            await client.Delete(project.Id);
        }

        [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithoutName_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.New(null);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_CreatesProjectsList(ProjectSyncanoClient client)
        {
            //when
            var response = await client.Get();

            //then
            response.ShouldNotBeEmpty();
            response.Any(p => p.Name == TestData.ProjectName).ShouldBeTrue();
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_CreatesProjectsList_MultipleProjects(ProjectSyncanoClient client)
        {
            //given
            const int count = 10;
            for (int i = 0; i < count; ++i)
                await client.New("Test project " + i);

            //when
            var response = await client.Get();

            //then
            response.ShouldNotBeEmpty();
            response.Any(p => p.Name == TestData.ProjectName).ShouldBeTrue();
            response.Count.ShouldEqual(count+1);

            //cleanup
            foreach (var project in response)
            {
                if(project.Id != TestData.ProjectId)
                    await client.Delete(project.Id);
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_CreatesNewProjectObject(ProjectSyncanoClient client)
        {
            //when
            var project = await client.GetOne(TestData.ProjectId);

            //then
            project.Id.ShouldEqual(TestData.ProjectId);
            project.Name.ShouldEqual(TestData.ProjectName);
            project.Name.ShouldNotBeNull();
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithoutProjectId_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(null);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithInvalidProjectId_ThrowsException(ProjectSyncanoClient client)
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

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_CreatesProjectObjectWithNewValues(ProjectSyncanoClient client)
        {
            //given
            string projectName = "UpdateProject test " + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();
            string projectNewName = "UpdateProject test new name" + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();
            const string projectOldDescription = "qwerty";
            const string projectNewDescription = "abc";
            var project = await client.New(projectName, projectOldDescription);

            //when
            project = await client.Update(project.Id, projectNewName, projectNewDescription);
            
            //then
            project.ShouldNotBeNull();
            project.Name.ShouldEqual(projectNewName);
            project.Description.ShouldEqual(projectNewDescription);

            //cleanup
            await client.Delete(project.Id);
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNoNewValues(ProjectSyncanoClient client)
        {
            //given
            string projectName = "UpdateProject test " + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();

            var project = await client.New(projectName);

            //when
            project = await client.Update(project.Id);

            //then
            project.ShouldNotBeNull();
            project.Name.ShouldEqual(projectName);
            project.Description.ShouldBeNull();

            //cleanup
            await client.Delete(project.Id);
        }

        [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidProjectId_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.Update("abc");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNoProjectId_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(null);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ReadDataPermission(ProjectSyncanoClient client)
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await client.New(projectName);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);
            
            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Deauthorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);
            await client.Delete(project.Id);
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_CreateDataPermission(ProjectSyncanoClient client)
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await client.New(projectName);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.CreateData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, project.Id);
            await client.Delete(project.Id);
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_DeleteDataPermission(ProjectSyncanoClient client)
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await client.New(projectName);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.DeleteData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, project.Id);
            await client.Delete(project.Id);
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_DeleteOwnDataPermission(ProjectSyncanoClient client)
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await client.New(projectName);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.DeleteOwnData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteOwnData, project.Id);
            await client.Delete(project.Id);
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
         public async Task Authorize_ReadOwnDataPermission(ProjectSyncanoClient client)
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await client.New(projectName);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.ReadOwnData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Deauthorize(TestData.UserApiClientId, Permissions.ReadOwnData, project.Id);
            await client.Delete(project.Id);
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_UpdateDataPermission(ProjectSyncanoClient client)
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await client.New(projectName);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.UpdateData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Deauthorize(TestData.UserApiClientId, Permissions.UpdateData, project.Id);
            await client.Delete(project.Id);
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_UpdateOwnDataPermission(ProjectSyncanoClient client)
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await client.New(projectName);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData, project.Id);
            await client.Delete(project.Id);
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithNoUserApiKey_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize(null, Permissions.CreateData, TestData.ProjectId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithInvalidUserApiKey_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize("abcde", Permissions.CreateData, TestData.ProjectId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithReadDataPermissions(ProjectSyncanoClient client)
        {
            //given
            string projectName = "Deauthorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await client.New(projectName);
            await client.Authorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);

            //when
            var result = await client.Deauthorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(project.Id);
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithNoUserApiKey_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(null, Permissions.CreateData, TestData.ProjectId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithInvalidUserApiKey_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize("abcde", Permissions.CreateData, TestData.ProjectId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
         public async Task Deauthorize_WithNoProjectId_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, null);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithInvalidProjectId_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, "abcde");
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
         public async Task Delete(ProjectSyncanoClient client)
        {
            //given
            string projectName = "NewProject test " + DateTime.Now.ToLongTimeString() + " " +
                                 DateTime.Now.ToShortDateString();
            const string projectDescription = "qwerty";
            var project = await client.New(projectName, projectDescription);

            //when
            var result = await client.Delete(project.Id);

            //then
            result.ShouldBeTrue();
        }

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
         public async Task Delete_WithNoProjectId_ThrowsException(ProjectSyncanoClient client)
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

         [Theory, PropertyData("ProjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
         public async Task Delete_WithInvalidProjectId_ThrowsException(ProjectSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete("abcde");
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        public void Dispose()
        {
        }
    }
}
