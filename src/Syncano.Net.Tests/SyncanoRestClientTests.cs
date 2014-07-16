using System;
using System.IO;
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
        public const string ProjectId = "1625";

        public const string CollectionId = "6490";
        public const string CollectionKey = "default";

        public const string FolderName = "Default";
        public const string FolderId = "1";

        public const string UserApiClientId = "1086";
        public const string UserName = "UserName";

        public static string ImageToBase64(string path)
        {

            using (var f = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (MemoryStream ms = new MemoryStream())
                {

                    f.CopyTo(ms);
                    byte[] imageBytes = ms.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
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

        public void Dispose()
        {
        }
    }
}