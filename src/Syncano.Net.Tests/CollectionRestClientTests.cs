using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Xunit;

namespace Syncano.Net.Tests
{
    public class CollectionRestClientTests : IDisposable
    {
        private Syncano _client;

        public CollectionRestClientTests()
        {
            _client = new Syncano(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task New_CreatesNewCollectionObject()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string collectionDescription = "abcde";

            //when
            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey, collectionDescription);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            collection.ShouldNotBeNull();
            collection.Status.ShouldEqual(CollectionStatus.Inactive);
            collection.Key.ShouldEqual(collectionKey);
            collection.Description.ShouldEqual(collectionDescription);
        }

        [Fact]
        public async Task New_WithoutKeyAndDescription_CreatesNewCollectionObject()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();

            //when
            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            collection.ShouldNotBeNull();
            collection.Status.ShouldEqual(CollectionStatus.Inactive);
            collection.Key.ShouldBeNull();
            collection.Description.ShouldBeNull();
        }

        [Fact]
        public async Task Get_OneTagVersion_WithoutTagAndStatus()
        {
            //when
            var result = await _client.Collections.Get(TestData.ProjectId);

            //then
            result.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task Get_OneTagVersion_WithoutTag_ActiveStatus()
        {
            //when
            var result = await _client.Collections.Get(TestData.ProjectId, CollectionStatus.Active);

            //then
            result.ShouldNotBeEmpty();
            result.Any(c => c.Id == TestData.CollectionId).ShouldBeTrue();
        }

        [Fact]
        public async Task Get_OneTagVersion_WithoutTag_InctiveStatus()
        {
            //given
            var collection = await _client.Collections.New(TestData.ProjectId, "Get test");

            //when
            var result = await _client.Collections.Get(TestData.ProjectId, CollectionStatus.Inactive);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldNotBeEmpty();
            result.Any(c => c.Id == collection.Id).ShouldBeTrue();
        }

        [Fact]
        public async Task Get_OneTagVersion_WithTagWithoutStatus()
        {
            //given
            string tag = "qwert";
            var collection = await _client.Collections.New(TestData.ProjectId, "Get test");
            await _client.Collections.AddTag(TestData.ProjectId, tag, collection.Id);

            //when
            var result = await _client.Collections.Get(TestData.ProjectId, withTag: tag);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task Activate_WithForce()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            var result = await _client.Collections.Activate(TestData.ProjectId, collection.Id, true);

            await _client.Collections.Delete(TestData.ProjectId, collectionKey: collection.Key);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Activate_WithoutForce()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            var result = await _client.Collections.Activate(TestData.ProjectId, collection.Id, false);

            await _client.Collections.Delete(TestData.ProjectId, collectionKey: collection.Key);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Deactivate_ByCollectionId()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id, true);

            //when
            var result = await _client.Collections.Deactivate(TestData.ProjectId, collection.Id);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Deactivate_ByCollectionKey()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id, true);

            //when
            var result = await _client.Collections.Deactivate(TestData.ProjectId, collectionKey: collection.Key);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Deactivate_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Collections.Deactivate(TestData.ProjectId);
                throw new Exception("Deactivate should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Update_ByCollectionId()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string collectionDescription = "abcde";
            string newCollectionName = "New name " + collectionName;

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            collection =
                await
                    _client.Collections.Update(TestData.ProjectId, collection.Id, name: newCollectionName,
                        description: collectionDescription);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            collection.ShouldNotBeNull();
            collection.Key.ShouldEqual(collectionKey);
            collection.Name.ShouldEqual(newCollectionName);
            collection.Description.ShouldEqual(collectionDescription);
        }

