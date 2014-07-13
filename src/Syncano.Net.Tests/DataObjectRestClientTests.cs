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

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId);

            await _client.DataObjects.Delete(TestData.ProjectId, new []{result.Id}, null, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
        }

        [Fact]
        public async Task New_ByCollectionKey_CreatesNewDataObject()
        {
            //given

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
        }

        [Fact]
        public async Task New_ByCollectionId_WithUserName_CreatesNewDataObject()
        {
            //given
            var userName = "UserName";

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId, userName: userName);

            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
            result.User.Name.ShouldEqual(userName);
        }

        [Fact]
        public async Task New_ByCollectionId_WithSourceUrl_CreatesNewDataObject()
        {
            //given
            var sourceUrl = "sourceUrl";

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId, sourceUrl: sourceUrl);

            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
            result.SourceUrl.ShouldEqual(sourceUrl);
        }

        [Fact]
        public async Task New_ByCollectionId_WithTitle_CreatesNewDataObject()
        {
            //given
            var title = "title";

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId, title: title);

            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
            result.Title.ShouldEqual(title);
        }

        [Fact]
        public async Task New_ByCollectionId_WithText_CreatesNewDataObject()
        {
            //given
            var text = "text";

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId, text: text);

            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
            result.Text.ShouldEqual(text);
        }

        [Fact]
        public async Task New_ByCollectionId_WithLink_CreatesNewDataObject()
        {
            //given
            var link = "link";

            //when
            var result = await _client.DataObjects.New(TestData.ProjectId, TestData.CollectionId, link: link);

            await _client.DataObjects.Delete(TestData.ProjectId, new[] { result.Id }, null, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
            result.Folder.ShouldEqual("Default");
            result.Link.ShouldEqual("link");
        }

        public void Dispose()
        {
        }
    }
}
