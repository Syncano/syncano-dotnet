using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Should;
using Xunit;
using System.IO;

namespace Syncano.Net.Tests
{
    public class DataObjectRestClientNewDeleteTests : IDisposable
    {
        private Syncano _client;

        public DataObjectRestClientNewDeleteTests()
        {
            _client = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task New_ByCollectionId_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionKey_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithUserName_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.UserName = TestData.UserName;

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.User.Name.ShouldEqual(request.UserName);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithSourceUrl_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.SourceUrl = "sourceUrl";

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.SourceUrl.ShouldEqual(request.SourceUrl);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithTitle_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Title = "title";

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Title.ShouldEqual(request.Title);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithText_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Text = "text";

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Text.ShouldEqual(request.Text);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithLink_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = "link";

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Link.ShouldEqual(request.Link);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithImage_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            var imageBase64 = TestData.ImageToBase64("sampleImage.jpg");

            request.ImageBase64 = imageBase64;

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithDataOneTwoThree_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.DataOne = -1;
            request.DataTwo = 1;
            request.DataThree = 100000;

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.DataOne.ShouldEqual(request.DataOne);
            result.DataTwo.ShouldEqual(request.DataTwo);
            result.DataThree.ShouldEqual(request.DataThree);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithFolder_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var folder = await _client.Folders.New(TestData.ProjectId, "testFolder", TestData.CollectionId);
            request.Folder = folder.Name;

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Folder.ShouldEqual(folder.Name);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
            await _client.Folders.Delete(TestData.ProjectId, folder.Name, TestData.CollectionId);
        }

        [Fact]
        public async Task New_ByCollectionId_WithModeratedState_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Moderated;

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.State.ShouldEqual(request.State);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithRejectedState_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Rejected;

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.State.ShouldEqual(request.State);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithParent_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            var parentRequest = new NewDataObjectRequest();
            parentRequest.ProjectId = TestData.ProjectId;
            parentRequest.CollectionId = TestData.CollectionId;
            var parentResult = await _client.DataObjects.New(parentRequest);

            request.ParentId = parentResult.Id;

            //when
            var result = await _client.DataObjects.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);

