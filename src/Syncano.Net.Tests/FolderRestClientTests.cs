using System;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Data;
using Xunit;

namespace Syncano.Net.Tests
{
    public class FolderRestClientTests : IDisposable
    {
        private readonly Syncano _client;

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

            //then
            folder.Id.ShouldNotEqual(null);

            //cleanup
            await _client.Folders.Delete(TestData.ProjectId, folderName, TestData.CollectionId);
        }

        [Fact]
        public async Task New_ByCollectionKey()
        {
            //given
            string folderName = "NewFolderTest  " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToShortDateString();

            //when
            var folder = await _client.Folders.New(TestData.ProjectId, folderName,
                collectionKey: TestData.CollectionKey);

            //then
            folder.Id.ShouldNotEqual(null);

            //cleanup
            await _client.Folders.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Fact]
        public async Task New_WithNullIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.New(TestData.ProjectId, TestData.FolderName);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task New_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.New(TestData.ProjectId, TestData.FolderName, "abcde", "abcde");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task New_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.New(null, TestData.FolderName, TestData.CollectionId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task New_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.New("abcde", TestData.FolderName, TestData.CollectionId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task New_WithNullName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.New(TestData.ProjectId, null, TestData.CollectionId);
                throw new Exception("New should throw an exception");
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
        public async Task Get_WithNullIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Get(TestData.ProjectId);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Get_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Get(TestData.ProjectId, "abcde", "abcde");
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Get_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Get(null, TestData.CollectionId);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Get_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Get("abcde", TestData.CollectionId);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
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
        public async Task GetOne_WithNullIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.GetOne(TestData.ProjectId, TestData.FolderName);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task GetOne_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.GetOne(TestData.ProjectId, TestData.FolderName, "abcde", "abcde");
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetOne_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.GetOne(null, TestData.FolderName, TestData.CollectionId);
                throw new Exception("GetOne should throw an exception");
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
                await _client.Folders.GetOne("abcde", TestData.FolderName, TestData.CollectionId);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetOne_WithNullFolderName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.GetOne(TestData.ProjectId, null, TestData.CollectionId);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task GetOne_WithInvalidFolderName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.GetOne(TestData.ProjectId, "abcde", TestData.CollectionId);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
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

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Folders.Delete(TestData.ProjectId, newFolderName, TestData.CollectionId);
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

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Folders.Delete(TestData.ProjectId, newFolderName, collectionKey: TestData.CollectionKey);
        }

        [Fact]
        public async Task Update_WithNullIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Update(TestData.ProjectId, TestData.FolderName);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Update_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Update(TestData.ProjectId, TestData.FolderName, "abcde", "abcde");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Update_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Update(null, TestData.FolderName, TestData.CollectionId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Update_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Update("abcde", TestData.FolderName, TestData.CollectionId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Update_WithNullFolderName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Update(TestData.ProjectId, null, TestData.CollectionId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Update_WithInvalidFolderName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Update(TestData.ProjectId, "abcde", TestData.CollectionId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
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

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Folders.Delete(TestData.ProjectId, folderName, TestData.CollectionId);
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

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Folders.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Fact]
        public async Task Authorize_WithNullIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Authorize_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, "abcde", "abcde");
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Authorize_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.DeleteData, null,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Authorize_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.DeleteData, "abcde",
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Authorize_WithNullFolderName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        null, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Authorize_WithInvalidFolderName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Authorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        "abcde", TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Authorize_WithNullApiClientId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Authorize(null, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Authorize_WithInvalidApiClientId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Authorize("abcde", Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
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

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Folders.Delete(TestData.ProjectId, folderName, TestData.CollectionId);
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

            //then
            result.ShouldBeTrue();

            //cleanup
            await _client.Folders.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Fact]
        public async Task Deauthorize_WithNullIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Deauthorize_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, "abcde", "abcde");
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Deauthorize_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, null,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Deauthorize_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, "abcde",
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Deauthorize_WithNullFolderName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        null, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Deauthorize_WithInvalidFolderName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        "abcde", TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Deauthorize_WithNullUserApiKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Deauthorize(null, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
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
                await _client.Folders.Deauthorize("abcde", Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
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
        public async Task Delete_WithNullIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Delete(TestData.ProjectId, TestData.FolderName);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Delete_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Delete(TestData.ProjectId, TestData.FolderName, "abcde", "abcde");
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Delete_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Delete(null, TestData.FolderName, TestData.CollectionId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Delete_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Delete("abcde", TestData.FolderName, TestData.CollectionId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Delete_WithNullFolderName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Delete(TestData.ProjectId, null, TestData.CollectionId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Delete_WithInvalidFolderName_ThrowsException()
        {
            try
            {
                //when
                await _client.Folders.Delete(TestData.ProjectId, "abcde", TestData.CollectionId);
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
