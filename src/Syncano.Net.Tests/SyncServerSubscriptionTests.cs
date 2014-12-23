using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Should.Core.Exceptions;
using Syncano.Net.DataRequests;
using SyncanoSyncServer.Net;
using SyncanoSyncServer.Net.Notifications;
using Xunit;

namespace Syncano.Net.Tests
{
    public class SyncServerSubscriptionTests:IDisposable
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

        public void Dispose()
        {
            if(_syncServer != null)
                _syncServer.Stop();


            if (_syncSubscriber != null)
                _syncSubscriber.Stop();

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
            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.NewDataObservable.Subscribe(m => newDataNotification = m);

            //when
            var newData = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "test"
            });
            await WaitForNotifications();

            //then
            newDataNotification.ShouldNotBeNull();
            newDataNotification.Data.Title.ShouldEqual(newData.Title);
            newDataNotification.Data.Id.ShouldEqual(newData.Id);
            newDataNotification.Type.ShouldEqual(NotificationType.New);
            newDataNotification.Object.ShouldEqual(NotificationObject.Data);

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });
        }

        [Fact]
        public async Task NewDataNotification_IsRecievedWhenNewData_MultipleNotifications()
        {
            //given
            var count = 10;
            var title = "Title";
            var notifications = new List<NewDataNotification>();
            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.NewDataObservable.Subscribe(m => notifications.Add(m));

            //when
            for (int i = 0; i < count; ++i)
                await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId,
                    Title = title,
                });

            await WaitForNotifications();

            //then
            notifications.Count.ShouldEqual(count);
            notifications.All(n => n.Data.Title == title).ShouldBeTrue();

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });
        }

        [Fact]
        public async Task NewDataNotification_IsRecievedWhenNewData_MultipleNotifications_WithImages()
        {
            //given
            var count = 10;
            var notifications = new List<NewDataNotification>();
            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.NewDataObservable.Subscribe(m => notifications.Add(m));

            //when
            for (int i = 0; i < count; ++i)
                await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId,
                    Title = "test",
                    ImageBase64 = TestData.ImageToBase64("smallSampleImage.png")
                });

            await WaitForNotifications();

            //then
            notifications.Count.ShouldEqual(count);
            notifications.All(n => n.Data.Image != null).ShouldBeTrue();

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });
        }

        [Fact]
        public async Task DeleteDataNotification_IsRecievedWhenDeleteData()
        {
            //given
            DeleteDataNotification deleteDataNotification = null;
            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.DeleteDataObservable.Subscribe(m => deleteDataNotification = m);

            var newData = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "test"
            });

            //given
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });

            await WaitForNotifications();

            //then
            deleteDataNotification.ShouldNotBeNull();
            deleteDataNotification.Target.ShouldNotBeNull();
            deleteDataNotification.Target.Ids.ShouldNotBeEmpty();
            deleteDataNotification.Target.Ids.Count.ShouldEqual(1);

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);

        }

        [Fact]
        public async Task ChangeDataNotification_IsRecievedWhenUpdateData()
        {
            //given
            ChangeDataNotification changeDataNotification = null;
            var newTitle = "newTitle";
            var newText = "newText";
            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.ChangeDataObservable.Subscribe(m => changeDataNotification = m);

            var newData = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "test"
            });

            //when
            await _syncServer.DataObjects.Update(
                new DataObjectDefinitionRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId,
                    Title = newTitle,
                    Text = newText
                }, newData.Id);

            await WaitForNotifications();

            //then
            changeDataNotification.ShouldNotBeNull();

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });
        }

        [Fact]
        public async Task ChangeDataNotification_IsRecievedWhenUpdateData_WithAdditionals()
        {
            //given
            ChangeDataNotification changeDataNotification = null;
            var newTitle = "newTitle";
            var newText = "newText";
            var additionals = new Dictionary<string, string>();
            additionals.Add("key", "value");

            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.ChangeDataObservable.Subscribe(m => changeDataNotification = m);

            var newData = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "test",
                Additional = additionals
            });

            additionals.Clear();
            additionals.Add("newKey", "newValue");

            //when
            await _syncServer.DataObjects.Update(
                new DataObjectDefinitionRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId,
                    Title = newTitle,
                    Text = newText,
                    Additional = additionals
                }, newData.Id);

            await WaitForNotifications();

            //then
            changeDataNotification.ShouldNotBeNull();
            changeDataNotification.Target.ShouldNotBeNull();

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });
        }

        [Fact]
        public async Task DataRelationNotification_IsRecievedOnCreatingRelation_ByAddChild()
        {
            //given
            DataRelationNotification relationNotification = null;
            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.DataRelationObservable.Subscribe(m => relationNotification = m);

            var child = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "child",
            });

            var parent = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "parent",
            });

            //when
            await _syncServer.DataObjects.AddChild(TestData.ProjectId, parent.Id, child.Id, TestData.SubscriptionCollectionId);
            await WaitForNotifications();

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });

            //then
            relationNotification.ShouldNotBeNull();
            relationNotification.Type.ShouldEqual(NotificationType.New);
            relationNotification.Object.ShouldEqual(NotificationObject.DataRelation);
            relationNotification.Target.ParentId.ShouldEqual(parent.Id);
            relationNotification.Target.ChildId.ShouldEqual(child.Id);
        }

        [Fact]
        public async Task DataRelationNotification_IsRecievedOnCreatingRelation_ByAddParent()
        {
            //given
            DataRelationNotification relationNotification = null;
            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.DataRelationObservable.Subscribe(m => relationNotification = m);

            var child = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "child",
            });

            var parent = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "parent",
            });

            //when
            await _syncServer.DataObjects.AddParent(TestData.ProjectId, child.Id, parent.Id, TestData.SubscriptionCollectionId);
            await WaitForNotifications();

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });

            //then
            relationNotification.ShouldNotBeNull();
            relationNotification.Type.ShouldEqual(NotificationType.New);
            relationNotification.Object.ShouldEqual(NotificationObject.DataRelation);
            relationNotification.Target.ParentId.ShouldEqual(parent.Id);
            relationNotification.Target.ChildId.ShouldEqual(child.Id);
        }

        [Fact]
        public async Task DataRelationNotification_IsRecievedOnRemovingRelation_ByRemoveChild()
        {
            //given
            DataRelationNotification relationNotification = null;
            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.DataRelationObservable.Subscribe(m => relationNotification = m);

            var child = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "child",
            });

            var parent = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "parent",
            });

            await _syncServer.DataObjects.AddChild(TestData.ProjectId, parent.Id, child.Id, TestData.SubscriptionCollectionId);

            //when
            await
                _syncServer.DataObjects.RemoveChild(TestData.ProjectId, parent.Id, child.Id,
                    TestData.SubscriptionCollectionId);
            await WaitForNotifications();

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });

            //then
            relationNotification.ShouldNotBeNull();
            relationNotification.Type.ShouldEqual(NotificationType.Delete);
            relationNotification.Object.ShouldEqual(NotificationObject.DataRelation);
            relationNotification.Target.ParentId.ShouldEqual(parent.Id);
            relationNotification.Target.ChildId.ShouldEqual(child.Id);
        }

        [Fact]
        public async Task DataRelationNotification_IsRecievedOnRemovingRelation_ByRemoveParent()
        {
            //given
            DataRelationNotification relationNotification = null;
            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.DataRelationObservable.Subscribe(m => relationNotification = m);

            var child = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "child",
            });

            var parent = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "parent",
            });

            await _syncServer.DataObjects.AddChild(TestData.ProjectId, parent.Id, child.Id, TestData.SubscriptionCollectionId);

            //when
            await
                _syncServer.DataObjects.RemoveParent(TestData.ProjectId, child.Id, parent.Id,
                    TestData.SubscriptionCollectionId);
            await WaitForNotifications();

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });

            //then
            relationNotification.ShouldNotBeNull();
            relationNotification.Type.ShouldEqual(NotificationType.Delete);
            relationNotification.Object.ShouldEqual(NotificationObject.DataRelation);
            relationNotification.Target.ParentId.ShouldEqual(parent.Id);
            relationNotification.Target.ChildId.ShouldEqual(child.Id);
        }

        [Fact]
        public async Task DataRelationNotification_IsRecievedOnRemovingRelation_RemoveMultipleChildren()
        {
            //given
            DataRelationNotification relationNotification = null;
            await
                _syncSubscriber.RealTimeSync.SubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            _syncSubscriber.DataRelationObservable.Subscribe(m => relationNotification = m);

            var parent = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "parent",
            });
            var childOne = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "child",
                ParentId = parent.Id
            });

            var childTwo = await _syncServer.DataObjects.New(new DataObjectDefinitionRequest()
            {
                ProjectId = TestData.ProjectId,
                CollectionId = TestData.SubscriptionCollectionId,
                Title = "child",
                ParentId = parent.Id
            });

            //when
            await
                _syncServer.DataObjects.RemoveChild(TestData.ProjectId, parent.Id, null,
                    collectionId: TestData.SubscriptionCollectionId);
            await WaitForNotifications();

            //cleanup
            await
                _syncSubscriber.RealTimeSync.UnsubscribeCollection(TestData.ProjectId,
                    collectionId: TestData.SubscriptionCollectionId);
            await
                _syncServer.DataObjects.Delete(new DataObjectSimpleQueryRequest()
                {
                    ProjectId = TestData.ProjectId,
                    CollectionId = TestData.SubscriptionCollectionId
                });

            //then
            relationNotification.ShouldNotBeNull();
            relationNotification.Type.ShouldEqual(NotificationType.Delete);
            relationNotification.Object.ShouldEqual(NotificationObject.DataRelation);
            relationNotification.Target.ParentId.ShouldEqual(parent.Id);
            relationNotification.Target.ChildId.ShouldBeNull();
        }

        [Fact]
        public async Task GenericNotification_IsRecieved()
        {
            //given
            var genericNotifications = new List<GenericNotification>();
            _syncSubscriber.GenericNotificationObservable.Subscribe(genericNotifications.Add);

            //when
            await _syncServer.RealTimeSync.SendNotification();
            await WaitForNotifications();

            //then
            genericNotifications.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GenericNotification_IsRecieved_WithAdditionals()
        {
            //given
            var additionals = new Dictionary<string, object>();
            additionals.Add("key1", "value1");
            additionals.Add("key2", new object());
            additionals.Add("key3", 123);
            var genericNotifications = new List<GenericNotification>();
            _syncSubscriber.GenericNotificationObservable.Subscribe(genericNotifications.Add);

            //when
            await _syncServer.RealTimeSync.SendNotification(additional: additionals);
            await WaitForNotifications();

            //then
            genericNotifications.ShouldNotBeEmpty();
            genericNotifications.Any( n => n.Data.Count == 3).ShouldBeTrue();
        }
    }
}