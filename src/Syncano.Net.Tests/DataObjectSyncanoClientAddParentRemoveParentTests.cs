using System;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class DataObjectSyncanoClientAddParentRemoveParentTests
    {
        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddParent_ByCollectionId(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            //when
            var result =
                await
                    client.AddParent(TestData.ProjectId, childObject.Id, parentObject.Id,
                        TestData.CollectionId);

            var getResult =
                await
                    client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldNotBeEmpty();
            getResult.Children[0].Id.ShouldEqual(childObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddParent_ByCollectionKey(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            //when
            var result =
                await
                    client.AddParent(TestData.ProjectId, childObject.Id, parentObject.Id,
                        collectionKey: TestData.CollectionKey);

            var getResult =
                await
                    client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldNotBeEmpty();
            getResult.Children[0].Id.ShouldEqual(childObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddParent_ByCollectionKey_WithRemoveOther(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newOtherRequest = new DataObjectDefinitionRequest();
            newOtherRequest.ProjectId = TestData.ProjectId;
            newOtherRequest.CollectionId = TestData.CollectionId;
            newOtherRequest.ParentId = parentObject.Id;
            var otherObject = await client.New(newOtherRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            //when
            var result =
                await
                    client.AddParent(TestData.ProjectId, childObject.Id, parentObject.Id, TestData.CollectionId,
                        removeOther: true);

            var getResult =
                await
                    client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldNotBeEmpty();
            getResult.Children[0].Id.ShouldEqual(otherObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddParent_ByCollectionKey_WithOtherChildren(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newOtherRequest = new DataObjectDefinitionRequest();
            newOtherRequest.ProjectId = TestData.ProjectId;
            newOtherRequest.CollectionId = TestData.CollectionId;
            newOtherRequest.ParentId = parentObject.Id;
            var otherObject = await client.New(newOtherRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            //when
            var result =
                await
                    client.AddParent(TestData.ProjectId, childObject.Id, parentObject.Id, TestData.CollectionId);

            var getResult =
                await
                    client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldNotBeEmpty();
            getResult.Children.Count.ShouldEqual(2);
            getResult.Children.Any(d => d.Id == otherObject.Id).ShouldBeTrue();
            getResult.Children.Any(d => d.Id == childObject.Id).ShouldBeTrue();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddParent_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            try
            {
                //when
                await
                    client.AddParent("abc", childObject.Id, parentObject.Id,
                        TestData.CollectionId);
                throw new Exception("AddChild should throw an exception");
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
        public async Task AddParent_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            try
            {
                //when
                await
                    client.AddParent(null, childObject.Id, parentObject.Id,
                        TestData.CollectionId);
                throw new Exception("AddChild should throw an exception");
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
        public async Task AddParent_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            try
            {
                //when
                await
                    client.AddParent(TestData.ProjectId, childObject.Id, parentObject.Id);
                throw new Exception("AddChild should throw an exception");
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
        public async Task AddParent_WithNullParentId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            try
            {
                //when
                await
                    client.AddParent(TestData.ProjectId, childObject.Id, null,
                        TestData.CollectionId);
                throw new Exception("AddChild should throw an exception");
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
        public async Task AddParent_WithNullChildId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            await client.New(newChildRequest);

            try
            {
                //when
                await
                    client.AddParent(TestData.ProjectId, null, parentObject.Id,
                        TestData.CollectionId);
                throw new Exception("AddChild should throw an exception");
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
        public async Task AddParent_WithInvalidParentId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            try
            {
                //when
                await
                    client.AddChild(TestData.ProjectId, childObject.Id, "abc",
                        TestData.CollectionId);
                throw new Exception("AddChild should throw an exception");
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
        public async Task AddParent_WithInvalidChildId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            await client.New(newChildRequest);

            try
            {
                //when
                await
                    client.AddParent(TestData.ProjectId, "abc", parentObject.Id,
                        TestData.CollectionId);
                throw new Exception("AddChild should throw an exception");
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
        public async Task RemoveParent_ByCollectionId(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            await client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            //when
            var result =
                await
                    client.RemoveParent(TestData.ProjectId, childObject.Id, parentObject.Id,
                        TestData.CollectionId);

            var getResult =
                await
                    client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldBeNull();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task RemoveParent_ByCollectionId_WithoutParentId(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            await client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            //when
            var result =
                await
                    client.RemoveParent(TestData.ProjectId, childObject.Id, collectionId: TestData.CollectionId);

            var getResult =
                await
                    client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldBeNull();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task RemoveParent_ByCollectionKey(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            await client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            //when
            var result =
                await
                    client.RemoveParent(TestData.ProjectId, childObject.Id, parentObject.Id,
                        collectionKey: TestData.CollectionKey);

            var getResult =
                await
                    client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldBeNull();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task RemoveParent_ByCollectionKey_WithOtherChildren(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newOtherRequest = new DataObjectDefinitionRequest();
            newOtherRequest.ProjectId = TestData.ProjectId;
            newOtherRequest.CollectionId = TestData.CollectionId;
            newOtherRequest.ParentId = parentObject.Id;
            var otherObject = await client.New(newOtherRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            await client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            //when
            var result =
                await
                    client.RemoveParent(TestData.ProjectId, childObject.Id, parentObject.Id,
                        TestData.CollectionId);

            var getResult =
                await
                    client.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldNotBeEmpty();
            getResult.Children.Count.ShouldEqual(1);
            getResult.Children.Any(d => d.Id == otherObject.Id).ShouldBeTrue();
            getResult.Children.Any(d => d.Id == childObject.Id).ShouldBeFalse();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task RemoveParent_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            await client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await
                    client.RemoveParent("abc", childObject.Id, parentObject.Id, TestData.CollectionId);
                throw new Exception("RemoveParent should throw an exception");
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
        public async Task RemoveParent_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            await client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await
                    client.RemoveParent(null, childObject.Id, parentObject.Id, TestData.CollectionId);
                throw new Exception("RemoveParent should throw an exception");
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
        public async Task RemoveParent_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            await client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await
                    client.RemoveParent(TestData.ProjectId, childObject.Id, parentObject.Id);
                throw new Exception("RemoveParent should throw an exception");
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
        public async Task RemoveParent_WithNullChildId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            await client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await client.RemoveParent(TestData.ProjectId, null, parentObject.Id, TestData.CollectionId);
                throw new Exception("RemoveParent should throw an exception");
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
        public async Task RemoveParent_WithInvalidParentId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            await client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await client.RemoveParent(TestData.ProjectId, childObject.Id, "abc", TestData.CollectionId);
                throw new Exception("RemoveParent should throw an exception");
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
        public async Task RemoveParent_WithInvalidChildId_ThrowsException(DataObjectSyncanoClient client)
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await client.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            await client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await client.RemoveParent(TestData.ProjectId, "abc", parentObject.Id, TestData.CollectionId);
                throw new Exception("RemoveParent should throw an exception");
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
    }
}
