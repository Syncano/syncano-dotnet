﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
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

        private string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
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

        [Fact]
        public async Task New_ByCollectionId_WithImage_CreatesNewDataObject()
        {
            //given
            var request = new NewDataObjectRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            System.Drawing.Image image = System.Drawing.Image.FromFile("sampleImage.jpg");
            var imageBase64 = ImageToBase64(image, ImageFormat.Jpeg);

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
            //await _client.DataObjects.Delete(deleteRequest);
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



        public void Dispose()
        {
        }
    }
}