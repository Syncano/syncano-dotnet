using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Syncano.Net.Data;
using Syncano.Net.Http;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class DataObjectSyncanoClientTests : IDisposable
    {
        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_ByCollectionId(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(newRequest);

            var getRequest = new DataObjectRichQueryRequest();
            getRequest.ProjectId = TestData.ProjectId;
            getRequest.CollectionId = TestData.CollectionId;

            //when
            var result =
                await client.Get(getRequest);

            //then
            result.ShouldNotBeNull();
            result.Any( d => d.Id == dataObject.Id).ShouldBeTrue();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_ByCollectionKey(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await client.New(newRequest);

            var getRequest = new DataObjectRichQueryRequest();
            getRequest.ProjectId = TestData.ProjectId;
            getRequest.CollectionKey = TestData.CollectionKey;

            //when
            var result =
                await client.Get(getRequest);

            //then
            result.ShouldNotBeNull();
            result.Any(d => d.Id == dataObject.Id).ShouldBeTrue();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_ByCollectionId_WithStateModerated(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionId = TestData.CollectionId;
            newRequest.State = DataObjectState.Moderated;
            var dataObject = await client.New(newRequest);

            var getRequest = new DataObjectRichQueryRequest();
            getRequest.ProjectId = TestData.ProjectId;
            getRequest.CollectionId = TestData.CollectionId;
            getRequest.State = DataObjectState.Moderated;

            //when
            var result =
                await client.Get(getRequest);

            //then
            result.ShouldNotBeNull();
            result.Any(d => d.Id == dataObject.Id).ShouldBeTrue();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_ByCollectionId_WithStateRejected(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionId = TestData.CollectionId;
            newRequest.State = DataObjectState.Rejected;
            var dataObject = await client.New(newRequest);

            var getRequest = new DataObjectRichQueryRequest();
            getRequest.ProjectId = TestData.ProjectId;
            getRequest.CollectionId = TestData.CollectionId;
            getRequest.State = DataObjectState.Rejected;

            //when
            var result =
                await client.Get(getRequest);

            //then
            result.ShouldNotBeNull();
            result.Any(d => d.Id == dataObject.Id).ShouldBeTrue();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectRichQueryRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectRichQueryRequest();
            request.ProjectId = "abc";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectRichQueryRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithNegativeLimit_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectRichQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Limit = -1;

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithToBigLimit_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectRichQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Limit = DataObjectSyncanoClient.MaxVauluesPerRequest + 1;

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithToMuchIds_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectRichQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.DataIds = new List<string>();
            for (int i = 0; i < DataObjectSyncanoClient.MaxVauluesPerRequest; ++i)
                request.DataIds.Add("abc");
            request.DataId = "abc";

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithToMuchFolders_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectRichQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Folders = new List<string>();
            for (int i = 0; i < DataObjectSyncanoClient.MaxVauluesPerRequest; ++i)
                request.Folders.Add("abc");
            request.Folder = "abc";

            try
            {
                //when
                await client.Get(request);
                throw new Exception("Get should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByCollectionId_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(newRequest);

            //when
            var result =
                await client.GetOne(TestData.ProjectId, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByCollectionKey_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await client.New(newRequest);

            //when
            var result =
                await client.GetOne(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_FilterByDataId_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await client.New(newRequest);

            //when
            var result =
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_FilterByDataKey_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            newRequest.DataKey = "testDataKey";
            var dataObject = await client.New(newRequest);

            //when
            var result =
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataKey: newRequest.DataKey);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ChildrenDefaultLimit_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await client.New(newRequest);
            newRequest.ParentId = dataObject.Id;

            for (int i = 0; i < DataObjectSyncanoClient.MaxVauluesPerRequest + 10; ++i)
                await client.New(newRequest);

            //when
            var result =
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id, includeChildren: true);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);
            result.Children.Count.ShouldEqual(DataObjectSyncanoClient.MaxVauluesPerRequest);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
            await client.Delete(deleteRequest);
        }

        [Theory(Skip = "Children limit not working over http."), PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ChildrenLimit_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await client.New(newRequest);
            newRequest.ParentId = dataObject.Id;
            var limit = 5;

            for (int i = 0; i < limit + 5; ++i)
                await client.New(newRequest);

            //when
            var result =
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id, includeChildren: true, childrenLimit: limit);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);
            result.Children.Count.ShouldEqual(limit);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ChildrenDepth_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await client.New(newRequest);
            newRequest.ParentId = dataObject.Id;
            var childObject = await client.New(newRequest);
            newRequest.ParentId = childObject.Id;
            childObject = await client.New(newRequest);
            newRequest.ParentId = childObject.Id;
            await client.New(newRequest);
            var depth = 2;

            //when
            var result =
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id, includeChildren: true, depth: depth);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);
            result.Children.Count.ShouldEqual(1);
            result.Children[0].Children.Count.ShouldEqual(1);
            result.Children[0].Children[0].Children.ShouldBeNull();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ChildrenTreeParentHierarchy_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await client.New(newRequest);
            newRequest.ParentId = dataObject.Id;
            var childObject = await client.New(newRequest);
            newRequest.ParentId = childObject.Id;
            childObject = await client.New(newRequest);
            newRequest.ParentId = childObject.Id;
            await client.New(newRequest);

            //when
            var result =
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id, includeChildren: true);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);
            result.Children.Count.ShouldEqual(1);
            result.Children[0].Children.Count.ShouldEqual(1);
            result.Children[0].Children[0].Children.Count.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory(Skip = "Children limit not working over http."), PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ChildrenTreeParentHierarchyWithLimit_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            var dataObject = await client.New(newRequest);
            newRequest.ParentId = dataObject.Id;
            var childObject = await client.New(newRequest);
            newRequest.ParentId = childObject.Id;
            childObject = await client.New(newRequest);
            newRequest.ParentId = childObject.Id;
            await client.New(newRequest);
            var limit = 2;

            //when
            var result =
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id, includeChildren: true, childrenLimit: limit);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newRequest.Folder);
            result.Id.ShouldEqual(dataObject.Id);
            result.Children.Count.ShouldEqual(1);
            result.Children[0].Children.Count.ShouldEqual(1);
            result.Children[0].Children[0].Children.ShouldBeNull();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByCollectionId_IncludeChildren_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            var parentRequest = new DataObjectDefinitionRequest();
            parentRequest.ProjectId = TestData.ProjectId;
            parentRequest.CollectionId = TestData.CollectionId;
            var parentResult = await client.New(parentRequest);

            request.ParentId = parentResult.Id;
            var childResult = await client.New(request);

            //when
            var result =
                await
                    client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentResult.Id,
                        includeChildren: true);

            //then
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(parentResult.Id);
            result.Children.ShouldNotBeEmpty();
            result.Children.Any(d => d.Id == childResult.Id).ShouldBeTrue();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = childResult.Id;
            await client.Delete(deleteRequest);

            var parentDeleteRequest = new DataObjectSimpleQueryRequest();
            parentDeleteRequest.ProjectId = TestData.ProjectId;
            parentDeleteRequest.CollectionId = TestData.CollectionId;
            parentDeleteRequest.DataId = parentResult.Id;
            await client.Delete(parentDeleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(TestData.ProjectId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(null, collectionId: TestData.CollectionId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne("abc", collectionId: TestData.CollectionId);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithNegativeLimit_ThrowsException(DataObjectSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(TestData.ProjectId, collectionId: TestData.CollectionId, childrenLimit: -1);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_WithToBigLimit_ThrowsException(DataObjectSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(TestData.ProjectId, collectionId: TestData.CollectionId, childrenLimit: DataObjectSyncanoClient.MaxVauluesPerRequest + 1);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Move_ByCollectionId_WithFolderAndState(DataObjectSyncanoClient client)
        {
            //given
            var folderClient =
                new FolderSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey));
            var folderOne = await folderClient.New(TestData.ProjectId, "folderOne", TestData.CollectionId);
            var folderTwo = await folderClient.New(TestData.ProjectId, "folderTwo", TestData.CollectionId);
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionId = TestData.CollectionId;
            newRequest.Folder = folderOne.Name;
            newRequest.State = DataObjectState.Pending;
            var dataObject = await client.New(newRequest);

            var moveRequest = new DataObjectSimpleQueryRequest();
            moveRequest.ProjectId = TestData.ProjectId;
            moveRequest.CollectionId = TestData.CollectionId;
            moveRequest.DataId = dataObject.Id;

            //when
            var result = await client.Move(moveRequest, folderTwo.Name, DataObjectState.Moderated);
            var resultObject =
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);

            //then
            result.ShouldBeTrue();
            resultObject.Folder.ShouldEqual(folderTwo.Name);

            //cleanup
            await client.Delete(moveRequest);
            await folderClient.Delete(TestData.ProjectId, folderOne.Name, TestData.CollectionId);
            await folderClient.Delete(TestData.ProjectId, folderTwo.Name, TestData.CollectionId);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Move_ByCollectionKey_WithFolderAndState(DataObjectSyncanoClient client)
        {
            //given
            var folderClient =
                new FolderSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey));
            var folderOne = await folderClient.New(TestData.ProjectId, "folderOne", TestData.CollectionId);
            var folderTwo = await folderClient.New(TestData.ProjectId, "folderTwo", TestData.CollectionId);
            var newRequest = new DataObjectDefinitionRequest();
            newRequest.ProjectId = TestData.ProjectId;
            newRequest.CollectionKey = TestData.CollectionKey;
            newRequest.Folder = folderOne.Name;
            newRequest.State = DataObjectState.Pending;
            var dataObject = await client.New(newRequest);

            var moveRequest = new DataObjectSimpleQueryRequest();
            moveRequest.ProjectId = TestData.ProjectId;
            moveRequest.CollectionKey = TestData.CollectionKey;
            moveRequest.DataId = dataObject.Id;

            //when
            var result = await client.Move(moveRequest, folderTwo.Name, DataObjectState.Moderated);
            var resultObject =
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);

            //then
            result.ShouldBeTrue();
            resultObject.Folder.ShouldEqual(folderTwo.Name);

            //cleanup
            await client.Delete(moveRequest);
            await folderClient.Delete(TestData.ProjectId, folderOne.Name, TestData.CollectionId);
            await folderClient.Delete(TestData.ProjectId, folderTwo.Name, TestData.CollectionId);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Move_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Move(request);
                throw new Exception("Move should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Move_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = "abc";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Move(request);
                throw new Exception("Move should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Move_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await client.Move(request);
                throw new Exception("Move should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Move_WithNegativeLimit_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Limit = -1;

            try
            {
                //when
                await client.Move(request);
                throw new Exception("Move should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Move_WithToBigLimit_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Limit = DataObjectSyncanoClient.MaxVauluesPerRequest + 1;

            try
            {
                //when
                await client.Move(request);
                throw new Exception("Move should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Move_WithToMuchIds_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.DataIds = new List<string>();
            for (int i = 0; i < DataObjectSyncanoClient.MaxVauluesPerRequest; ++i)
                request.DataIds.Add("abc");
            request.DataId = "abc";

            try
            {
                //when
                await client.Move(request);
                throw new Exception("Move should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Move_WithToMuchFolders_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Folders = new List<string>();
            for (int i = 0; i < DataObjectSyncanoClient.MaxVauluesPerRequest; ++i)
                request.Folders.Add("abc");
            request.Folder = "abc";

            try
            {
                //when
                await client.Move(request);
                throw new Exception("Move should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Copy_ByCollectionId_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);
            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = TestData.ProjectId;
            copyRequest.CollectionId = TestData.CollectionId;
            copyRequest.DataId = dataObject.Id;

            //when
            var result = await client.Copy(copyRequest);

            //then
            result.ShouldNotBeNull();
            result[0].Folder.ShouldEqual(request.Folder);
            result[0].Id.ShouldNotEqual(dataObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Copy_DataIdsListCountGreaterThenOne_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);
            var dataIds = new List<string>();
            dataIds.Add(dataObject.Id);
            dataObject = await client.New(request);
            dataIds.Add(dataObject.Id);
            
            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = TestData.ProjectId;
            copyRequest.CollectionId = TestData.CollectionId;
            copyRequest.DataIds = dataIds;

            //when
            var result = await client.Copy(copyRequest);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(2);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Copy_DataIdsListAndDataIdUsed_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);
            var dataIds = new List<string>();
            dataIds.Add(dataObject.Id);
            dataObject = await client.New(request);
            
            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = TestData.ProjectId;
            copyRequest.CollectionId = TestData.CollectionId;
            copyRequest.DataIds = dataIds;
            copyRequest.DataId = dataObject.Id;

            //when
            var result = await client.Copy(copyRequest);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(2);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Copy_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = null;
            copyRequest.CollectionId = TestData.CollectionId;
            copyRequest.DataId = dataObject.Id;

            try
            {
                //when
                await client.Copy(copyRequest);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Copy_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = TestData.ProjectId;
            copyRequest.DataId = dataObject.Id;

            try
            {
                //when
                await client.Copy(copyRequest);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Copy_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = "abc";
            copyRequest.CollectionId = TestData.CollectionId;
            copyRequest.DataId = dataObject.Id;

            try
            {
                //when
                await client.Copy(copyRequest);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Copy_WithoutAnyDataIds_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            await client.New(request);

            var copyRequest = new CopyDataObjectRequest();
            copyRequest.ProjectId = TestData.ProjectId;
            copyRequest.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Copy(copyRequest);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionId(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(1);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionId_MultipleDataObjects(DataObjectSyncanoClient client)
        {
            //given
            var count = 15;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            for(int i = 0; i < count; ++i)
                await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByStateModerated(DataObjectSyncanoClient client)
        {
            //given
            var count = 15;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.State = DataObjectState.Moderated;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.State = DataObjectState.Moderated;

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionKey_MultipleDataObjects(DataObjectSyncanoClient client)
        {
            //given
            var count = 15;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionKey_MultipleDataObjects_FilterByStateModerated(DataObjectSyncanoClient client)
        {
            //given
            var count = 15;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.State = DataObjectState.Moderated;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.State = DataObjectState.Moderated;

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByStateAll(DataObjectSyncanoClient client)
        {
            //given
            var count = 15;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Rejected;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.State = DataObjectState.Moderated;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.State = DataObjectState.All;

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(2*count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByImageContent(DataObjectSyncanoClient client)
        {
            //given
            var count = 15;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.UserName = TestData.UserName;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.ByUser = TestData.UserName;

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByTextContent(DataObjectSyncanoClient client)
        {
            //given
            var count = 15;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.Text = "text content";

            for (int i = 0; i < count; ++i)
                await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Filter = DataObjectContentFilter.Text;

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(2*count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByFolders(DataObjectSyncanoClient client)
        {
            //given
            var folderClient =
                new FolderSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey));
            var folderOne =
                await folderClient.New(TestData.ProjectId, "folderOne", collectionId: TestData.CollectionId);
            var folderTwo =
                await folderClient.New(TestData.ProjectId, "folderTwo", collectionId: TestData.CollectionId);
            var folderThree =
                await folderClient.New(TestData.ProjectId, "foldeThree", collectionId: TestData.CollectionId);

            var count = 15;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            request.Folder = folderOne.Name;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.Folder = folderTwo.Name;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.Folder = folderThree.Name;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Folder = folderOne.Name;

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
            await folderClient.Delete(TestData.ProjectId, folderOne.Name, TestData.CollectionId);
            await folderClient.Delete(TestData.ProjectId, folderTwo.Name, TestData.CollectionId);
            await folderClient.Delete(TestData.ProjectId, folderThree.Name, TestData.CollectionId);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByFoldersUsingFoldersList(DataObjectSyncanoClient client)
        {
            //given
            var folderClient =
                new FolderSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey));
            var folderOne =
                await folderClient.New(TestData.ProjectId, "folderOne", collectionId: TestData.CollectionId);
            var folderTwo =
                await folderClient.New(TestData.ProjectId, "folderTwo", collectionId: TestData.CollectionId);
            var folderThree =
                await folderClient.New(TestData.ProjectId, "foldeThree", collectionId: TestData.CollectionId);

            var count = 15;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            request.Folder = folderOne.Name;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.Folder = folderTwo.Name;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.Folder = folderThree.Name;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Folders = new List<string> { folderOne.Name, folderTwo.Name };

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(2*count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
            await folderClient.Delete(TestData.ProjectId, folderOne.Name, TestData.CollectionId);
            await folderClient.Delete(TestData.ProjectId, folderTwo.Name, TestData.CollectionId);
            await folderClient.Delete(TestData.ProjectId, folderThree.Name, TestData.CollectionId);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_ByCollectionId_MultipleDataObjects_FilterByFoldersUsingFolderAndFoldersList(DataObjectSyncanoClient client)
        {
            //given
            var folderClient =
                new FolderSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey));
            var folderOne =
                await folderClient.New(TestData.ProjectId, "folderOne", collectionId: TestData.CollectionId);
            var folderTwo =
                await folderClient.New(TestData.ProjectId, "folderTwo", collectionId: TestData.CollectionId);
            var folderThree =
                await folderClient.New(TestData.ProjectId, "foldeThree", collectionId: TestData.CollectionId);

            var count = 15;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            request.Folder = folderOne.Name;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.Folder = folderTwo.Name;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            request.Folder = folderThree.Name;

            for (int i = 0; i < count; ++i)
                await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Folders = new List<string> { folderOne.Name };
            countRequest.Folder = folderTwo.Name;

            //when
            var result = await client.Count(countRequest);

            //then
            result.ShouldEqual(2 * count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
            await folderClient.Delete(TestData.ProjectId, folderOne.Name, TestData.CollectionId);
            await folderClient.Delete(TestData.ProjectId, folderTwo.Name, TestData.CollectionId);
            await folderClient.Delete(TestData.ProjectId, folderThree.Name, TestData.CollectionId);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = "abc";
            countRequest.CollectionId = TestData.CollectionId;


            try
            {
                //when
                await client.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }
            
            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = null;
            countRequest.CollectionId = TestData.CollectionId;


            try
            {
                //when
                await client.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await client.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithInvalidCollectionId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = "abc";


            try
            {
                //when
                await client.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithToBigFoldersList_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Folders = new List<string>();

            for(int i = 0; i < DataObjectSyncanoClient.MaxVauluesPerRequest + 10; ++i)
                countRequest.Folders.Add("folder");

            try
            {
                //when
                await client.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentException>();
            }

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Count_WithToMuchFolders_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            await client.New(request);

            var countRequest = new CountDataObjectRequest();
            countRequest.ProjectId = TestData.ProjectId;
            countRequest.CollectionId = TestData.CollectionId;
            countRequest.Folders = new List<string>();

            for (int i = 0; i < DataObjectSyncanoClient.MaxVauluesPerRequest; ++i)
                countRequest.Folders.Add("folder");

            countRequest.Folder = "folder";

            try
            {
                //when
                await client.Count(countRequest);
                throw new Exception("Count should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentException>();
            }

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        public void Dispose()
        {
        }
    }
}
