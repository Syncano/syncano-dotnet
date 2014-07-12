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

            await _client.Projects.Delete(project.Id);

            //then
            project.ShouldNotBeNull();
            project.Description.ShouldBeNull();
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

            await _client.Projects.Delete(project.Id);

            //then
            project.ShouldNotBeNull();
            project.Description.ShouldEqual(projectDescription);
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
        public async Task GetOne_CreatesNewProjectObject()
        {
            //when
            var project = await _client.Projects.GetOne(TestData.ProjectId);

            //then
            project.Id.ShouldEqual(TestData.ProjectId);
            project.Name.ShouldNotBeNull();
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
            await _client.Projects.Delete(project.Id);

            //then
            project.ShouldNotBeNull();
            project.Name.ShouldEqual(projectNewName);
            project.Description.ShouldEqual(projectNewDescription);
        }

        [Fact]
        public async Task Authorize()
        {
            //given
            string projectName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);

            //when
            var result = await _client.Projects.Authorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);
            await _client.Projects.Deauthorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);
            await _client.Projects.Delete(project.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Deauthorize()
        {
            //given
            string projectName = "Deauthorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            var project = await _client.Projects.New(projectName);
            await _client.Projects.Authorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);

            //when
            var result = await _client.Projects.Deauthorize(TestData.UserApiClientId, Permissions.ReadData, project.Id);
            await _client.Projects.Delete(project.Id);

            //then
            result.ShouldBeTrue();
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