            var parentDeleteRequest = new DeleteDataObjectRequest();
            parentDeleteRequest.ProjectId = TestData.ProjectId;
            parentDeleteRequest.CollectionId = TestData.CollectionId;
            parentDeleteRequest.DataId = parentResult.Id;
            await _client.DataObjects.Delete(parentDeleteRequest);
        }

        [Fact]
        public async Task New_WithInvalidProjectId_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = "abc";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await _client.DataObjects.New(request);
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
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await _client.DataObjects.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task New_WithInvalidCollectionId_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = "abc";

            try
            {
                //when
                await _client.DataObjects.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task New_WithNullCollectionIdAndCollectionKey_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await _client.DataObjects.New(request);
                throw new Exception("New should throw an exception");
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
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_ByCollectionKey()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;
            var dataObject = await _client.DataObjects.New(request);
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_FilterByStateModerated()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Moderated;
            var dataObject = await _client.DataObjects.New(request);
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;
            deleteRequest.State = DataObjectState.Moderated;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_FilterByStatePending()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Pending;
            var dataObject = await _client.DataObjects.New(request);
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;
            deleteRequest.State = DataObjectState.Pending;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_MultipleDataIds()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObjectOne = await _client.DataObjects.New(request);
            var dataObjectTwo = await _client.DataObjects.New(request);
            var dataObjectThree = await _client.DataObjects.New(request);

            var dataIds = new List<string>() { dataObjectTwo.Id, dataObjectThree.Id };
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObjectOne.Id;
            deleteRequest.DataIds = dataIds;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectOne.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectTwo.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectThree.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_MultipleDataIds_WithLimit()
        {
            //given
            int counter = 0;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObjectOne = await _client.DataObjects.New(request);
            var dataObjectTwo = await _client.DataObjects.New(request);
            var dataObjectThree = await _client.DataObjects.New(request);

            var dataIds = new List<string>() { dataObjectTwo.Id, dataObjectThree.Id };
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObjectOne.Id;
            deleteRequest.DataIds = dataIds;
            deleteRequest.Limit = 2;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectOne.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                if (!(e is SyncanoException))
                {
                    ++counter;
                }
            }

            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectTwo.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                if (!(e is SyncanoException))
                {
                    ++counter;
                }
            }

            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectThree.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                if (!(e is SyncanoException))
                {
                    ++counter;
                }
            }

            counter.ShouldEqual(1);
            result.ShouldBeTrue();

            //cleanup
            deleteRequest.Limit = 100;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Delete_FilterByUserName()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.UserName = TestData.UserName;
            var dataObject = await _client.DataObjects.New(request);
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.ByUser = TestData.UserName;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_FilterByTextContent()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Text = "text content";
            var dataObject = await _client.DataObjects.New(request);
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;
            deleteRequest.Filter = DataObjectContentFilter.Text;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_FilterByImageContent()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.ImageBase64 = TestData.ImageToBase64("sampleImage.jpg");
            var dataObject = await _client.DataObjects.New(request);
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;
            deleteRequest.Filter = DataObjectContentFilter.Image;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_WithNullProjectId_ThrowsException()
        {
            //given
            var request = new DeleteDataObjectRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await _client.DataObjects.Delete(request);
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
            //given
            var request = new DeleteDataObjectRequest();
            request.ProjectId = "abc";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await _client.DataObjects.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Delete_WithNullCollectionIdAndCollectionKey_ThrowsException()
        {
            //given
            var request = new DeleteDataObjectRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await _client.DataObjects.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Delete_WithNegativeLimit_ThrowsException()
        {
            //given
            var request = new DeleteDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Limit = -1;

            try
            {
                //when
                await _client.DataObjects.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task Delete_WithToBigLimit_ThrowsException()
        {
            //given
            var request = new DeleteDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Limit = DataObjectRestClient.MaxVauluesPerRequest + 1;

            try
            {
                //when
                await _client.DataObjects.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task Delete_WithToMuchIds_ThrowsException()
        {
            //given
            var request = new DeleteDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.DataIds = new List<string>();
            for (int i = 0; i < DataObjectRestClient.MaxVauluesPerRequest; ++i)
                request.DataIds.Add("abc");
            request.DataId = "abc";

            try
            {
                //when
                await _client.DataObjects.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task Delete_WithToMuchFolders_ThrowsException()
        {
            //given
            var request = new DeleteDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Folders = new List<string>();
            for (int i = 0; i < DataObjectRestClient.MaxVauluesPerRequest; ++i)
                request.Folders.Add("abc");
            request.Folder = "abc";

            try
            {
                //when
                await _client.DataObjects.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task Delete_FilterWithStateAll()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObjectOne = await _client.DataObjects.New(request);
            request.State = DataObjectState.Moderated;
            var dataObjectTwo = await _client.DataObjects.New(request);
            request.State = DataObjectState.Rejected;
            var dataObjectThree = await _client.DataObjects.New(request);

            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.State = DataObjectState.All;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectOne.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectTwo.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectThree.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_MultipleFolders()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var folderOne = await _client.Folders.New(TestData.ProjectId, "folderOne", TestData.CollectionId);
            var folderTwo = await _client.Folders.New(TestData.ProjectId, "folderTwo", TestData.CollectionId);
            var folderThree = await _client.Folders.New(TestData.ProjectId, "folderThree", TestData.CollectionId);
            request.Folder = folderOne.Name;
            var dataObjectOne = await _client.DataObjects.New(request);
            request.Folder = folderTwo.Name;
            var dataObjectTwo = await _client.DataObjects.New(request);
            request.Folder = folderThree.Name;
            var dataObjectThree = await _client.DataObjects.New(request);

            var folders = new List<string>() { folderOne.Name, folderTwo.Name };
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.Folders = folders;
            deleteRequest.Folder = folderThree.Name;

            //when
            var result = await _client.DataObjects.Delete(deleteRequest);

            //then
            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectOne.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectTwo.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                var getDataObject =
                    await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectThree.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();

            //cleanup
            await _client.Folders.Delete(TestData.ProjectId, folderOne.Name, TestData.CollectionId);
            await _client.Folders.Delete(TestData.ProjectId, folderTwo.Name, TestData.CollectionId);
            await _client.Folders.Delete(TestData.ProjectId, folderThree.Name, TestData.CollectionId);
        }

        public void Dispose()
        {
        }
    }
}
