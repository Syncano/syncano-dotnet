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
            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");

            //cleanup
            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);
        }

        [Fact]
        public async Task New_ByCollectionKey_CreatesNewDataObject()
        {
            //given

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");

            //cleanup
            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);
        }

        [Fact]
        public async Task New_ByCollectionId_WithUserName_CreatesNewDataObject()
        {
            //given
            var userName = "UserName";

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId, userName: userName);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
            result.User.Name.ShouldEqual(userName);

            //cleanup
            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);
        }

        [Fact]
        public async Task New_ByCollectionId_WithSourceUrl_CreatesNewDataObject()
        {
            //given
            var sourceUrl = "sourceUrl";

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId, sourceUrl: sourceUrl);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
            result.SourceUrl.ShouldEqual(sourceUrl);

            //cleanup
            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);
        }

        [Fact]
        public async Task New_ByCollectionId_WithTitle_CreatesNewDataObject()
        {
            //given
            var title = "title";

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId, title: title);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
            result.Title.ShouldEqual(title);

            //cleanup
            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);
        }

        [Fact]
        public async Task New_ByCollectionId_WithText_CreatesNewDataObject()
        {
            //given
            var text = "text";

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId, text: text);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
            result.Text.ShouldEqual(text);

            //cleanup
            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);
        }

        [Fact]
        public async Task New_ByCollectionId_WithLink_CreatesNewDataObject()
        {
            //given
            var link = "link";

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId, link: link);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
            result.Link.ShouldEqual("link");

            //cleanup
            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);
        }

        public void Dispose()
        {
        }
    }
}
