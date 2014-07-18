using System;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Syncano.Net.Data;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class FolderSyncanoClientTests : IDisposable
    {
        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId(FolderSyncanoClient client)
        {
            //given
            string folderName = "NewFolderTest  " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            //when
            var folder = await client.New(TestData.ProjectId, folderName,
                TestData.CollectionId);

            //then
            folder.Id.ShouldNotEqual(null);

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, TestData.CollectionId);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionKey(FolderSyncanoClient client)
        {
            //given
            string folderName = "NewFolderTest  " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToShortDateString();

            //when
            var folder = await client.New(TestData.ProjectId, folderName,
                collectionKey: TestData.CollectionKey);

            //then
            folder.Id.ShouldNotEqual(null);

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.New(TestData.ProjectId, TestData.FolderName);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithInvalidIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.New(TestData.ProjectId, TestData.FolderName, "abcde", "abcde");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.New(null, TestData.FolderName, TestData.CollectionId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithInvalidProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.New("abcde", TestData.FolderName, TestData.CollectionId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.New(TestData.ProjectId, null, TestData.CollectionId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_ByCollectionId(FolderSyncanoClient client)
        {
            //when
            var response = await client.Get(TestData.ProjectId, TestData.CollectionId);

            //then
            response.ShouldNotBeEmpty();
            response.Any(f => f.Name == TestData.FolderName).ShouldBeTrue();
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_ByCollectionKey(FolderSyncanoClient client)
        {
            //when
            var response = await client.Get(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            //then
            response.ShouldNotBeEmpty();
            response.Any(f => f.Name == TestData.FolderName).ShouldBeTrue();
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithNullIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Get(TestData.ProjectId);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithInvalidIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Get(TestData.ProjectId, "abcde", "abcde");
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithNullProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Get(null, TestData.CollectionId);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithInvalidProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Get("abcde", TestData.CollectionId);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByCollectionId(FolderSyncanoClient client)
        {
            //when
            var folder = await client.GetOne(TestData.ProjectId, TestData.FolderName, TestData.CollectionId);

            //then
            folder.Id.ShouldEqual(TestData.FolderId);
            folder.Name.ShouldEqual(TestData.FolderName);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByCollectionKey(FolderSyncanoClient client)
        {
            //when
            var folder = await client.GetOne(TestData.ProjectId, TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            folder.Id.ShouldEqual(TestData.FolderId);
            folder.Name.ShouldEqual(TestData.FolderName);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithNullIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(TestData.ProjectId, TestData.FolderName);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithInvalidIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(TestData.ProjectId, TestData.FolderName, "abcde", "abcde");
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithNullProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(null, TestData.FolderName, TestData.CollectionId);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithInvalidProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne("abcde", TestData.FolderName, TestData.CollectionId);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithNullFolderName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(TestData.ProjectId, null, TestData.CollectionId);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithInvalidFolderName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(TestData.ProjectId, "abcde", TestData.CollectionId);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionId(FolderSyncanoClient client)
        {
            //given
            string folderName = "Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            string newFolderName = "Update test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            await client.New(TestData.ProjectId, folderName, TestData.CollectionId);

            //when
            var result = await client.Update(TestData.ProjectId, folderName, TestData.CollectionId, null, newFolderName,
                        "qwerty");

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, newFolderName, TestData.CollectionId);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionKey(FolderSyncanoClient client)
        {
            //given
            string folderName = "Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            string newFolderName = "Update test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            await client.New(TestData.ProjectId, folderName, TestData.CollectionId);

            //when
            var result = await client.Update(TestData.ProjectId, folderName, null, TestData.CollectionKey, newFolderName,
                        "qwerty");

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, newFolderName, collectionKey: TestData.CollectionKey);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNullIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(TestData.ProjectId, TestData.FolderName);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(TestData.ProjectId, TestData.FolderName, "abcde", "abcde");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNullProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(null, TestData.FolderName, TestData.CollectionId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Update("abcde", TestData.FolderName, TestData.CollectionId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNullFolderName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(TestData.ProjectId, null, TestData.CollectionId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidFolderName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(TestData.ProjectId, "abcde", TestData.CollectionId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ByCollectionId(FolderSyncanoClient client)
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await client.New(TestData.ProjectId, folderName, TestData.CollectionId);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);

            await client.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, TestData.CollectionId);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ByCollectionKey_CreateDataPermission(FolderSyncanoClient client)
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await client.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            await client.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ByCollectionKey_DeleteDataPermission(FolderSyncanoClient client)
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await client.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            await client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ByCollectionKey_DeleteOwnDataPermission(FolderSyncanoClient client)
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await client.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.DeleteOwnData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            await client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteOwnData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ByCollectionKey_ReadDataPermission(FolderSyncanoClient client)
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await client.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.ReadData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            await client.Deauthorize(TestData.UserApiClientId, Permissions.ReadData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ByCollectionKey_ReadOwnDataPermission(FolderSyncanoClient client)
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await client.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.ReadOwnData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            await client.Deauthorize(TestData.UserApiClientId, Permissions.ReadOwnData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ByCollectionKey_UpdateDataPermission(FolderSyncanoClient client)
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await client.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.UpdateData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            await client.Deauthorize(TestData.UserApiClientId, Permissions.UpdateData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ByCollectionKey_UpdateOwnDataPermission(FolderSyncanoClient client)
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await client.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await client.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            await client.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithNullIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithInvalidIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, "abcde", "abcde");
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithNullProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize(TestData.UserApiClientId, Permissions.DeleteData, null,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithInvalidProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize(TestData.UserApiClientId, Permissions.DeleteData, "abcde",
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithNullFolderName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        null, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithInvalidFolderName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        "abcde", TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithNullApiClientId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize(null, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithInvalidApiClientId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Authorize("abcde", Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_ByCollectionId(FolderSyncanoClient client)
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await client.New(TestData.ProjectId, folderName, TestData.CollectionId);
            await client.Authorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);

            //when
            var result = await client.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, TestData.CollectionId);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_ByCollectionKey(FolderSyncanoClient client)
        {
            //given
            string folderName = "Authorize Test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();

            await client.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
            await client.Authorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await client.Deauthorize(TestData.UserApiClientId, Permissions.CreateData, TestData.ProjectId,
                        TestData.FolderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithNullIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithInvalidIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, "abcde", "abcde");
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithNullProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, null,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithInvalidProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, "abcde",
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithNullFolderName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        null, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithInvalidFolderName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteData, TestData.ProjectId,
                        "abcde", TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithNullUserApiKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize(null, Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithInvalidUserApiKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Deauthorize("abcde", Permissions.DeleteData, TestData.ProjectId,
                        TestData.FolderName, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByCollectionId(FolderSyncanoClient client)
        {
            //given
            string folderName = "Delete test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            await client.New(TestData.ProjectId, folderName, TestData.CollectionId);

            //when
            var result = await client.Delete(TestData.ProjectId, folderName, TestData.CollectionId);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByCollectionKey(FolderSyncanoClient client)
        {
            //given
            string folderName = "Delete test " + DateTime.Now.ToLongTimeString() + " " +
                                DateTime.Now.ToShortDateString();
            await client.New(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //when
            var result = await client.Delete(TestData.ProjectId, folderName, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithNullIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(TestData.ProjectId, TestData.FolderName);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithInvalidIdAndKey_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(TestData.ProjectId, TestData.FolderName, "abcde", "abcde");
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithNullProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(null, TestData.FolderName, TestData.CollectionId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithInvalidProjectId_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete("abcde", TestData.FolderName, TestData.CollectionId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithNullFolderName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(TestData.ProjectId, null, TestData.CollectionId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("FolderSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithInvalidFolderName_ThrowsException(FolderSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(TestData.ProjectId, "abcde", TestData.CollectionId);
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
