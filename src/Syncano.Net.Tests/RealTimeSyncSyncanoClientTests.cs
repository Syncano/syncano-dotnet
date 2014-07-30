using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Should.Core.Exceptions;
using Syncano.Net.Data;
using SyncanoSyncServer.Net;
using SyncanoSyncServer.Net.RealTimeSyncApi;
using Xunit;

namespace Syncano.Net.Tests
{
    public class RealTimeSyncSyncanoClientTests
    {
        private SyncServer _syncServer;
        private Connection _currentConnection;

        public RealTimeSyncSyncanoClientTests()
        {
            if (!PrepareSyncServer().Wait(50000))
                throw new AssertException("Failed to initialize Syncano Sync Server client");
        }

        private async Task PrepareSyncServer()
        {
            _syncServer = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
            await _syncServer.Start();
            _currentConnection = (await _syncServer.RealTimeSync.GetConnections())[0];
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
        public async Task GetSubscriptions_WithSessionIdValues_ForProject()
        {
            //given
            var sessionId = await _syncServer.ApiKeys.StartSession();
            await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId, Context.Session);

            //when
            var result = await _syncServer.RealTimeSync.GetSubscriptions(sessionId: sessionId);

            //then
            result.ShouldNotBeEmpty();
            result.Any( s => s.Id == TestData.ProjectId && s.Context == Context.Session).ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);
        }

        [Fact]
        public async Task GetSubscriptions_WithConnectionUuid_ForProject()
        {
            //given
            await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId, Context.Connection);

