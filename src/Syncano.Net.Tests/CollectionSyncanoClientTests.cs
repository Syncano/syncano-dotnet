using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Syncano.Net.Data;
using Syncano.Net.DataRequests;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class CollectionSyncanoClientTests : IDisposable
    {
        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_CreatesNewCollectionObject(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            const string collectionDescription = "abcde";

            //when
            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey, collectionDescription);

            //then
            collection.ShouldNotBeNull();
            collection.Status.ShouldEqual(CollectionStatus.Inactive);
            collection.Key.ShouldEqual(collectionKey);
            collection.Description.ShouldEqual(collectionDescription);

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithoutKeyAndDescription_CreatesNewCollectionObject(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();

            //when
            var collection =
                await client.New(TestData.ProjectId, collectionName);

            //then
            collection.ShouldNotBeNull();
            collection.Status.ShouldEqual(CollectionStatus.Inactive);
            collection.Key.ShouldBeNull();
            collection.Description.ShouldBeNull();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.New(null, "New collection name");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithInvalidProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.New("abcde", "New collection name");
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNullCollectionName_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.New(TestData.ProjectId, null);
                throw new Exception("New should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_NoTagVersion_WithoutStatus(CollectionSyncanoClient client)
        {
            //given
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeEmpty();
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_NoTagVersion_WitActiveStatus(CollectionSyncanoClient client)
        {
            //given
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Status = CollectionStatus.Active;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeEmpty();
            result.Any(c => c.Id == TestData.CollectionId).ShouldBeTrue();
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_NoTagVersion_WithInctiveStatus(CollectionSyncanoClient client)
        {
            //given
            var collection = await client.New(TestData.ProjectId, "Get test");
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Status = CollectionStatus.Inactive;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeEmpty();
            result.Any(c => c.Id == collection.Id).ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_OneTagVersion_WithTagWithoutStatus(CollectionSyncanoClient client)
        {
            //given
            string tag = "qwert";
            var collection = await client.New(TestData.ProjectId, "Get test");
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tag = tag;
            tagRequest.CollectionId = collection.Id;
            await client.AddTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tag;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeEmpty();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_OneTagVersion_WithTagAndStatus(CollectionSyncanoClient client)
        {
            //given
            string tag = "qwert";
            var collection = await client.New(TestData.ProjectId, "Get test");
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tag = tag;
            tagRequest.CollectionId = collection.Id;
            await client.AddTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tag;
            request.Status = CollectionStatus.Inactive;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeEmpty();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_MultipleTagVersion_WithTagsWithoutStatus(CollectionSyncanoClient client)
        {
            //given
            var tags = new List<string> { "abc", "def", "ghi" };
            var collection = await client.New(TestData.ProjectId, "Get test");
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = tags;
            tagRequest.CollectionId = collection.Id;
            await client.AddTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tags = tags;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeEmpty();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_MultipleTagVersion_WithTagsAndStatus(CollectionSyncanoClient client)
        {
            //given
            var tags = new List<string> { "abc", "def", "ghi" };
            var collection = await client.New(TestData.ProjectId, "Get test");
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = tags;
            tagRequest.CollectionId = collection.Id;
            await client.AddTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tags = tags;
            request.Status = CollectionStatus.Inactive;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeEmpty();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_MultipleTagVersion_WithTagsAndTag(CollectionSyncanoClient client)
        {
            //given
            var tags = new List<string> { "abc", "def", "ghi", "jkl" };
            var collection = await client.New(TestData.ProjectId, "Get test");
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = tags;
            tagRequest.CollectionId = collection.Id;
            await client.AddTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tags = new List<string> {tags[0], tags[1], tags[2]};
            request.Tag = tags[3];
            request.Status = CollectionStatus.Inactive;

            //when
            var result = await client.Get(request);

            //then
            result.ShouldNotBeEmpty();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_MultipleTagVersion_WithEmptyTags(CollectionSyncanoClient client)
        {
            //given
            //given
            var tags = new List<string>();
            var collection = await client.New(TestData.ProjectId, "Get test");
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tags = tags;
            
            //when
            var result = await client.Get(request);
            
            //then
            result.ShouldNotBeEmpty();
            result.Any(c => c.Id == collection.Id).ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);

        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithInvalidProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new GetCollectionRequest();
            request.ProjectId = "abcde";

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

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_WithNullProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new GetCollectionRequest();
            request.ProjectId = null;

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

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByCollectionId_GetsCollectionObject(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();

            var collection =
                await client.New(TestData.ProjectId, collectionName);

            //then
            var result = await client.GetOne(TestData.ProjectId, collection.Id);

            //then
            result.ShouldNotBeNull();
            result.Status.ShouldEqual(CollectionStatus.Inactive);
            result.Key.ShouldBeNull();
            result.Description.ShouldBeNull();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_ByCollectionKey_GetsCollectionObject(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "abcde";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            //then
            var result = await client.GetOne(TestData.ProjectId, collectionKey: collectionKey);

            //then
            result.ShouldNotBeNull();
            result.Status.ShouldEqual(CollectionStatus.Active);
            result.Key.ShouldEqual(collectionKey);
            result.Description.ShouldBeNull();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_NullCollectionIdAndKey(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(TestData.ProjectId);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_IncalidCollectionId(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(TestData.ProjectId, "abcde");
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_IncalidCollectionKey(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(TestData.ProjectId, collectionKey:"abcde");
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_NullProjectId(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne(null, TestData.CollectionId);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task GetOne_InvalidProjectId(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.GetOne("abcde", TestData.CollectionId);
                throw new Exception("GetOne should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Activate_WithForce(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            var result = await client.Activate(TestData.ProjectId, collection.Id, true);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collectionKey: collection.Key);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Activate_WithoutForce(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            var result = await client.Activate(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collectionKey: collection.Key);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Activate_InvalidProjectId(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Activate("abcde", TestData.CollectionId);
                throw new Exception("Activate should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Activate_NullProjectId(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Activate(null, TestData.CollectionId);
                throw new Exception("Activate should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Activate_NullCollectionId(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Activate(TestData.ProjectId, null);
                throw new Exception("Activate should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Activate_InvalidCollectionId(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Activate(TestData.ProjectId, "abcde");
                throw new Exception("Activate should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deactivate_ByCollectionId(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            //when
            var result = await client.Deactivate(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deactivate_ByCollectionKey(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            //when
            var result = await client.Deactivate(TestData.ProjectId, collectionKey: collection.Key);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deactivate_WithNullIdAndKey_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Deactivate(TestData.ProjectId);
                throw new Exception("Deactivate should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deactivate_WithInvalidCollectionId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Deactivate(TestData.ProjectId, "abcde");
                throw new Exception("Deactivate should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deactivate_WithInvalidCollectionKey_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Deactivate(TestData.ProjectId, collectionKey: "abcde");
                throw new Exception("Deactivate should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deactivate_WithNullProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Deactivate(null, TestData.CollectionId);
                throw new Exception("Deactivate should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deactivate_WithInvalidProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Deactivate("abcde", TestData.CollectionId);
                throw new Exception("Deactivate should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionId(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string collectionDescription = "abcde";
            string newCollectionName = "New name " + collectionName;

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            collection =
                await
                    client.Update(TestData.ProjectId, collection.Id, name: newCollectionName,
                        description: collectionDescription);

            //then
            collection.ShouldNotBeNull();
            collection.Key.ShouldEqual(collectionKey);
            collection.Name.ShouldEqual(newCollectionName);
            collection.Description.ShouldEqual(collectionDescription);

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionKey(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string collectionDescription = "abcde";
            string newCollectionName = "New name " + collectionName;

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            //when
            collection =
                await
                    client.Update(TestData.ProjectId, collectionKey: collectionKey, name: newCollectionName,
                        description: collectionDescription);

            //then
            collection.ShouldNotBeNull();
            collection.Key.ShouldEqual(collectionKey);
            collection.Name.ShouldEqual(newCollectionName);
            collection.Description.ShouldEqual(collectionDescription);

            //cleanup
            await client.Delete(TestData.ProjectId, collectionKey: collection.Key);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionId_NewCollectionKey(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string collectionDescription = "abcde";
            string newCollectionName = "New name " + collectionName;
            string newKey = "newKey value";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            //when
            collection =
                await
                    client.Update(TestData.ProjectId, collection.Id, collectionKey: newKey, name: newCollectionName,
                        description: collectionDescription);

            //then
            collection.ShouldNotBeNull();
            collection.Key.ShouldEqual(newKey);
            collection.Name.ShouldEqual(newCollectionName);
            collection.Description.ShouldEqual(collectionDescription);

            //cleanup
            await client.Delete(TestData.ProjectId, collectionKey: collection.Key);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_ByCollectionId_WithoutNameAndDescription(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            collection =
                await
                    client.Update(TestData.ProjectId, collection.Id);

            //then
            collection.ShouldNotBeNull();
            collection.Key.ShouldEqual(collectionKey);

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNullIdAndKey_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(TestData.ProjectId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidCollectionId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(TestData.ProjectId, "abcde");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidCollectionKey_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(TestData.ProjectId, collectionKey: "abcde");
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithInvalidProjectIdId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Update("abcde", TestData.CollectionId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Update_WithNullProjectIdId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Update(null, TestData.CollectionId);
                throw new Exception("Update should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ByCollectionId(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();

            var collection =
                await client.New(TestData.ProjectId, collectionName);

            //when
            var result =
                await
                    client.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);
            await client.Delete(TestData.ProjectId, collection.Id);

        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_ByCollectionKey(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            //when
            var result =
                await
                    client.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collectionKey: collectionKey);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithNullIdAndKey_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Authorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        TestData.ProjectId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithInvalidCollectionId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Authorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        TestData.ProjectId, "abcde");
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithInvalidCollectionKey_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Authorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        TestData.ProjectId, collectionKey: "abcde");
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithNullProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Authorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        null, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithInvalidProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Authorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        "abcde", TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithNullUserApiClientId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Authorize(null, Permissions.DeleteOwnData,
                        TestData.ProjectId, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Authorize_WithInvalidUserApiClientId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Authorize("abcde", Permissions.DeleteOwnData,
                        TestData.ProjectId, TestData.CollectionId);
                throw new Exception("Authorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_ByCollectionId(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();

            var collection =
                await client.New(TestData.ProjectId, collectionName);

                await client.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);

            //when
            var result = await client.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_ByCollectionKey(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            await client.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collectionKey: collectionKey);

            //when
            var result = await client.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);
            
            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithNullIdAndKey_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        TestData.ProjectId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithInvalidCollectionId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        TestData.ProjectId, "abcde");
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithInvalidCollectionKey_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        TestData.ProjectId, collectionKey: "abcde");
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithNullProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        null, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithInvalidProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Deauthorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        "abcde", TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithNullUserApiClientId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Deauthorize(null, Permissions.DeleteOwnData,
                        TestData.ProjectId, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Deauthorize_WithInvalidUserApiClientId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await
                    client.Deauthorize("abcde", Permissions.DeleteOwnData,
                        TestData.ProjectId, TestData.CollectionId);
                throw new Exception("Deauthorize should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByCollectionId(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            
            var collection =
                await client.New(TestData.ProjectId, collectionName);

            //when
            var result = await client.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByCollectionKey(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            //when
            var result = await client.Delete(TestData.ProjectId, collectionKey: collection.Key);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithNullIdAndKey_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(TestData.ProjectId);
                throw  new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithInvalidCollectionId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(TestData.ProjectId, "abcde");
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithInvalidCollectionKey_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(TestData.ProjectId, collectionKey: "abcde");
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithInvalidProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete("abcde", TestData.CollectionId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_WithNullProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            try
            {
                //when
                await client.Delete(null, TestData.CollectionId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_ByCollectionId_WithoutWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            const string tag = "abcde";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tag;
            request.CollectionId = collection.Id;

            //when
            var result = await client.AddTag(request);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_ByCollectionId_WithWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            const string tag = "abcde";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tag;
            request.CollectionId = collection.Id;

            //when
            var result = await client.AddTag(request, 3.5, true);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_ByCollectionKey_WithoutWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            const string tag = "abcde";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id);

            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tag;
            request.CollectionKey = collectionKey;

            //when
            var result = await client.AddTag(request);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_ByCollectionKey_WithWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            const string tag = "abcde";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id);

            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tag;
            request.CollectionKey = collectionKey;

            //when
            var result = await client.AddTag(request, 3.5, true);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_WithNullKeyAndId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = "tag";

            try
            {
                //when
                await client.AddTag(request);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_WithInvalidCollectionId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = "tag";
            request.CollectionId = "abcde";

            try
            {
                //when
                await client.AddTag(request);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_WithInvalidCollectionKey_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = "tag";
            request.CollectionKey = "abcde";

            try
            {
                //when
                await client.AddTag(request);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_WithInvalidProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = "abcde";
            request.Tag = "tag";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.AddTag(request);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_WithNullProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = null;
            request.Tag = "tag";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.AddTag(request);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_NoTags_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.AddTag(request);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_SingleTagVersion_EmptyTag_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Tag = "";

            try
            {
                //when
                await client.AddTag(request);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_ByCollectionId_WithoutWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            var tags = new [] {"abc", "def", "ghi"};

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = collection.Id;

            //when
            var result = await client.AddTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tags[0];
            var array = await client.Get(request);
            
            //then
            result.ShouldBeTrue();
            array.ShouldNotBeEmpty();
            array.Any(c => c.Tags.Count == tags.Length).ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_ByCollectionId_WithWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            var tags = new [] { "abc", "def", "ghi" };

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = collection.Id;

            //when
            var result = await client.AddTag(tagRequest, 10.84, true);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tags[0];
            var array = await client.Get(request); 
            
            //then
            result.ShouldBeTrue();
            array.ShouldNotBeEmpty();
            array.Any(c => c.Tags.Count == tags.Length).ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_ByCollectionKey_WithoutWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            var tags = new[] { "abc", "def", "ghi" };

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionKey = collectionKey;

            //when
            var result = await client.AddTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tags[0];
            var array = await client.Get(request);         

            //then
            result.ShouldBeTrue();
            array.ShouldNotBeEmpty();
            array.Any(c => c.Tags.Count == tags.Length).ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_ByCollectionKey_WithWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            var tags = new[] { "abc", "def", "ghi" };

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionKey = collectionKey;

            //when
            var result = await client.AddTag(tagRequest, 10.84, true);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tags[0];
            var array = await client.Get(request); 
            
            //then
            result.ShouldBeTrue();
            array.ShouldNotBeEmpty();
            array.Any(c => c.Tags.Count == tags.Length).ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_WithNullKeyAndId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tags = new[] {"abc", "def", "ghi"};
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);

            try
            {
                //when
                await client.AddTag(tagRequest);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_WithInvalidCollectionId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tags = new[] { "abc", "def", "ghi" };
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = "abcde";

            try
            {
                //when
                await client.AddTag(tagRequest);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_WithInvalidCollectionKey_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tags = new[] { "abc", "def", "ghi" };
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionKey = "abcde";

            try
            {
                //when
                await client.AddTag(tagRequest);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_WithInvalidProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tags = new[] { "abc", "def", "ghi" };
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = "abcde";
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.AddTag(tagRequest);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_WithNullProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tags = new[] { "abc", "def", "ghi" };
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = null;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.AddTag(tagRequest);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_EmptyTagsArray_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tags = new List<string>();
            request.CollectionKey = TestData.CollectionKey;

            try
            {
                //when
                await
                    client.AddTag(request, 10.84, true);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_MultipleTagVersion_NoTags_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionKey = TestData.CollectionKey;

            try
            {
                //when
                await
                    client.AddTag(request, 10.84, true);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_BothTagAndTags_ByCollectionId_WithWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            var tags = new[] { "abc", "def", "ghi" };
            var tag = "jkl";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = collection.Id;
            tagRequest.Tag = tag;

            //when
            var result = await client.AddTag(tagRequest, 10.84, true);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tags = new List<string> {tags[0], tag};
            var array = await client.Get(request);

            //then
            result.ShouldBeTrue();
            array.ShouldNotBeEmpty();
            array.Any(c => c.Tags.Count == tags.Length + 1).ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddTag_RemoveOtherTest(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            var tags = new[] { "abc", "def", "ghi" };
            var tag = "jkl";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.CollectionId = collection.Id;
            tagRequest.Tag = tag;
            await client.AddTag(tagRequest);

            tagRequest.Tags = new List<string>(tags);
            tagRequest.Tag = null;

            //when
            var result = await client.AddTag(tagRequest, 10.84, true);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tags = new List<string>(tags);
            var array = await client.Get(request);

            //then
            result.ShouldBeTrue();
            array.ShouldNotBeEmpty();
            array.Any(c => c.Tags.Count == tags.Length).ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_ByCollectionId_WithoutWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            string tag = "abcde";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tag;
            request.CollectionId = collection.Id;

            await client.AddTag(request);

            //when
            var result = await client.DeleteTag(request);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_ByCollectionId_WithWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            const string tag = "abcde";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tag;
            request.CollectionId = collection.Id;

            await client.AddTag(request, 3.5, true);

            //when
            var result = await client.DeleteTag(request);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_ByCollectionKey_WithoutWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string tag = "abcde";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id);

            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tag;
            request.CollectionKey = collectionKey;

            await client.AddTag(request);

            //when
            var result = await client.DeleteTag(request);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_ByCollectionKey_WithWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string tag = "abcde";

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id);

            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tag;
            request.CollectionKey = collectionKey;

            await client.AddTag(request, 3.5, true);

            //when
            var result = await client.DeleteTag(request);

            //then
            result.ShouldBeTrue();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_WithNullKeyAndId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = "tag";

            try
            {
                //when
                await client.DeleteTag(request);

                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_WithInvalidCollectionId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = "tag";
            request.CollectionId = "abcde";

            try
            {
                //when
                await client.DeleteTag(request);

                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_WithInvalidCollectionKey_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = "tag";
            request.CollectionKey = "abcde";

            try
            {
                //when
                await client.DeleteTag(request);

                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_WithInvalidProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = "abcde";
            request.Tag = "tag";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.DeleteTag(request);

                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_WithNullProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = null;
            request.Tag = "tag";
            request.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.DeleteTag(request);

                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_WithNullTag_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Tag = null;

            try
            {
                //when
                await client.DeleteTag(request);
                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_SingleTagVersion_WithEmptyTag_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var request = new ManageCollactionTagsRequest();
            request.ProjectId = TestData.ProjectId;
            request.CollectionId = TestData.CollectionId;
            request.Tag = "";

            try
            {
                //when
                await client.DeleteTag(request);
                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_ByCollectionId_WithoutWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            var tags = new[] { "abc", "def", "ghi" };

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = collection.Id;

            await client.AddTag(tagRequest);

            //when
            var result = await client.DeleteTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tags[0];
            var array = await client.Get(request);

            //then
            result.ShouldBeTrue();
            array.ShouldBeEmpty();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_ByCollectionId_WithWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            var tags = new[] { "abc", "def", "ghi" };

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);

            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = collection.Id;

            await client.AddTag(tagRequest, 10.84, true);

            //when
            var result = await client.DeleteTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tags[0];
            var array = await client.Get(request);

            //then
            result.ShouldBeTrue();
            array.ShouldBeEmpty();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_ByCollectionKey_WithoutWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            var tags = new[] { "abc", "def", "ghi" };

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionKey = collectionKey;

            await client.AddTag(tagRequest);

            //when
            var result = await client.DeleteTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tags[0];
            var array = await client.Get(request);

            //then
            result.ShouldBeTrue();
            array.ShouldBeEmpty();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_ByCollectionKey_WithWeightAndRemoveOther(CollectionSyncanoClient client)
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            const string collectionKey = "qwert";
            var tags = new[] { "abc", "def", "ghi" };

            var collection =
                await client.New(TestData.ProjectId, collectionName, collectionKey);
            await client.Activate(TestData.ProjectId, collection.Id, true);

            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionKey = collectionKey;

            await client.AddTag(tagRequest, 10.84, true);

            //when
            var result = await client.DeleteTag(tagRequest);
            var request = new GetCollectionRequest();
            request.ProjectId = TestData.ProjectId;
            request.Tag = tags[0];
            var array = await client.Get(request);

            //then
            result.ShouldBeTrue();
            array.ShouldBeEmpty();

            //cleanup
            await client.Delete(TestData.ProjectId, collection.Id);
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_WithNullKeyAndId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tags = new[] { "abc", "def", "ghi" };
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);

            try
            {
                //when
                await client.DeleteTag(tagRequest);
                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_WithInvalidCollectionId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tags = new[] { "abc", "def", "ghi" };
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = "abcde";

            try
            {
                //when
                await client.DeleteTag(tagRequest);
                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_WithInvalidCollectionKey_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tags = new[] { "abc", "def", "ghi" };
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionKey = "abcde";

            try
            {
                //when
                await client.DeleteTag(tagRequest);
                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_WithInvalidProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tags = new[] { "abc", "def", "ghi" };
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = "abcde";
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.DeleteTag(tagRequest);
                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_WithNullProjectId_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tags = new[] { "abc", "def", "ghi" };
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = null;
            tagRequest.Tags = new List<string>(tags);
            tagRequest.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.DeleteTag(tagRequest);
                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_WithNullTags_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.CollectionId = TestData.CollectionId;

            try
            {
                //when
                await client.DeleteTag(tagRequest);
                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Theory, PropertyData("CollectionSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task DeleteTag_MultipleTagVersion_WithEmptyTags_ThrowsException(CollectionSyncanoClient client)
        {
            //given
            var tagRequest = new ManageCollactionTagsRequest();
            tagRequest.ProjectId = TestData.ProjectId;
            tagRequest.CollectionId = TestData.CollectionId;
            tagRequest.Tags = new List<string>();

            try
            {
                //when
                await client.DeleteTag(tagRequest);
                throw new Exception("DeleteTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        public void Dispose()
        {
        }
    }
}
