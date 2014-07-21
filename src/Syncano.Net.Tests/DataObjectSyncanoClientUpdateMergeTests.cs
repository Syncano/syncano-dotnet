using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Syncano.Net.Data;
using Syncano.Net.Http;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class DataObjectSyncanoClientUpdateMergeTests
    {
        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionId_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            //when
            var result = await client.Update(request, dataObject.Id);

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
        public async Task Update_ByCollectionKey_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;
            var dataObject = await client.New(request);

            //when
            var result = await client.Update(request, dataObject.Id);

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
        public async Task Update_ByCollectionId_WithUserName_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            request.UserName = TestData.UserName;

            //when
            var result = await client.Update(request, dataObject.Id);

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
        public async Task Update_ByCollectionId_WithSourceUrl_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            request.SourceUrl = "sourceUrl";

            //when
            var result = await client.Update(request, dataObject.Id);

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
        public async Task Update_ByCollectionId_WithTitle_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            request.Title = "New title";

            //when
            var result = await client.Update(request, dataObject.Id);

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
        public async Task Update_ByCollectionId_WithText_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            request.Text = "text content";

            //when
            var result = await client.Update(request, dataObject.Id);

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
        public async Task Update_ByCollectionId_WithLink_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            request.Link = "dataObject link";

            //when
            var result = await client.Update(request, dataObject.Id);

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

        [Theory(Skip = "Image too big for current tcp implementation."), PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionId_WithImage_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            request.ImageBase64 = TestData.ImageToBase64("sampleImage.jpg");

            //when
            var result = await client.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Image.ShouldNotBeNull();
            result.Image.ImageWidth.ShouldEqual(717);
            result.Image.ImageHeight.ShouldEqual(476);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory(Skip = "Image too big for current tcp implementation."), PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionId_DeleteImage_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.ImageBase64 = TestData.ImageToBase64("sampleImage.jpg");
            var dataObject = await client.New(request);

            request.ImageBase64 = null;

            //when
            var result = await client.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Image.ShouldBeNull();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionId_WithData_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            request.DataOne = 1;
            request.DataTwo = 2;
            request.DataThree = 3;

            //when
            var result = await client.Update(request, dataObject.Id);

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
        public async Task Update_ByCollectionId_WithStateModerated_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            request.State = DataObjectState.Moderated;

            //when
            var result = await client.Update(request, dataObject.Id);

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
        public async Task Update_ByCollectionId_WithParentId_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);
            var parentObject = await client.New(request);

            request.ParentId = parentObject.Id;

            //when
            var result = await client.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionId_WithFolderName_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var folderClient =
                new FolderSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey));
            var newFolder = await folderClient.New(TestData.ProjectId, "NewFolder", TestData.CollectionId);
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);
            await client.New(request);

            request.Folder = newFolder.Name;

            //when
            var result = await client.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newFolder.Name);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
            await folderClient.Delete(TestData.ProjectId, newFolder.Name, TestData.CollectionId);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionId_WithAdditionals_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var additionals = new Dictionary<string, string>();
            additionals.Add("param1", "value1");
            additionals.Add("param2", "value2");
            additionals.Add("param3", "value3");
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await client.New(request);

            request.Additional = additionals;

            //when
            var result = await client.Update(request, dataObject.Id);

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
        public async Task Update_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = "abcde";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Update(request, "dataId");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Update(request, "dataId");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidCollectionId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = "abcde";

            try
            {
                //when
                await client.Update(request, "dataId");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNullCollectionIdAndKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await client.Update(request, "dataId");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithTooMuchAdditionals_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Additional = new Dictionary<string, string>();
            for (int i = 0; i < DataObjectSyncanoClient.MaxAdditionalsCount + 5; ++i)
                request.Additional.Add(i.ToString(CultureInfo.InvariantCulture), "additional value");

            try
            {
                //when
                await client.Update(request);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithTooLongAdditionalKey_ThrowsException(DataObjectSyncanoClient client)
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
                await client.Update(request);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithTooLongAdditionalValue_ThrowsException(DataObjectSyncanoClient client)
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
                await client.Update(request);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithTooLongTitle_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Title = new String('a', DataObjectSyncanoClient.MaxTitleLenght + 1);

            try
            {
                //when
                await client.Update(request);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithTooLongText_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Text = new String('a', DataObjectSyncanoClient.MaxTextLenght + 1);

            try
            {
                //when
                await client.Update(request);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);

            request.Link = null;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_WithAdditionals_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var additionals = new Dictionary<string, string>();
            additionals.Add("param1", "value1");
            additionals.Add("param2", "value2");
            additionals.Add("param3", "value3");
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);

            request.Link = null;
            request.Additional = additionals;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Link.ShouldEqual(link);
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
        public async Task Merge_ByCollectionKey_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;
            request.Link = link;
            var dataObject = await client.New(request);

            request.Link = null;

            //when
            var result = await client.Merge(request, dataObject.Id);

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
        public async Task Merge_ByCollectionId_WithUserName_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);

            request.UserName = TestData.UserName;
            request.Link = link;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.User.Name.ShouldEqual(request.UserName);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_WithSourceUrl_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);

            request.SourceUrl = "sourceUrl";
            request.Link = link;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.SourceUrl.ShouldEqual(request.SourceUrl);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_WithTitle_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);

            request.Title = "New title";
            request.Link = null;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Title.ShouldEqual(request.Title);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_WithText_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);

            request.Text = "text content";
            request.Link = null;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Text.ShouldEqual(request.Text);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_WithLink_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var title = "title";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Title = title;
            var dataObject = await client.New(request);

            request.Link = "dataObject link";
            request.Title = null;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Link.ShouldEqual(request.Link);
            result.Title.ShouldEqual(title);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory(Skip = "Image too big for current tcp implementation."), PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_WithImage_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);

            request.ImageBase64 = TestData.ImageToBase64("sampleImage.jpg");
            request.Link = null;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Image.ShouldNotBeNull();
            result.Image.ImageWidth.ShouldEqual(717);
            result.Image.ImageHeight.ShouldEqual(476);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_WithData_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);

            request.DataOne = 1;
            request.DataTwo = 2;
            request.DataThree = 3;
            request.Link = null;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.DataOne.ShouldEqual(request.DataOne);
            result.DataTwo.ShouldEqual(request.DataTwo);
            result.DataThree.ShouldEqual(request.DataThree);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_WithStateModerated_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);

            request.State = DataObjectState.Moderated;
            request.Link = null;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.State.ShouldEqual(request.State);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_WithParentId_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);
            var parentObject = await client.New(request);

            request.ParentId = parentObject.Id;
            request.Link = null;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_ByCollectionId_WithFolderName_CreatesNewDataObject(DataObjectSyncanoClient client)
        {
            //given
            var link = "custom link";
            var folderClient =
                new FolderSyncanoClient(new SyncanoHttpClient(TestData.InstanceName, TestData.BackendAdminApiKey));
            var newFolder = await folderClient.New(TestData.ProjectId, "NewFolder", TestData.CollectionId);
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await client.New(request);

            request.Folder = newFolder.Name;
            request.Link = null;

            //when
            var result = await client.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newFolder.Name);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await client.Delete(deleteRequest);
            await folderClient.Delete(TestData.ProjectId, newFolder.Name, TestData.CollectionId);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = "abcde";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Merge(request, "dataId");
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.Merge(request, "dataId");
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_WithInvalidCollectionId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = "abcde";

            try
            {
                //when
                await client.Merge(request, "dataId");
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_WithNullCollectionIdAndKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await client.Merge(request, "dataId");
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_WithTooMuchAdditionals_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Additional = new Dictionary<string, string>();
            for (int i = 0; i < DataObjectSyncanoClient.MaxAdditionalsCount + 5; ++i)
                request.Additional.Add(i.ToString(CultureInfo.InvariantCulture), "additional value");

            try
            {
                //when
                await client.Merge(request);
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_WithTooLongAdditionalKey_ThrowsException(DataObjectSyncanoClient client)
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
                await client.Merge(request);
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_WithTooLongAdditionalValue_ThrowsException(DataObjectSyncanoClient client)
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
                await client.Merge(request);
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_WithTooLongTitle_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Title = new String('a', DataObjectSyncanoClient.MaxTitleLenght + 1);

            try
            {
                //when
                await client.Merge(request);
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Merge_WithTooLongText_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Text = new String('a', DataObjectSyncanoClient.MaxTextLenght + 1);

            try
            {
                //when
                await client.Merge(request);
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }
    }
}
