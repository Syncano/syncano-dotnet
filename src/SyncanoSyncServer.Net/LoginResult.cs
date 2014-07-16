namespace SyncanoSyncServer.Net
{
    public class LoginResult
    {
        public LoginResult(bool wasSuccessful)
        {
            WasSuccessful = wasSuccessful;
        }

        public bool WasSuccessful { get; private set; }
    }
}