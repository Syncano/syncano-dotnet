using System;
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
    }
}
