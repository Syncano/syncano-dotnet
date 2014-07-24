using System;
using System.IO;
namespace Syncano.Net.Tests
{
    public static class TestData
    {
        public const string InstanceName = "icy-brook-267066";

        public const string BackendAdminApiKey = "f020f3a62b2ea236100a732adcf60cb98683e2e5";
        public const string BackendAdminApiId = "1033";

        public const string ProjectName = "Default";
        public const string ProjectId = "1625";

        public const string CollectionId = "6490";
        public const string SubscriptionCollectionId = "8765";
        public const string CollectionKey = "default";

        public const string FolderName = "Default";
        public const string FolderId = "1";

        public const string UserApiClientId = "1086";
        public const string UserName = "UserName";
        public const string UserPassword = "qwerty";
        public const string UserId = "529";
        public const string UserApiKey = "2347b3ee5d7a9b13622e2812e34aaa7351dc7cbb";
        public const string OldUserId = "524";

        public const string RoleId = "4";
        public const string RoleName = "Viewer";

        public const string AdminId = "1115";
        public const string AdminEmail = "albertwolant@gmail.com";

        public static string ImageToBase64(string path)
        {

            using (var f = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var ms = new MemoryStream())
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
        public void Dispose()
        {
        }
    }
}