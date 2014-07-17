using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Data;
using Xunit;

namespace Syncano.Net.Tests
{
    public class DataObjectUpdateMergeTests
    {
        private readonly Syncano _client;

        public DataObjectUpdateMergeTests()
        {
            _client = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task Update_ByCollectionId_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionKey_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;
            var dataObject = await _client.DataObjects.New(request);

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithUserName_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            request.UserName = TestData.UserName;

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.User.Name.ShouldEqual(request.UserName);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithSourceUrl_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            request.SourceUrl = "sourceUrl";

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.SourceUrl.ShouldEqual(request.SourceUrl);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithTitle_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            request.Title = "New title";

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Title.ShouldEqual(request.Title);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithText_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            request.Text = "text content";

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Text.ShouldEqual(request.Text);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithLink_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            request.Link = "dataObject link";

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Link.ShouldEqual(request.Link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithImage_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            request.ImageBase64 = TestData.ImageToBase64("sampleImage.jpg");

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_DeleteImage_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.ImageBase64 = TestData.ImageToBase64("sampleImage.jpg");
            var dataObject = await _client.DataObjects.New(request);

            request.ImageBase64 = null;

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Image.ShouldBeNull();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithData_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            request.DataOne = 1;
            request.DataTwo = 2;
            request.DataThree = 3;

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithStateModerated_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            request.State = DataObjectState.Moderated;

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.State.ShouldEqual(request.State);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithParentId_CreatesNewDataObject()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);
            var parentObject = await _client.DataObjects.New(request);

            request.ParentId = parentObject.Id;

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithFolderName_CreatesNewDataObject()
        {
            //given
            var newFolder = await _client.Folders.New(TestData.ProjectId, "NewFolder", TestData.CollectionId);
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);
            await _client.DataObjects.New(request);

            request.Folder = newFolder.Name;

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newFolder.Name);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
            await _client.Folders.Delete(TestData.ProjectId, newFolder.Name, TestData.CollectionId);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithAdditionals_CreatesNewDataObject()
        {
            //given
            var additionals = new Dictionary<string, string>();
            additionals.Add("param1", "value1");
            additionals.Add("param2", "value2");
            additionals.Add("param3", "value3");
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            var dataObject = await _client.DataObjects.New(request);

            request.Additional = additionals;

            //when
            var result = await _client.DataObjects.Update(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Update_WithInvalidProjectId_ThrowsException()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = "abcde";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await _client.DataObjects.Update(request, "dataId");
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
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await _client.DataObjects.Update(request, "dataId");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Update_WithInvalidCollectionId_ThrowsException()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = "abcde";

            try
            {
                //when
                await _client.DataObjects.Update(request, "dataId");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Update_WithNullCollectionIdAndKey_ThrowsException()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await _client.DataObjects.Update(request, "dataId");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Merge_ByCollectionId_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);

            request.Link = null;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithAdditionals_CreatesNewDataObject()
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
            var dataObject = await _client.DataObjects.New(request);

            request.Link = null;
            request.Additional = additionals;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionKey_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);

            request.Link = null;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithUserName_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);

            request.UserName = TestData.UserName;
            request.Link = link;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithSourceUrl_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);

            request.SourceUrl = "sourceUrl";
            request.Link = link;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithTitle_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);

            request.Title = "New title";
            request.Link = null;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithText_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);

            request.Text = "text content";
            request.Link = null;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithLink_CreatesNewDataObject()
        {
            //given
            var title = "title";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Title = title;
            var dataObject = await _client.DataObjects.New(request);

            request.Link = "dataObject link";
            request.Title = null;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithImage_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);

            request.ImageBase64 = TestData.ImageToBase64("sampleImage.jpg");
            request.Link = null;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithData_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);

            request.DataOne = 1;
            request.DataTwo = 2;
            request.DataThree = 3;
            request.Link = null;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithStateModerated_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);

            request.State = DataObjectState.Moderated;
            request.Link = null;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithParentId_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);
            var parentObject = await _client.DataObjects.New(request);

            request.ParentId = parentObject.Id;
            request.Link = null;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(request.Folder);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task Merge_ByCollectionId_WithFolderName_CreatesNewDataObject()
        {
            //given
            var link = "custom link";
            var newFolder = await _client.Folders.New(TestData.ProjectId, "NewFolder", TestData.CollectionId);
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Link = link;
            var dataObject = await _client.DataObjects.New(request);

            request.Folder = newFolder.Name;
            request.Link = null;

            //when
            var result = await _client.DataObjects.Merge(request, dataObject.Id);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual(newFolder.Name);
            result.Link.ShouldEqual(link);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            deleteRequest.DataId = result.Id;
            await _client.DataObjects.Delete(deleteRequest);
            await _client.Folders.Delete(TestData.ProjectId, newFolder.Name, TestData.CollectionId);
        }

        [Fact]
        public async Task Merge_WithInvalidProjectId_ThrowsException()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = "abcde";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await _client.DataObjects.Merge(request, "dataId");
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Merge_WithNullProjectId_ThrowsException()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = null;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await _client.DataObjects.Merge(request, "dataId");
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Merge_WithInvalidCollectionId_ThrowsException()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = "abcde";

            try
            {
                //when
                await _client.DataObjects.Merge(request, "dataId");
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task Merge_WithNullCollectionIdAndKey_ThrowsException()
        {
            //given
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = TestData.ProjectId;

            try
            {
                //when
                await _client.DataObjects.Merge(request, "dataId");
                throw new Exception("Merge should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }
    }
}
