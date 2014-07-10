﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Xunit;

namespace Syncano.Net.Tests
{
    public static class TestData
    {
        public const string InstanceName = "icy-brook-267066";

        public const string BackendAdminApiKey = "f020f3a62b2ea236100a732adcf60cb98683e2e5";

        public const string ProjectName = "Default";
        public const string ProjectId = "1288";

        public const string CollectionId = "4995";
        public const string CollectionKey = "default";

        public const string FolderName = "Default";
    }

    public class SyncanoRestClientTests : IDisposable
    {
        private SyncanoRestClient _client;


        public SyncanoRestClientTests()
        {
            _client = new SyncanoRestClient(TestData.InstanceName, TestData.BackendAdminApiKey);
        }

        [Fact]
        public async Task StartSession_WhenValidInstanceAndKey_CreatesSessionId()
        {
            //when 
            var sessionId = await _client.StartSession();

            //then
            sessionId.ShouldNotBeNull();
        }


        [Fact]
        public async Task StartSession_WithInvalidKeys_ThrowsException()
        {
            try
            {
                var session = await new SyncanoRestClient(TestData.InstanceName, "2123").StartSession();

                throw new Exception("StartSession should throw exception");
                
            }
            catch (Exception e)
            {
                e.ShouldBeType<SyncanoException>();
            }
        }

        [Fact]
        public async Task GetProjects()
        {
            //when
            var response = await _client.GetProjects();

            //then
            response.ShouldNotBeEmpty();
            response.Any(p => p.Name == TestData.ProjectName).ShouldBeTrue();
        }

        [Fact]
        public async Task GetProject()
        {
            //when
            var project = await _client.GetProject(TestData.ProjectId);

            //then
            project.Id.ShouldEqual(TestData.ProjectId);
            project.Name.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetFoldersByCollectionId()
        {
            //when
            var response = await _client.GetFoldersByCollectionId(TestData.ProjectId, TestData.CollectionId);

            //then
            response.ShouldNotBeEmpty();
            response.Any(f => f.Name == TestData.FolderName).ShouldBeTrue();
        }

        [Fact]
        public async Task GetFoldersByCollectionKey()
        {
            //when
            var response = await _client.GetFoldersByCollectionKey(TestData.ProjectId, TestData.CollectionKey);

            //then
            response.ShouldNotBeEmpty();
            response.Any(f => f.Name == TestData.FolderName).ShouldBeTrue();
        }


        public void Dispose()
        {
        }
    }
}