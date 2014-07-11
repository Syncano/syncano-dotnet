using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Xunit;

namespace Syncano.Net.Tests
{
    public class FolderRestClientTests : IDisposable
    {
        private Syncano _client;

        public FolderRestClientTests()
        {
            _client = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task New_ByCollectionId()
        {
            //given
            string folderName = "NewFolderTest  " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            //when
            var folder = await _client.Folders.New(TestData.ProjectId, folderName,
                TestData.CollectionId);

            await _client.Folders.Delete(TestData.ProjectId, folderName, TestData.CollectionId);

            //then
            folder.Id.ShouldNotEqual(null);
        }

        [Fact]
        public async Task New_ByCollectionKey()
        {
            //given
            string folderName = "NewFolderTest  " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToShortDateString();

            //when
            var folder = await _client.Folders.New(TestData.ProjectId, folderName,
                collectionKey: TestData.CollectionKey);

            await _client.Folders.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //then
            folder.Id.ShouldNotEqual(null);
        }

        [Fact]
        public async Task New_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                var response = await _client.Folders.New(TestData.ProjectId, TestData.FolderName, null, null);
                throw new Exception("Get folders should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Get_ByCollectionId()
        {
            //when
            var response = await _client.Folders.Get(TestData.ProjectId, TestData.CollectionId);

            //then
            response.ShouldNotBeEmpty();
            response.Any(f => f.Name == TestData.FolderName).ShouldBeTrue();
        }

        [Fact]
        public async Task Get_ByCollectionKey()
        {
            //when
            var response = await _client.Folders.Get(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            //then
            response.ShouldNotBeEmpty();
            response.Any(f => f.Name == TestData.FolderName).ShouldBeTrue();
        }

        [Fact]
        public async Task Get_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                var response = await _client.Folders.Get(TestData.ProjectId, null, null);
                throw new Exception("Get folders should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task GetOne_ByCollectionId()
        {
            //when
            var folder = await _client.Folders.GetOne(TestData.ProjectId, TestData.FolderName, TestData.CollectionId);

            //then
            folder.Id.ShouldEqual(TestData.FolderId);
            folder.Name.ShouldEqual(TestData.FolderName);
        }

        [Fact]
        public async Task GetOne_ByCollectionKey()
        {
            //when
            var folder = await _client.Folders.GetOne(TestData.ProjectId, TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            folder.Id.ShouldEqual(TestData.FolderId);
            folder.Name.ShouldEqual(TestData.FolderName);
        }

        [Fact]
        public async Task GetOne_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                var response = await _client.Folders.GetOne(TestData.ProjectId, TestData.FolderName, null, null);
                throw new Exception("GetFolders should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Update_ByCollectionId()
        {
            //given
            string folderName = "Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            string newFolderName = "Update test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            await _client.Folders.New(TestData.ProjectId, folderName, TestData.CollectionId);

            //when
            var result = await _client.Folders.Update(TestData.ProjectId, folderName, TestData.CollectionId, null, newFolderName,
                        "qwerty");

            await _client.Folders.Delete(TestData.ProjectId, newFolderName, TestData.CollectionId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Update_ByCollectionKey()
        {
            //given
            string folderName = "Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            string newFolderName = "Update test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            await _client.Folders.New(TestData.ProjectId, folderName, TestData.CollectionId);

            //when
            var result = await _client.Folders.Update(TestData.ProjectId, folderName, null, TestData.CollectionKey, newFolderName,
                        "qwerty");

            await _client.Folders.Delete(TestData.ProjectId, newFolderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Update_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Update(TestData.ProjectId, TestData.FolderName, null, null);
                throw new Exception("GetFolders should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Authorize_ByCollectionId()
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await _client.Folders.New(TestData.ProjectId, folderName, TestData.CollectionId);

            //when
            var result = await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);

            await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);

            await _client.Folders.Delete(TestData.ProjectId, folderName, TestData.CollectionId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Authorize_ByCollectionKey()
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await _client.Folders.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            await _client.Folders.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Authorize_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, null, null);
                throw new Exception("AuthorizeFolder should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Deauthorize_ByCollectionId()
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await _client.Folders.New(TestData.ProjectId, folderName, TestData.CollectionId);
            await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);

            //when
            var result = await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);

            await _client.Folders.Delete(TestData.ProjectId, folderName, TestData.CollectionId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Deauthorize_ByCollectionKey()
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await _client.Folders.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
            await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            await _client.Folders.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Deauthorize_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, null, null);
                throw new Exception("DeauthorizeFolder should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Delete_ByCollectionId()
        {
            //given
            string folderName = "Delete test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            await _client.Folders.New(TestData.ProjectId, folderName, TestData.CollectionId);

            //when
            var result = await _client.Folders.Delete(TestData.ProjectId, folderName, TestData.CollectionId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_ByCollectionKey()
        {
            //given
            string folderName = "Delete test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            await _client.Folders.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await _client.Folders.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Delete(TestData.ProjectId, TestData.FolderName, null, null);
                throw new Exception("GetFolders should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        public void Dispose()
        {

        }
    }
}
