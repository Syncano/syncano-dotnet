using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Should;
using Xunit;

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
            request.UserName = "UserName";

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

        public void Dispose()
        {
        }
    }
}
