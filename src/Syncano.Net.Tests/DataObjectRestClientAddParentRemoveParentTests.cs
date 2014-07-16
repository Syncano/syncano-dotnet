using System;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Xunit;

namespace Syncano.Net.Tests
{
    public class DataObjectRestClientAddParentRemoveParentTests
    {
        private Syncano _client;

        public DataObjectRestClientAddParentRemoveParentTests()
        {
            _client = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task AddParent_ByCollectionId()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            //when
            var result =
                await
                    _client.DataObjects.AddParent(TestData.ProjectId, childObject.Id, parentObject.Id,
                        TestData.CollectionId);

            var getResult =
                await
                    _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldNotBeEmpty();
            getResult.Children[0].Id.ShouldEqual(childObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task AddParent_ByCollectionKey()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            //when
            var result =
                await
                    _client.DataObjects.AddParent(TestData.ProjectId, childObject.Id, parentObject.Id,
                        collectionKey: TestData.CollectionKey);

            var getResult =
                await
                    _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldNotBeEmpty();
            getResult.Children[0].Id.ShouldEqual(childObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task AddParent_ByCollectionKey_WithRemoveOther()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newOtherRequest = new DataObjectDefinitionRequest();
            newOtherRequest.ProjectId = TestData.ProjectId;
            newOtherRequest.CollectionId = TestData.CollectionId;
            newOtherRequest.ParentId = parentObject.Id;
            var otherObject = await _client.DataObjects.New(newOtherRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            //when
            var result =
                await
                    _client.DataObjects.AddParent(TestData.ProjectId, childObject.Id, parentObject.Id, TestData.CollectionId,
                        removeOther: true);

            var getResult =
                await
                    _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldNotBeEmpty();
            getResult.Children[0].Id.ShouldEqual(otherObject.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task AddParent_ByCollectionKey_WithOtherChildren()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newOtherRequest = new DataObjectDefinitionRequest();
            newOtherRequest.ProjectId = TestData.ProjectId;
            newOtherRequest.CollectionId = TestData.CollectionId;
            newOtherRequest.ParentId = parentObject.Id;
            var otherObject = await _client.DataObjects.New(newOtherRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            //when
            var result =
                await
                    _client.DataObjects.AddParent(TestData.ProjectId, childObject.Id, parentObject.Id, TestData.CollectionId);

            var getResult =
                await
                    _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task AddParent_WithInvalidProjectId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            try
            {
                //when
                await
                    _client.DataObjects.AddParent("abc", childObject.Id, parentObject.Id,
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task AddParent_WithNullProjectId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            try
            {
                //when
                await
                    _client.DataObjects.AddParent(null, childObject.Id, parentObject.Id,
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task AddParent_WithNullCollectionIdAndCollectionKey_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            try
            {
                //when
                await
                    _client.DataObjects.AddParent(TestData.ProjectId, childObject.Id, parentObject.Id);
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task AddParent_WithNullParentId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            try
            {
                //when
                await
                    _client.DataObjects.AddParent(TestData.ProjectId, childObject.Id, null,
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task AddParent_WithNullChildId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            try
            {
                //when
                await
                    _client.DataObjects.AddParent(TestData.ProjectId, null, parentObject.Id,
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task AddParent_WithInvalidParentId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            try
            {
                //when
                await
                    _client.DataObjects.AddChild(TestData.ProjectId, childObject.Id, "abc",
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task AddParent_WithInvalidChildId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            try
            {
                //when
                await
                    _client.DataObjects.AddParent(TestData.ProjectId, "abc", parentObject.Id,
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task RemoveParent_ByCollectionId()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            await _client.DataObjects.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            //when
            var result =
                await
                    _client.DataObjects.RemoveParent(TestData.ProjectId, childObject.Id, parentObject.Id,
                        TestData.CollectionId);

            var getResult =
                await
                    _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldBeNull();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task RemoveParent_ByCollectionId_WithoutParentId()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            await _client.DataObjects.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            //when
            var result =
                await
                    _client.DataObjects.RemoveParent(TestData.ProjectId, childObject.Id, collectionId: TestData.CollectionId);

            var getResult =
                await
                    _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldBeNull();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task RemoveParent_ByCollectionKey()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            await _client.DataObjects.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            //when
            var result =
                await
                    _client.DataObjects.RemoveParent(TestData.ProjectId, childObject.Id, parentObject.Id,
                        collectionKey: TestData.CollectionKey);

            var getResult =
                await
                    _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
                        includeChildren: true);

            //then
            result.ShouldBeTrue();
            getResult.Children.ShouldBeNull();

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task RemoveParent_ByCollectionKey_WithOtherChildren()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newOtherRequest = new DataObjectDefinitionRequest();
            newOtherRequest.ProjectId = TestData.ProjectId;
            newOtherRequest.CollectionId = TestData.CollectionId;
            newOtherRequest.ParentId = parentObject.Id;
            var otherObject = await _client.DataObjects.New(newOtherRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            await _client.DataObjects.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            //when
            var result =
                await
                    _client.DataObjects.RemoveParent(TestData.ProjectId, childObject.Id, parentObject.Id,
                        TestData.CollectionId);

            var getResult =
                await
                    _client.DataObjects.GetOne(TestData.ProjectId, TestData.CollectionId, dataId: parentObject.Id,
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task RemoveParent_WithInvalidProjectId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            await _client.DataObjects.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await
                    _client.DataObjects.RemoveParent("abc", childObject.Id, parentObject.Id, TestData.CollectionId);
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task RemoveParent_WithNullProjectId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            await _client.DataObjects.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await
                    _client.DataObjects.RemoveParent(null, childObject.Id, parentObject.Id, TestData.CollectionId);
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task RemoveParent_WithNullCollectionIdAndCollectionKey_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            await _client.DataObjects.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await
                    _client.DataObjects.RemoveParent(TestData.ProjectId, childObject.Id, parentObject.Id);
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task RemoveParent_WithNullChildId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            await _client.DataObjects.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await _client.DataObjects.RemoveParent(TestData.ProjectId, null, parentObject.Id, TestData.CollectionId);
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task RemoveParent_WithInvalidParentId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            await _client.DataObjects.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await _client.DataObjects.RemoveParent(TestData.ProjectId, childObject.Id, "abc", TestData.CollectionId);
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
            await _client.DataObjects.Delete(deleteRequest);
        }

        [Fact]
        public async Task RemoveParent_WithInvalidChildId_ThrowsException()
        {
            //given
            var newParentRequest = new DataObjectDefinitionRequest();
            newParentRequest.ProjectId = TestData.ProjectId;
            newParentRequest.CollectionId = TestData.CollectionId;
            var parentObject = await _client.DataObjects.New(newParentRequest);

            var newChildRequest = new DataObjectDefinitionRequest();
            newChildRequest.ProjectId = TestData.ProjectId;
            newChildRequest.CollectionId = TestData.CollectionId;
            var childObject = await _client.DataObjects.New(newChildRequest);

            await _client.DataObjects.AddChild(TestData.ProjectId, parentObject.Id, childObject.Id, TestData.CollectionId);

            try
            {
                //when
                await _client.DataObjects.RemoveParent(TestData.ProjectId, "abc", parentObject.Id, TestData.CollectionId);
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
            await _client.DataObjects.Delete(deleteRequest);
        }
    }
}
