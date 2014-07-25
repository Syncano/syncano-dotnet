using System;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Syncano.Net.DataRequests;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class DataObjectSyncanoClientAddChildRemoveChildTests
    {
        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task AddChild_ByCollectionId(DataObjectSyncanoClient client)
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
                    client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id,
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
        public async Task AddChild_ByCollectionKey(DataObjectSyncanoClient client)
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
                    client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id,
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
        public async Task AddChild_ByCollectionKey_WithRemoveOther(DataObjectSyncanoClient client)
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
            await client.New(newOtherRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await client.New(newChildRequest);

            //when
            var result =
                await
                    client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId,
                        removeOther: true);

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
        public async Task AddChild_ByCollectionKey_WithOtherChildren(DataObjectSyncanoClient client)
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
                    client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

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
        public async Task AddChild_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
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
                    client.AddChild("abc", parentObject.Id, childObject.Id,
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
        public async Task AddChild_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
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
                    client.AddChild(null, parentObject.Id, childObject.Id,
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
        public async Task AddChild_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
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
                    client.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id);
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
        public async Task AddChild_WithNullParentId_ThrowsException(DataObjectSyncanoClient client)
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
                    client.AddChild(TestData.ProjectId, null, childObject.Id,
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
        public async Task AddChild_WithNullChildId_ThrowsException(DataObjectSyncanoClient client)
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
                    client.AddChild(TestData.ProjectId, parentObject.Id, null,
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
        public async Task AddChild_WithInvalidParentId_ThrowsException(DataObjectSyncanoClient client)
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
                    client.AddChild(TestData.ProjectId, "abc", childObject.Id,
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
        public async Task AddChild_WithInvalidChildId_ThrowsException(DataObjectSyncanoClient client)
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
                    client.AddChild(TestData.ProjectId, parentObject.Id, "abc",
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
        public async Task RemoveChild_ByCollectionId(DataObjectSyncanoClient client)
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
                    client.RemoveChild(TestData.ProjectId, parentObject.Id, childObject.Id,
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
        public async Task RemoveChild_ByCollectionKey(DataObjectSyncanoClient client)
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
                    client.RemoveChild(TestData.ProjectId, parentObject.Id, childObject.Id,
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
        public async Task RemoveChild_ByCollectionKey_WithOtherChildren(DataObjectSyncanoClient client)
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
                    client.RemoveChild(TestData.ProjectId, parentObject.Id, childObject.Id,
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
        public async Task RemoveChild_WithInvalidProjectId_ThrowsException(DataObjectSyncanoClient client)
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
                    client.RemoveChild("abc", parentObject.Id, childObject.Id, TestData.CollectionId);
                throw new Exception("RemoveChild should throw an exception");
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
        public async Task RemoveChild_WithNullProjectId_ThrowsException(DataObjectSyncanoClient client)
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
                    client.RemoveChild(null, parentObject.Id, childObject.Id, TestData.CollectionId);
                throw new Exception("RemoveChild should throw an exception");
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
        public async Task RemoveChild_WithNullCollectionIdAndCollectionKey_ThrowsException(DataObjectSyncanoClient client)
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
                    client.RemoveChild(TestData.ProjectId, parentObject.Id, childObject.Id);
                throw new Exception("RemoveChild should throw an exception");
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
        public async Task RemoveChild_WithNullParentId_ThrowsException(DataObjectSyncanoClient client)
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
                    client.RemoveChild(TestData.ProjectId, null, childObject.Id,
                        TestData.CollectionId);
                throw new Exception("RemoveChild should throw an exception");
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

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof (SyncanoClientsProvider))]
        public async Task RemoveChild_WithNullChildId_RemovesAllChildren(DataObjectSyncanoClient client)
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
            var result = await client.RemoveChild(TestData.ProjectId, parentObject.Id, collectionId: TestData.CollectionId);

            //then
            result.ShouldBeTrue();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task RemoveChild_WithInvalidParentId_ThrowsException(DataObjectSyncanoClient client)
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
                await client.RemoveChild(TestData.ProjectId, "abc", childObject.Id, TestData.CollectionId);
                throw new Exception("RemoveChild should throw an exception");
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
        public async Task RemoveChild_WithInvalidChildId_ThrowsException(DataObjectSyncanoClient client)
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
                await client.RemoveChild(TestData.ProjectId, parentObject.Id, "abc", TestData.CollectionId);
                throw new Exception("RemoveChild should throw an exception");
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
