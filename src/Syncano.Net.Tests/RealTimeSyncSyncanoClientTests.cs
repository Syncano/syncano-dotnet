using System;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Should.Core.Exceptions;
using SyncanoSyncServer.Net;
using Xunit;

namespace Syncano.Net.Tests
{
    public class RealTimeSyncSyncanoClientTests
    {
        private SyncServer _syncServer;

        public RealTimeSyncSyncanoClientTests()
        {
            if (!PrepareSyncServer().Wait(50000))
                throw new AssertException("Failed to initialize Syncano Sync Server client");
        }

        private async Task PrepareSyncServer()
        {
            _syncServer = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
            await _syncServer.Start();
        }

        [Fact]
        public async Task SubscribeProject_WithConnectionContext()
        {
            //when
            var result = await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId, Context.Connection);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);
        }

        [Fact]
        public async Task SubscribeProject_WithSessionContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();

            //when
            var result = await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId, Context.Connection);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);
        }

        [Fact]
        public async Task SubscribeProject_WithDefaultContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();

            //when
            var result = await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);
        }

        [Fact]
        public async Task SubscribeProject_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.SubscribeProject(null);
                throw new Exception("SubscribeProject should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task SubscribeProject_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.SubscribeProject("abcde123");
                throw new Exception("SubscribeProject should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task UnsubscribeProject_WithConnectionContext()
        {
            //given
            await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId, Context.Connection);

            //when
            var result = await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task UnsubscribeProject_WithSessionContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();
            await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId, Context.Session);

            //when
            var result = await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task UnsubscribeProject_WithDefaultContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();
            await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId);

            //when
            var result = await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task UnsubscribeProject_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.UnsubscribeProject(null);
                throw new Exception("UnsubscribeProject should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task UnsubscribeProject_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.UnsubscribeProject("abcde123");
                throw new Exception("UnsubscribeProject should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task SubscribeCollection_ByCollectionId_WithConnectionContext()
        {
            //when
            var result = await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId, context: Context.Connection);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);
        }

        [Fact]
        public async Task SubscribeCollection_ByCollectionId_WithSessionContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();

            //when
            var result = await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId, context: Context.Connection);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);
        }

        [Fact]
        public async Task SubscribeCollection_ByCollectionId_WithDefaultContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();

            //when
            var result = await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);
        }

        [Fact]
        public async Task SubscribeCollection_ByCollectionKey_WithConnectionContext()
        {
            //when
            var result =
                await
                    _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                        collectionKey: TestData.CollectionKey, context: Context.Connection);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);
        }

        [Fact]
        public async Task SubscribeCollection_ByCollectionKey_WithSessionContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();

            //when
            var result =
                await
                    _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                        collectionKey: TestData.CollectionKey, context: Context.Connection);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);
        }

        [Fact]
        public async Task SubscribeCollection_ByCollectionKey_WithDefaultContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();

            //when
            var result = await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);
        }

        [Fact]
        public async Task SubscribeCollection_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.SubscribeCollection(null, TestData.CollectionId);
                throw new Exception("SubscribeCollection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task SubscribeCollection_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.SubscribeCollection("abcde123", TestData.CollectionId);
                throw new Exception("SubscribeCollection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task SubscribeCollection_WithInvalidCollectionId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, "abcde123");
                throw new Exception("SubscribeCollection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task SubscribeCollection_WithInvalidCollectionKey_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, collectionKey: "abcde123");
                throw new Exception("SubscribeCollection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task SubscribeCollection_WithNullCollectionIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId);
                throw new Exception("SubscribeCollection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task UnsubscribeCollection_ByCollectionId_WithConnectionContext()
        {
            //given
            await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId, context: Context.Connection);

            //when
            var result = await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task UnsubscribeCollection_ByCollectionId_WithSessionContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();
            await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId, context: Context.Session);

            //when
            var result = await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task UnsubscribeCollection_ByCollectionId_WithDefaultContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();
            await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId);

            //when
            var result = await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task UnsubscribeCollection_ByCollectionKey_WithConnectionContext()
        {
            //given
            await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId, context: Context.Connection);

            //when
            var result = await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task UnsubscribeCollection_ByCollectionKey_WithSessionContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();
            await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId, context: Context.Session);

            //when
            var result = await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task UnsubscribeCollection_ByCollectionKey_WithDefaultContext()
        {
            //given
            await _syncServer.ApiKeys.StartSession();
            await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId);

            //when
            var result = await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, collectionKey: TestData.CollectionKey);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task UnsubscribeCollection_WithNullProjectId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.UnsubscribeCollection(null);
                throw new Exception("UnsubscribeCollection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task UnsubscribeCollection_WithInvalidProjectId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.UnsubscribeCollection("abcde123", TestData.CollectionId);
                throw new Exception("UnsubscribeCollection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task UnsubscribeCollection_WithInvalidCollectionId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, "abcde123");
                throw new Exception("UnsubscribeCollection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task UnsubscribeCollection_WithInvalidCollectionKey_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, collectionKey: "abcde123");
                throw new Exception("UnsubscribeCollection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task UnsubscribeCollection_WithNullCollectionIdAndKey_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId);
                throw new Exception("UnsubscribeCollection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task GetSubscription_WithSessionIdValues()
        {
            //given
            var sessionId = await _syncServer.ApiKeys.StartSession();
            await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId, Context.Session);

            //when
            var result = await _syncServer.RealTimeSync.GetSubscription(sessionId: sessionId);

            //then
            result.ShouldNotBeEmpty();
            result.Count.ShouldEqual(1);
            result.Any( s => s.Id == TestData.ProjectId && s.Context == Context.Session).ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);
        }

        [Fact]
        public async Task GetSubscription_WithDefaultValues_ForProject()
        {
            //given
            await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId);

            //when
            var result = await _syncServer.RealTimeSync.GetSubscription();

            //then
            result.ShouldNotBeEmpty();
            result.Any( s => s.Id == TestData.ProjectId).ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);
        }

        [Fact]
        public async Task GetSubscription_WithDefaultValues_ForCollection()
        {
            //given
            await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId);

            //when
            var result = await _syncServer.RealTimeSync.GetSubscription();

            //then
            result.ShouldNotBeEmpty();
            result.Any(s => s.Id == TestData.CollectionId).ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);
        }
    }
}
