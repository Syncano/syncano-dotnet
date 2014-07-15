using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Xunit;
using System.IO;

namespace Syncano.Net.Tests
{
    public class DataObjectRestClientTests : IDisposable
    {
        private Syncano _client;

        public DataObjectRestClientTests()
        {
            _client = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task GetOne_ByCollectionId_CreatesNewDataObject()
        {
            //given
            var newRequest = new NewDataObjectRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(newRequest);

            //when
            var result =
                await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task GetOne_ByCollectionKey_CreatesNewDataObject()
        {
            //given
            var newRequest = new NewDataObjectRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await _client.DataObjects.New(newRequest);

            //when
            var result =
                await _client.DataObjects.GetOne(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task GetOne_FilterByDataId_CreatesNewDataObject()
        {
            //given
            var newRequest = new NewDataObjectRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await _client.DataObjects.New(newRequest);

            //when
            var result =
                await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task GetOne_FilterByDataKey_CreatesNewDataObject()
        {
            //given
            var newRequest = new NewDataObjectRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            newRequest.DataKey = "testDataKey";
            var dataObject = await _client.DataObjects.New(newRequest);

            //when
            var result =
                await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataKey: newRequest.DataKey);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task GetOne_ChildrenDefaultLimit_CreatesNewDataObject()
        {
            //given
            var newRequest = new NewDataObjectRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = dataObject.Id;

            for (int i = 0; i < DataObjectRestClient.MaxVauluesPerRequest + 10; ++i)
                await _client.DataObjects.New(newRequest);

            //when
            var result =
                await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id, includeChildren: true);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);
            result.Children.Count.ShouldEqual(DataObjectRestClient.MaxVauluesPerRequest);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact(Skip = "Syncano bug - children limit parameter not working")]
        public async Task GetOne_ChildrenLimit_CreatesNewDataObject()
        {
            //given
            var newRequest = new NewDataObjectRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = dataObject.Id;
            var limit = 20;

            for (int i = 0; i < limit + 10; ++i)
                await _client.DataObjects.New(newRequest);

            //when
            var result =
                await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id, includeChildren: true, childrenLimit: limit);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);
            result.Children.Count.ShouldEqual(limit);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task GetOne_ChildrenDepth_CreatesNewDataObject()
        {
            //given
            var newRequest = new NewDataObjectRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = dataObject.Id;
            var childObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = childObject.Id;
            childObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = childObject.Id;
            await _client.DataObjects.New(newRequest);
            var depth = 2;

            //when
            var result =
                await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id, includeChildren: true, depth: depth);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);
            result.Children.Count.ShouldEqual(1);
            result.Children[0].Children.Count.ShouldEqual(1);
            result.Children[0].Children[0].Children.ShouldBeNull();

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task GetOne_ChildrenTreeParentHierarchy_CreatesNewDataObject()
        {
            //given
            var newRequest = new NewDataObjectRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = dataObject.Id;
            var childObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = childObject.Id;
            childObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = childObject.Id;
            await _client.DataObjects.New(newRequest);

            //when
            var result =
                await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id, includeChildren: true);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);
            result.Children.Count.ShouldEqual(1);
            result.Children[0].Children.Count.ShouldEqual(1);
            result.Children[0].Children[0].Children.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task GetOne_ChildrenTreeParentHierarchyWithLimit_CreatesNewDataObject()
        {
            //given
            var newRequest = new NewDataObjectRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = dataObject.Id;
            var childObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = childObject.Id;
            childObject = await _client.DataObjects.New(newRequest);
            newRequest.ParentId = childObject.Id;
            await _client.DataObjects.New(newRequest);
            var limit = 2;

            //when
            var result =
                await _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id, includeChildren: true, childrenLimit: limit);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);
            result.Children.Count.ShouldEqual(1);
            result.Children[0].Children.Count.ShouldEqual(1);
            result.Children[0].Children[0].Children.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task GetOne_ByCollectionId_IncludeChildren_CreatesNewDataObject()
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
            var childResult = await _client.DataObjects.New(request);

