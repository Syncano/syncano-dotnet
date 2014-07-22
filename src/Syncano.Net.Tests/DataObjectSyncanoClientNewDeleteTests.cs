using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Syncano.Net.Data;
using Syncano.Net.Http;
using SyncanoSyncServer.Net;
using Xunit;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class DataObjectSyncanoClientNewDeleteTests : IDisposable
    {
        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionKey_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithUserName_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.UserName = TestData.UserName;

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.User.Name.ShouldEqual(request.UserName);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithSourceUrl_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.SourceUrl = "sourceUrl";

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.SourceUrl.ShouldEqual(request.SourceUrl);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithTitle_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Title = "title";

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Title.ShouldEqual(request.Title);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithText_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Text = "text";

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Text.ShouldEqual(request.Text);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithLink_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = "link";

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Link.ShouldEqual(request.Link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithImage_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            var imageBase64 = TestData.ImageToBase64("smallSampleImage.png");

            request.ImageBase64 = imageBase64;

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Fact]
        public async Task New_ByCollectionId_WithTooBigImageForTcp_ThrowsException()
        {
            //given
            var syncServer = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
            await syncServer.Start();
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            var imageBase64 = TestData.ImageToBase64("sampleImage.jpg");
            request.ImageBase64 = imageBase64;

            try
            {
                //when
                await syncServer.DataObjects.New(request);
                throw new Exception("New should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithDataOneTwoThree_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.DataOne = -1;
            request.DataTwo = 1;
            request.DataThree = 100000;

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.DataOne.ShouldEqual(request.DataOne);
            result.DataTwo.ShouldEqual(request.DataTwo);
            result.DataThree.ShouldEqual(request.DataThree);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithFolder_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var folderClient =
                new FolderSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey));
            var folder = await folderClient.New(TestData.ProjectId, "testFolder", TestData.CollectionId);
            request.Folder = folder.Name;

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Folder.ShouldEqual(folder.Name);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
            await folderClient.Delete(TestData.ProjectId, folder.Name, TestData.CollectionId);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithModeratedState_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Moderated;

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.State.ShouldEqual(request.State);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithRejectedState_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Rejected;

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.State.ShouldEqual(request.State);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithParent_CreatesNewDataObject(DataObjectSyncanoClient client)
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

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);

            var parentDeleteRequest = new DataObjectSimpleQueryRequest();
            parentDeleteRequest.ProjectId = TestData.ProjectId;
            parentDeleteRequest.CollectionId = TestData.CollectionId;
            parentDeleteRequest.DataId = parentResult.Id;
            await client.Delete(parentDeleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_ByCollectionId_WithAdditionals_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var additionals = new Dictionary<string, string>();
            additionals.Add("param1", "value1");
            additionals.Add("param2", "value2");
            additionals.Add("param3", "value3");
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Additional = additionals;

            //when
            var result = await client.New(request);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Additional.ShouldNotBeNull();
            result.Additional.Count.ShouldEqual(additionals.Count);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = "abc";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithInvalidCollectionId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = "abc";

            try
            {
                //when
                await client.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await client.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithTooMuchAdditionals_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Additional = new Dictionary<string, string>();
            for(int i = 0; i < DataObjectSyncanoClient.MaxAdditionalsCount + 5; ++i)
                request.Additional.Add(i.ToString(CultureInfo.InvariantCulture), "additional value");

            try
            {
                //when
                await client.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithTooLongAdditionalKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Additional = new Dictionary<string, string>();
            request.Additional.Add(new String('a', DataObjectSyncanoClient.MaxAdditionalKeyLenght + 1), "additional value");

            try
            {
                //when
                await client.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithTooLongAdditionalValue_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Additional = new Dictionary<string, string>();
            request.Additional.Add("additional key", new String('a', DataObjectSyncanoClient.MaxAdditionalValueLenght + 1));

            try
            {
                //when
                await client.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithTooLongTitle_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Title = new String('a', DataObjectSyncanoClient.MaxTitleLenght + 1);

            try
            {
                //when
                await client.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithTooLongText_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Text = new String('a', DataObjectSyncanoClient.MaxTextLenght + 1);

            try
            {
                //when
                await client.New(request);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByCollectionId(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByCollectionKey(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;
            var dataObject = await client.New(request);
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_FilterByStateModerated(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Moderated;
            var dataObject = await client.New(request);
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;
            deleteRequest.State = DataObjectState.Moderated;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_FilterByStatePending(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.State = DataObjectState.Pending;
            var dataObject = await client.New(request);
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;
            deleteRequest.State = DataObjectState.Pending;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_MultipleDataIds(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObjectOne = await client.New(request);
            var dataObjectTwo = await client.New(request);
            var dataObjectThree = await client.New(request);

            var dataIds = new List<string> { dataObjectTwo.Id, dataObjectThree.Id };
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObjectOne.Id;
            deleteRequest.DataIds = dataIds;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectOne.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectTwo.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectThree.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_MultipleDataIds_WithLimit(DataObjectSyncanoClient client)
        {
            //given
            int counter = 0;
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObjectOne = await client.New(request);
            var dataObjectTwo = await client.New(request);
            var dataObjectThree = await client.New(request);

            var dataIds = new List<string> { dataObjectTwo.Id, dataObjectThree.Id };
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObjectOne.Id;
            deleteRequest.DataIds = dataIds;
            deleteRequest.Limit = 2;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectOne.Id);
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
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectTwo.Id);
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
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectThree.Id);
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
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_FilterByUserName(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.UserName = TestData.UserName;
            await client.New(request);
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.ByUser = TestData.UserName;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_FilterByTextContent(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Text = "text content";
            var dataObject = await client.New(request);
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;
            deleteRequest.Filter = DataObjectContentFilter.Text;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_FilterByImageContent(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.ImageBase64 = TestData.ImageToBase64("smallSampleImage.png");
            var dataObject = await client.New(request);
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = dataObject.Id;
            deleteRequest.Filter = DataObjectContentFilter.Image;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObject.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = "abc";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await client.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithNegativeLimit_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Limit = -1;

            try
            {
                //when
                await client.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithToBigLimit_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectSimpleQueryRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Limit = DataObjectSyncanoClient.MaxVauluesPerRequest + 1;

            try
            {
                //when
                await client.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithToMuchIds_ThrowsException(DataObjectSyncanoClient client)
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
                await client.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithToMuchFolders_ThrowsException(DataObjectSyncanoClient client)
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
                await client.Delete(request);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_FilterWithStateAll(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObjectOne = await client.New(request);
            var dataObjectTwo = await client.New(request);
            request.State = DataObjectState.Moderated;
            request.State = DataObjectState.Rejected;
            var dataObjectThree = await client.New(request);

            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.State = DataObjectState.All;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectOne.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectTwo.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectThree.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_MultipleFolders(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var folderClient =
                new FolderSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey));
            var folderOne = await folderClient.New(TestData.ProjectId, "folderOne", TestData.CollectionId);
            var folderTwo = await folderClient.New(TestData.ProjectId, "folderTwo", TestData.CollectionId);
            var folderThree = await folderClient.New(TestData.ProjectId, "folderThree", TestData.CollectionId);
            request.Folder = folderOne.Name;
            var dataObjectOne = await client.New(request);
            request.Folder = folderTwo.Name;
            var dataObjectTwo = await client.New(request);
            request.Folder = folderThree.Name;
            var dataObjectThree = await client.New(request);

            var folders = new List<string> { folderOne.Name, folderTwo.Name };
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.Folders = folders;
            deleteRequest.Folder = folderThree.Name;

            //when
            var result = await client.Delete(deleteRequest);

            //then
            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectOne.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectTwo.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            try
            {
                await client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: dataObjectThree.Id);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }

            result.ShouldBeTrue();

            //cleanup
            await folderClient.Delete(TestData.ProjectId, folderOne.Name, TestData.CollectionId);
            await folderClient.Delete(TestData.ProjectId, folderTwo.Name, TestData.CollectionId);
            await folderClient.Delete(TestData.ProjectId, folderThree.Name, TestData.CollectionId);
        }

        public void Dispose()
        {
        }
    }
}