        [Fact]
        public async Task Update_ByCollectionKey()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string collectionDescription = "abcde";
            string newCollectionName = "New name " + collectionName;

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id, true);

            //when
            collection =
                await
                    _client.Collections.Update(TestData.ProjectId, collectionKey: collectionKey, name: newCollectionName,
                        description: collectionDescription);

            await _client.Collections.Delete(TestData.ProjectId, collectionKey: collection.Key);

            //then
            collection.ShouldNotBeNull();
            collection.Key.ShouldEqual(collectionKey);
            collection.Name.ShouldEqual(newCollectionName);
            collection.Description.ShouldEqual(collectionDescription);
        }

        [Fact]
        public async Task Update_ByCollectionId_WithoutNameAndDescription()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            collection =
                await
                    _client.Collections.Update(TestData.ProjectId, collection.Id);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            collection.ShouldNotBeNull();
            collection.Key.ShouldEqual(collectionKey);
        }

        [Fact]
        public async Task Update_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Collections.Update(TestData.ProjectId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Authorize_ByCollectionId()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName);

            //when
            var result =
                await
                    _client.Collections.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);

            await _client.Collections.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);
            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Authorize_ByCollectionKey()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id, true);

            //when
            var result =
                await
                    _client.Collections.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collectionKey: collectionKey);

            await _client.Collections.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);
            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Authorize_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await
                    _client.Collections.Authorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        TestData.ProjectId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Deauthorize_ByCollectionId()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName);

                await _client.Collections.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);

            //when
            var result = await _client.Collections.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);
            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Deauthorize_ByCollectionKey()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id, true);

            await _client.Collections.Authorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collectionKey: collectionKey);

            //when
            var result = await _client.Collections.Deauthorize(TestData.UserApiClientId, Permissions.UpdateOwnData,
                        TestData.ProjectId, collection.Id);
            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Deauthorize_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await
                    _client.Collections.Deauthorize(TestData.UserApiClientId, Permissions.DeleteOwnData,
                        TestData.ProjectId);
                throw new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task Delete_ByCollectionId()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            
            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName);

            //when
            var result = await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_ByCollectionKey()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id, true);

            //when
            var result = await _client.Collections.Delete(TestData.ProjectId, collectionKey: collection.Key);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Delete_WithInvalidIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _client.Collections.Delete(TestData.ProjectId);
                throw  new Exception("Delete should throw an exception");
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task AddTag_SingleTagVersion_ByCollectionId_WithoutWeightAndRemoveOther()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string tag = "abcde";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            var result = await _client.Collections.AddTag(TestData.ProjectId, tag, collection.Id);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AddTag_SingleTagVersion_ByCollectionId_WithWeightAndRemoveOther()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string tag = "abcde";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            var result = await _client.Collections.AddTag(TestData.ProjectId, tag, collection.Id, weight: 3.5, removeOther: true);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AddTag_SingleTagVersion_ByCollectionKey_WithoutWeightAndRemoveOther()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string tag = "abcde";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id);

            //when
            var result = await _client.Collections.AddTag(TestData.ProjectId, tag, collectionKey: collectionKey);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AddTag_SingleTagVersion_ByCollectionKey_WithWeightAndRemoveOther()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string tag = "abcde";

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id);

            //when
            var result = await _client.Collections.AddTag(TestData.ProjectId, tag, collectionKey: collectionKey, weight: 3.5, removeOther: true);

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task Add_Tag_SingleTagVersion_WithInvalidKeyAndId_ThrowsException()
        {
            try
            {
                //when
                await _client.Collections.AddTag(TestData.ProjectId, "tag");
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task AddTag_MultipleTagVersion_ByCollectionId_WithoutWeightAndRemoveOther()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string[] tags = new[] {"abc", "def", "ghi"};

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            var result = await _client.Collections.AddTag(TestData.ProjectId, tags, collection.Id);

            var array = await _client.Collections.Get(TestData.ProjectId, CollectionStatus.All, withTag: tags[0]);
            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
            array.ShouldNotBeEmpty();
            array.Any(c => c.Tags.Count == tags.Length).ShouldBeTrue();
        }

        [Fact]
        public async Task AddTag_MultipleTagVersion_ByCollectionId_WithWeightAndRemoveOther()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string[] tags = new[] { "abc", "def", "ghi" };

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);

            //when
            var result = await _client.Collections.AddTag(TestData.ProjectId, tags, collection.Id, weight: 10.84, removeOther: true);

            var array = await _client.Collections.Get(TestData.ProjectId, CollectionStatus.All, withTag: tags[0]);
            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
            array.ShouldNotBeEmpty();
            array.Any(c => c.Tags.Count == tags.Length).ShouldBeTrue();
        }

        [Fact]
        public async Task AddTag_MultipleTagVersion_ByCollectionKey_WithoutWeightAndRemoveOther()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string[] tags = new[] { "abc", "def", "ghi" };

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id, true);

            //when
            var result = await _client.Collections.AddTag(TestData.ProjectId, tags, collectionKey: collectionKey);

            var array = await _client.Collections.Get(TestData.ProjectId, CollectionStatus.All, withTag: tags[0]);
            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
            array.ShouldNotBeEmpty();
            array.Any(c => c.Tags.Count == tags.Length).ShouldBeTrue();
        }

        [Fact]
        public async Task AddTag_MultipleTagVersion_ByCollectionKey_WithWeightAndRemoveOther()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string[] tags = new[] { "abc", "def", "ghi" };

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id, true);

            //when
            var result = await _client.Collections.AddTag(TestData.ProjectId, tags, collectionKey: collectionKey, weight: 10.84, removeOther: true);

            var array = await _client.Collections.Get(TestData.ProjectId, CollectionStatus.All, withTag: tags[0]);
            await _client.Collections.Delete(TestData.ProjectId, collection.Id);

            //then
            result.ShouldBeTrue();
            array.ShouldNotBeEmpty();
            array.Any(c => c.Tags.Count == tags.Length).ShouldBeTrue();
        }

        [Fact]
        public async Task Add_Tag_MultipleTagVersion_WithInvalidKeyAndId_ThrowsException()
        {
            //given
            string[] tags = new[] {"abc", "def", "ghi"};

            try
            {
                //when
                await _client.Collections.AddTag(TestData.ProjectId, tags);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task AddTag_MultipleTagVersion_ByCollectionKey_WithWeightAndRemoveOther_EmptyTagsArray_ThrowsException()
        {
            //given
            string collectionName = "NewCollection test " + DateTime.Now.ToLongTimeString() + " " +
                                    DateTime.Now.ToShortDateString();
            string collectionKey = "qwert";
            string[] tags = new string[0];

            var collection =
                await _client.Collections.New(TestData.ProjectId, collectionName, collectionKey);
            await _client.Collections.Activate(TestData.ProjectId, collection.Id, true);

            try
            {
                //when
                await
                    _client.Collections.AddTag(TestData.ProjectId, tags, collectionKey: collectionKey, weight: 10.84,
                        removeOther: true);
                throw new Exception("AddTag should throw an exception");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }

            await _client.Collections.Delete(TestData.ProjectId, collection.Id);
        }

        public void Dispose()
        {
        }
    }
}
