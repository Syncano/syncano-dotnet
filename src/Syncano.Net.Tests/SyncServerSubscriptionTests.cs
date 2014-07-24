using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Should;
using Should.Core.Exceptions;
using Syncano.Net.DataRequests;
using SyncanoSyncServer.Net;
using SyncanoSyncServer.Net.Notifications;
using Xunit;

namespace Syncano.Net.Tests
{
    public class SyncServerSubscriptionTests
    {

        private SyncServer _syncSubscriber;

        private SyncServer _syncServer;

        public SyncServerSubscriptionTests()
        {
            if (!PrepareSyncServer().Wait(50000))
                throw new AssertException("Failed to initialize Syncano Sync Server client");
        }

        private async Task PrepareSyncServer()
        {
            _syncSubscriber = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
            await _syncSubscriber.Start();


            _syncServer = new SyncServer(TestData.InstanceName, TestData.BackendAdminApiKey);
            await _syncServer.Start();
        }

        private static async Task WaitForNotifications()
        {
            await Task.Delay(1000);
        }

        [Fact]
        public async Task NewDataNotification_IsRecievedWhenNewData()
        {
            //given
            NewDataNotification newDataNotification = null;
            await _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId, collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.NewDataObservable.Subscribe(m => newDataNotification = m);

            //when
            var newData = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest() {ProjectId =  TestData.ProjectId, 
                CollectionId = TestData.SubscriptionCollectionId, 
                Title = "test"});
            await WaitForNotifications();

            //then
            newDataNotification.ShouldNotBeNull();
            newDataNotification.Data.Title.ShouldEqual(newData.Title);
            newDataNotification.Data.Id.ShouldEqual(newData.Id);

            //cleanup
            await _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId, collectionId: TestData.SubscriptionCollectionId);

        }

       
    }
}