            //when
            var result = await _syncServer.RealTimeSync.GetSubscriptions(uuid: _currentConnection.Uuid);

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);

            //then
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetSubscriptions_WithDefaultValues_ForProject()
        {
            //given
            await _syncServer.RealTimeSync.SubscribeProject(TestData.ProjectId);

            //when
            var result = await _syncServer.RealTimeSync.GetSubscriptions();

            //then
            result.ShouldNotBeEmpty();
            result.Any( s => s.Id == TestData.ProjectId).ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeProject(TestData.ProjectId);
        }

        [Fact]
        public async Task GetSubscriptions_WithDefaultValues_ForCollection()
        {
            //given
            await _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId);

            //when
            var result = await _syncServer.RealTimeSync.GetSubscriptions();

            //then
            result.ShouldNotBeEmpty();
            result.Any(s => s.Id == TestData.CollectionId).ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);
        }

        [Fact]
        public async Task GetSubscriptions_WithSessionIdValues_ForCollection()
        {
            //given
            var sessionId = await _syncServer.ApiKeys.StartSession();
            await
                _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId,
                    context: Context.Session);

            //when
            var result = await _syncServer.RealTimeSync.GetSubscriptions(sessionId: sessionId);

            //then
            result.ShouldNotBeEmpty();
            result.Any(s => s.Id == TestData.CollectionId && s.Context == Context.Session).ShouldBeTrue();

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);
        }

        [Fact]
        public async Task GetSubscriptions_WithConnectionUuid_ForCollection()
        {
            //given
            await
                _syncServer.RealTimeSync.SubscribeCollection(TestData.ProjectId, TestData.CollectionId,
                    context: Context.Connection);

            //when
            var result = await _syncServer.RealTimeSync.GetSubscriptions(uuid: _currentConnection.Uuid);

            //cleanup
            await _syncServer.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, TestData.CollectionId);

            //then
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetSubscriptions_WithInvalidApiClientId_ThrowsEcxeption()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetSubscriptions("abcde123");
                throw new Exception("GetSubscriptions should throw an excpetion.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetSubscriptions_WithInvalidConnectionUuid_ThrowsEcxeption()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetSubscriptions(uuid: "abcde123");
                throw new Exception("GetSubscriptions should throw an excpetion.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task NotificationSend_WithDefaultParameters()
        {
            //when
            var result = await _syncServer.RealTimeSync.SendNotification();

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task NotificationSend_WithApiKeyId()
        {
            //when
            var result = await _syncServer.RealTimeSync.SendNotification(apiClientId:TestData.UserApiClientId);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task NotificationSend_WithUuid()
        {
            //given
            var list = await _syncServer.RealTimeSync.GetConnections();

            //when
            var result = await _syncServer.RealTimeSync.SendNotification(uuid: list[0].Uuid);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task NotificationSend_WithAdditionals()
        {
            //given
            var additionals = new Dictionary<string, object>();
            additionals.Add("key", "value");

            //when
            var result = await _syncServer.RealTimeSync.SendNotification(additional: additionals);

            //then
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task NotificationSend_WithInvalidApiKeyId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.SendNotification(apiClientId: "abcde123");
                throw new Exception("SendNotification should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task NotificationSend_WithInvalidUuid_ThrowsExcpetion()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.SendNotification(uuid: "abcde123");
                throw new Exception("SendNotification should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetHistory_WithDefaultParameters()
        {
            //when
            var result = await _syncServer.RealTimeSync.GetHistory();

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GetHistory_WithLimit()
        {
            //when
            var result = await _syncServer.RealTimeSync.GetHistory(limit: 1);

            //then
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetHistory_WithDescendingOrder()
        {
            //when
            var result = await _syncServer.RealTimeSync.GetHistory(order: DataObjectOrder.Descending);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            (result[0].Timestamp > result[1].Timestamp).ShouldBeTrue();
        }

        [Fact]
        public async Task GetHistory_WithAscendingOrder()
        {
            //when
            var result = await _syncServer.RealTimeSync.GetHistory(order: DataObjectOrder.Ascending);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            (result[0].Timestamp < result[1].Timestamp).ShouldBeTrue();
        }

        [Fact]
        public async Task GetHistory_WithSinceId()
        {
            //given
            var list = await _syncServer.RealTimeSync.GetHistory();

            //when
            var result = await _syncServer.RealTimeSync.GetHistory(sinceId: list[0].Id);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.All( n => int.Parse(n.Id) > int.Parse(list[0].Id)).ShouldBeTrue();
        }

        [Fact]
        public async Task GetHistory_WithSinceTime()
        {
            //given
            var list = await _syncServer.RealTimeSync.GetHistory();

            //when
            var result = await _syncServer.RealTimeSync.GetHistory(sinceTime: list[0].Timestamp);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBeLessThan(list.Count);
            result.All(n => int.Parse(n.Id) > int.Parse(list[0].Id)).ShouldBeTrue();
        }

        [Fact]
        public async Task GetHistory_WithNegativeLimit()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetHistory(limit: -1);
                throw new Exception("GetHistory should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task GetHistory_WithTooBigLimit()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetHistory(limit: RealTimeSyncSyncanoClient.MaxLimit + 1);
                throw new Exception("GetHistory should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task GetHistory_WithInvalidSinceId()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetHistory("abcde123");
                throw new Exception("GetHistory should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetHistory_WithEmptySinceTime()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetHistory(sinceTime: DateTime.MinValue);
                throw new Exception("GetHistory should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetConnections_WithDefaultValues()
        {
            //when
            var result = await _syncServer.RealTimeSync.GetConnections();

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any( c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();
        }

        [Fact]
        public async Task GetConnections_WithMultipleConnections()
        {
            //given
            var count = 10;
            var servers = new SyncServer[count];
            for (int i = 0; i < count; ++i)
            {
                servers[i] = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
                await servers[i].Start();
            }

            //when
            var result = await _syncServer.RealTimeSync.GetConnections();

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();

            //cleanup
            foreach (var server in servers)
                server.Stop();
        }

        [Fact]
        public async Task GetConnections_WithLimit()
        {
            //given
            var limit = 5;
            var count = 10;
            var servers = new SyncServer[count];
            for (int i = 0; i < count; ++i)
            {
                servers[i] = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
                await servers[i].Start();
            }

            //when
            var result = await _syncServer.RealTimeSync.GetConnections(limit: limit);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();

            //cleanup
            foreach (var server in servers)
                server.Stop();
        }

        [Fact]
        public async Task GetConnections_WithSinceId()
        {
            //given
            var limit = 5;
            var count = 10;
            var list = await _syncServer.RealTimeSync.GetConnections();

            var servers = new SyncServer[count];
            for (int i = 0; i < count; ++i)
            {
                servers[i] = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
                await servers[i].Start();
            }

            //when
            var result = await _syncServer.RealTimeSync.GetConnections(sinceId: list[0].Id);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();

            //cleanup
            foreach (var server in servers)
                server.Stop();
        }

        [Fact]
        public async Task GetConnections_WithApiClientId()
        {
            //given
            var count = 10;
            var servers = new SyncServer[count];
            for (int i = 0; i < count; ++i)
            {
                servers[i] = new SyncServer(TestData.InstanceName, TestData.UserApiKey);
                await servers[i].Start();
            }

            //when
            var result = await _syncServer.RealTimeSync.GetConnections(TestData.BackendAdminApiId);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();

            //cleanup
            foreach (var server in servers)
                server.Stop();
        }

        [Fact]
        public async Task GetConnections_WithName()
        {
            //given
            var count = 10;
            var newName = "newConnectionName";
            var list = await _syncServer.RealTimeSync.GetConnections();
            await _syncServer.RealTimeSync.UpdateConnection(list[0].Uuid, newName);

            var servers = new SyncServer[count];
            for (int i = 0; i < count; ++i)
            {
                servers[i] = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
                await servers[i].Start();
            }

            //when
            var result = await _syncServer.RealTimeSync.GetConnections(name: newName);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();

            //cleanup
            foreach (var server in servers)
                server.Stop();
        }

        [Fact]
        public async Task GetConnections_ByApiClientId()
        {
            //when
            var result = await _syncServer.RealTimeSync.GetConnections(TestData.BackendAdminApiId);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();
        }

        [Fact]
        public async Task GetConnections_WithTooBigLimit_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetConnections(limit: RealTimeSyncSyncanoClient.MaxLimit + 1);
                throw new Exception("GetConnection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task GetConnections_WithTooNegativeLimit_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetConnections(limit: -1);
                throw new Exception("GetConnection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task GetConnections_WithInvalidSindeId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetConnections(sinceId: "abcde123");
                throw new Exception("GetConnection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetConnections_WithInvalidApiKeyId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetConnections("abcde123");
                throw new Exception("GetConnection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetConnections_WithInvalidName_ThrowsException()
        {
            //when
            var result = await _syncServer.RealTimeSync.GetConnections(name: "abcde123");

            //then
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public async Task GetAllConnections_WithDefaultParameters()
        {
            //when
            var result = await _syncServer.RealTimeSync.GetAllConnections();

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();
        }

        [Fact]
        public async Task GetAllConnections_WithMultipleConnections()
        {
            //given
            var count = 10;
            var servers = new SyncServer[count];
            for (int i = 0; i < count; ++i)
            {
                servers[i] = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
                await servers[i].Start();
            }

            //when
            var result = await _syncServer.RealTimeSync.GetAllConnections();

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();

            //cleanup
            foreach (var server in servers)
                server.Stop();
        }

        [Fact]
        public async Task GetAllConnections_WithLimit()
        {
            //given
            var limit = 5;
            var count = 10;
            var servers = new SyncServer[count];
            for (int i = 0; i < count; ++i)
            {
                servers[i] = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
                await servers[i].Start();
            }

            //when
            var result = await _syncServer.RealTimeSync.GetAllConnections(limit: limit);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();

            //cleanup
            foreach (var server in servers)
                server.Stop();
        }

        [Fact]
        public async Task GetAllConnections_WithSinceId()
        {
            //given
            var limit = 5;
            var count = 10;
            var list = await _syncServer.RealTimeSync.GetAllConnections();

            var servers = new SyncServer[count];
            for (int i = 0; i < count; ++i)
            {
                servers[i] = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
                await servers[i].Start();
            }

            //when
            var result = await _syncServer.RealTimeSync.GetConnections(sinceId: list[0].Id);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();

            //cleanup
            foreach (var server in servers)
                server.Stop();
        }

        [Fact]
        public async Task GetAllConnections_WithName()
        {
            //given
            var count = 10;
            var newName = "newConnectionName";
            var list = await _syncServer.RealTimeSync.GetAllConnections();
            await _syncServer.RealTimeSync.UpdateConnection(list[0].Uuid, newName);

            var servers = new SyncServer[count];
            for (int i = 0; i < count; ++i)
            {
                servers[i] = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
                await servers[i].Start();
            }

            //when
            var result = await _syncServer.RealTimeSync.GetConnections(name: newName);

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();

            //cleanup
            foreach (var server in servers)
                server.Stop();
        }

        [Fact]
        public async Task GetAllConnections_ByApiClientId()
        {
            //when
            var result = await _syncServer.RealTimeSync.GetAllConnections();

            //then
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Any(c => c.Source == Source.Tcp).ShouldBeTrue();
            result.Any(c => c.ApiClientId == TestData.BackendAdminApiId).ShouldBeTrue();
        }

        [Fact]
        public async Task GetAllConnections_WithTooBigLimit_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetAllConnections(limit: RealTimeSyncSyncanoClient.MaxLimit + 1);
                throw new Exception("GetConnection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task GetAllConnections_WithTooNegativeLimit_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetAllConnections(limit: -1);
                throw new Exception("GetConnection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<ArgumentException>();
            }
        }

        [Fact]
        public async Task GetAllConnections_WithInvalidSindeId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.GetAllConnections(sinceId: "abcde123");
                throw new Exception("GetConnection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task UpdateConnection_NewName()
        {
            //given
            var list = await _syncServer.RealTimeSync.GetConnections(TestData.BackendAdminApiId);
            var connection = list[0];
            var newName = "newConnectionName";
            

            //when
            var result = await _syncServer.RealTimeSync.UpdateConnection(connection.Uuid, newName);

            //then
            result.Name.ShouldEqual(newName);

            //cleanup
            await _syncServer.RealTimeSync.UpdateConnection(connection.Uuid, "");
        }

        [Fact]
        public async Task UpdateConnection_NewName_ByApiKeyId()
        {
            //given
            var list = await _syncServer.RealTimeSync.GetConnections(TestData.BackendAdminApiId);
            var connection = list[0];
            var newName = "newConnectionName";


            //when
            var result = await _syncServer.RealTimeSync.UpdateConnection(connection.Uuid, newName, apiClientId: TestData.BackendAdminApiId);

            //then
            result.Name.ShouldEqual(newName);

            //cleanup
            await _syncServer.RealTimeSync.UpdateConnection(connection.Uuid, "");
        }

        [Fact]
        public async Task UpdateConnection_WithInvalidUuid_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.UpdateConnection("abcde123");
                throw new Exception("UpdateConnection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task UpdateConnection_WithInvalidApiClientId_ThrowsException()
        {
            try
            {
                //when
                await _syncServer.RealTimeSync.UpdateConnection(_currentConnection.Uuid, apiClientId: "abcde123");
                throw new Exception("UpdateConnection should throw an exception.");
            }
            catch (Exception e)
            {
                //then
                e.ShouldBeType<SyncanoException>();
            }
        }
    }
}
