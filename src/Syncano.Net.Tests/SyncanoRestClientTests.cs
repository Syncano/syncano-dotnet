using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Xunit;

namespace Syncano.Net.Tests
{
    public static class TestData
    {
        public const string InstanceName = "icy-brook-267066";

        public const string BackendAdminApiKey = "f020f3a62b2ea236100a732adcf60cb98683e2e5";

        public const string ProjectName = "Default";
        public const string ProjectId = "1288";

        public const string CollectionId = "4995";
        public const string CollectionKey = "default";

        public const string FolderName = "Default";
        public const string FolderId = "1";
    }

    public class SyncanoRestClientTests : IDisposable
    {
        private SyncanoRestClient _client;


        public SyncanoRestClientTests()
        {
            _client = new SyncanoRestClient(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task StartSession_WhenValidInstanceAndKey_CreatesSessionId()
        {
            //when 
            var sessionId = await _client.StartSession();

            //then
            sessionId.ShouldNotBeNull();
        }


        [Fact]
        public async Task StartSession_WithInvalidKeys_ThrowsException()
        {
            try
            {
                var session = await new SyncanoRestClient(TestData.InstanceName, "2123").StartSession();

                throw new Exception("StartSession should throw exception");
                
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetProjects()
        {
            //when
            var response = await _client.GetProjects();

            //then
            response.ShouldNotBeEmpty();
            response.Any(p => p.Name == TestData.ProjectName).ShouldBeTrue();
        }

        [Fact]
        public async Task GetProject()
        {
            //when
            var project = await _client.GetProject(TestData.ProjectId);

            //then
            project.Id.ShouldEqual(TestData.ProjectId);
            project.Name.ShouldNotBeNull();
        }

        [Fact]
        public async Task NewFolder_ByCollectionId()
        {
            //when
            var folder = await _client.NewFolder(TestData.ProjectId, "NewFolderTest  " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToShortDateString(),
                TestData.CollectionId);

            //then
            folder.Id.ShouldNotEqual(null);
        }

        [Fact]
        public async Task NewFolder_ByCollectionKey()
        {
            //when
            var folder = await _client.NewFolder(TestData.ProjectId, "NewFolderTest  " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToShortDateString(),
                collectionKey: TestData.CollectionKey);

            //then
            folder.Id.ShouldNotEqual(null);
        }

        [Fact]
        public async Task NewFolder_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                var response = await _client.NewFolder(TestData.ProjectId, TestData.FolderName,null, null);
                throw new Exception("Get folders should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task GetFolders_ByCollectionId()
        {
            //when
            var response = await _client.GetFolders(TestData.ProjectId, TestData.CollectionId);

            //then
            response.ShouldNotBeEmpty();
            response.Any(f => f.Name == TestData.FolderName).ShouldBeTrue();
        }

        [Fact]
        public async Task GetFolders_ByCollectionKey()
        {
            //when
            var response = await _client.GetFolders(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            //then
            response.ShouldNotBeEmpty();
            response.Any(f => f.Name == TestData.FolderName).ShouldBeTrue();
        }

        [Fact]
        public async Task GetFolders_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                var response = await _client.GetFolders(TestData.ProjectId, null, null);
                throw new Exception("Get folders should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task GetFolder_ByCollectionId()
        {
            //when
            var folder = await _client.GetFolder(TestData.ProjectId, TestData.FolderName, TestData.CollectionId);

            //then
            folder.Id.ShouldEqual(TestData.FolderId);
            folder.Name.ShouldEqual(TestData.FolderName);
        }

        [Fact]
        public async Task GetFolder_ByCollectionKey()
        {
            //when
            var folder = await _client.GetFolder(TestData.ProjectId, TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            folder.Id.ShouldEqual(TestData.FolderId);
            folder.Name.ShouldEqual(TestData.FolderName);
        }

        [Fact]
        public async Task GetFolder_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                var response = await _client.GetFolder(TestData.ProjectId, TestData.FolderName,null, null);
                throw new Exception("GetFolders should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task DeleteFolder_ByCollectionId()
        {
            //when
            string folderName = "Delete test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            await _client.NewFolder(TestData.ProjectId, folderName, TestData.CollectionId);
            await _client.DeleteFolder(TestData.ProjectId, folderName, TestData.CollectionId);

            //then
            try
            {
                var folder = await _client.GetFolder(TestData.ProjectId, folderName, TestData.CollectionId);
                throw new Exception("GetFolder should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task DeleteFolder_ByCollectionKey()
        {
            //when
            string folderName = "Delete test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            await _client.NewFolder(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
            await _client.DeleteFolder(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //then
            try
            {
                var folder = await _client.GetFolder(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
                throw new Exception("GetFolder should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task DeleteFolder_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                await _client.DeleteFolder(TestData.ProjectId, TestData.FolderName, null, null);
                throw new Exception("GetFolders should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }
        }


        public void Dispose()
        {
        }
    }
}