            //when
            var result =
                await
                    _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentResult.Id,
                        includeChildren: true);

            //then
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(parentResult.Id);
            result.Children.ShouldNotBeEmpty();
            result.Children.Any(d => d.Id == childResult.Id).ShouldBeTrue();

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = childResult.Id;
            await _client.DataObjects.Delete(deleteRequest);

            var parentDeleteRequest = new DeleteDataObjectRequest();
            parentDeleteRequest.ProjectId = TestData.ProjectId;
            parentDeleteRequest.CollectionId = TestData.CollectionId;
            parentDeleteRequest.DataId = parentResult.Id;
            await _client.DataObjects.Delete(parentDeleteRequest);
        }

        [Fact]
        public async Task GetOne_WithNullCollectionIdAndCollectionKey_ThrowsException()
        {
            try
            {
                //when
                await _client.DataObjects.GetOne(TestData.ProjectId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task GetOne_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _client.DataObjects.GetOne(null, collectionId: TestData.CollectionId);
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
                await _client.DataObjects.GetOne("abc", collectionId: TestData.CollectionId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetOne_WithNegativeLimit_ThrowsException()
        {
            try
            {
                //when
                await _client.DataObjects.GetOne(TestData.ProjectId, collectionId: TestData.CollectionId, childrenLimit: -1);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task GetOne_WithToBigLimit_ThrowsException()
        {
            try
            {
                //when
                await _client.DataObjects.GetOne(TestData.ProjectId, collectionId: TestData.CollectionId, childrenLimit: DataObjectRestClient.MaxVauluesPerRequest + 1);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task Copy_ByCollectionId_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);
            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = TestData.ProjectId;
            copyRequest.CollectionId = TestData.CollectionId;
            copyRequest.DataId = dataObject.Id;

            //when
            var result = await _client.DataObjects.Copy(copyRequest);

            //then
            result.ShouldNotBeNull();
            result[0].Folder.ShouldEqual(request.Folder);
            result[0].Id.ShouldNotEqual(dataObject.Id);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Copy_DataIdsListCountGreaterThenOne_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);
            var dataIds = new List<string>();
            dataIds.Add(dataObject.Id);
            dataObject = await _client.DataObjects.New(request);
            dataIds.Add(dataObject.Id);
            
            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = TestData.ProjectId;
            copyRequest.CollectionId = TestData.CollectionId;
            copyRequest.DataIds = dataIds;

            //when
            var result = await _client.DataObjects.Copy(copyRequest);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(2);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Copy_DataIdsListAndDataIdUsed_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);
            var dataIds = new List<string>();
            dataIds.Add(dataObject.Id);
            dataObject = await _client.DataObjects.New(request);
            
            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = TestData.ProjectId;
            copyRequest.CollectionId = TestData.CollectionId;
            copyRequest.DataIds = dataIds;
            copyRequest.DataId = dataObject.Id;

            //when
            var result = await _client.DataObjects.Copy(copyRequest);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(2);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Copy_WithNullProjectId_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = null;
            copyRequest.CollectionId = TestData.CollectionId;
            copyRequest.DataId = dataObject.Id;

            try
            {
                //when
                await _client.DataObjects.Copy(copyRequest);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Copy_WithNullCollectionIdAndCollectionKey_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = TestData.ProjectId;
            copyRequest.DataId = dataObject.Id;

            try
            {
                //when
                await _client.DataObjects.Copy(copyRequest);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Copy_WithInvalidProjectId_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = "abc";
            copyRequest.CollectionId = TestData.CollectionId;
            copyRequest.DataId = dataObject.Id;

            try
            {
                //when
                await _client.DataObjects.Copy(copyRequest);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Copy_WithoutAnyDataIds_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = TestData.ProjectId;
            copyRequest.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await _client.DataObjects.Copy(copyRequest);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_ByCollectionId()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_ByCollectionId_MultipleDataObjects()
        {
            //given
            var count = 15;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            for(int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByStateModerated()
        {
            //given
            var count = 15;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.State = DataObjectState.Moderated;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.State = DataObjectState.Moderated;

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_ByCollectionKey_MultipleDataObjects()
        {
            //given
            var count = 15;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_ByCollectionKey_MultipleDataObjects_FilterByStateModerated()
        {
            //given
            var count = 15;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.State = DataObjectState.Moderated;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.State = DataObjectState.Moderated;

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByStateAll()
        {
            //given
            var count = 15;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Rejected;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.State = DataObjectState.Moderated;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.State = DataObjectState.All;

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(2*count);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByImageContent()
        {
            //given
            var count = 15;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.UserName = TestData.UserName;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.ByUser = TestData.UserName;

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByTextContent()
        {
            //given
            var count = 15;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.Text = "text content";

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Filter = DataObjectContentFilter.Text;

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(2*count);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByFolders()
        {
            //given
            var folderOne =
                await _client.Folders.New(TestData.ProjectId, "folderOne", collectionId: TestData.CollectionId);
            var folderTwo =
                await _client.Folders.New(TestData.ProjectId, "folderTwo", collectionId: TestData.CollectionId);
            var folderThree =
                await _client.Folders.New(TestData.ProjectId, "foldeThree", collectionId: TestData.CollectionId);

            var count = 15;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            request.Folder = folderOne.Name;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.Folder = folderTwo.Name;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.Folder = folderThree.Name;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Folder = folderOne.Name;

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
            await _client.Folders.Delete(TestData.ProjectId, folderOne.Name, TestData.CollectionId);
            await _client.Folders.Delete(TestData.ProjectId, folderTwo.Name, TestData.CollectionId);
            await _client.Folders.Delete(TestData.ProjectId, folderThree.Name, TestData.CollectionId);
        }

        [Fact]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByFoldersUsingFoldersList()
        {
            //given
            var folderOne =
                await _client.Folders.New(TestData.ProjectId, "folderOne", collectionId: TestData.CollectionId);
            var folderTwo =
                await _client.Folders.New(TestData.ProjectId, "folderTwo", collectionId: TestData.CollectionId);
            var folderThree =
                await _client.Folders.New(TestData.ProjectId, "foldeThree", collectionId: TestData.CollectionId);

            var count = 15;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            request.Folder = folderOne.Name;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.Folder = folderTwo.Name;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.Folder = folderThree.Name;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Folders = new List<string>() { folderOne.Name, folderTwo.Name };

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(2*count);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
            await _client.Folders.Delete(TestData.ProjectId, folderOne.Name, TestData.CollectionId);
            await _client.Folders.Delete(TestData.ProjectId, folderTwo.Name, TestData.CollectionId);
            await _client.Folders.Delete(TestData.ProjectId, folderThree.Name, TestData.CollectionId);
        }

        [Fact]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByFoldersUsingFolderAndFoldersList()
        {
            //given
            var folderOne =
                await _client.Folders.New(TestData.ProjectId, "folderOne", collectionId: TestData.CollectionId);
            var folderTwo =
                await _client.Folders.New(TestData.ProjectId, "folderTwo", collectionId: TestData.CollectionId);
            var folderThree =
                await _client.Folders.New(TestData.ProjectId, "foldeThree", collectionId: TestData.CollectionId);

            var count = 15;
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            request.Folder = folderOne.Name;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.Folder = folderTwo.Name;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            request.Folder = folderThree.Name;

            for (int i = 0; i < count; ++i)
                await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Folders = new List<string>() { folderOne.Name };
            countRequest.Folder = folderTwo.Name;

            //when
            var result = await _client.DataObjects.Count(countRequest);

            //then
            result.ShouldEqual(2 * count);

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
            await _client.Folders.Delete(TestData.ProjectId, folderOne.Name, TestData.CollectionId);
            await _client.Folders.Delete(TestData.ProjectId, folderTwo.Name, TestData.CollectionId);
            await _client.Folders.Delete(TestData.ProjectId, folderThree.Name, TestData.CollectionId);
        }

        [Fact]
        public async Task Count_WithInvalidProjectId_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = "abc";
            countRequest.CollectionId = TestData.CollectionId;


            try
            {
                //when
                await _client.DataObjects.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }
            
            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_WithNullProjectId_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = null;
            countRequest.CollectionId = TestData.CollectionId;


            try
            {
                //when
                await _client.DataObjects.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_WithNullCollectionIdAndCollectionKey_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await _client.DataObjects.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_WithInvalidCollectionId_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = "abc";


            try
            {
                //when
                await _client.DataObjects.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_WithToBigFoldersList_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Folders = new List<string>();

            for(int i = 0; i < DataObjectRestClient.MaxVauluesPerRequest + 10; ++i)
                countRequest.Folders.Add("folder");

            try
            {
                //when
                await _client.DataObjects.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentException>();
            }

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Count_WithToMuchFolders_ThrowsException()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Folders = new List<string>();

            for (int i = 0; i < DataObjectRestClient.MaxVauluesPerRequest; ++i)
                countRequest.Folders.Add("folder");

            countRequest.Folder = "folder";

            try
            {
                //when
                await _client.DataObjects.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentException>();
            }

            //cleanup
            var deleteRequest = new DeleteDataObjectRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        public void Dispose()
        {
        }
    }
}
