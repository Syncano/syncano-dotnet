namespace SyncanoSyncServer.Net
{
    /// <summary>
    /// Class contains info about status of login operation.
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// Creates LoginResult object.
        /// </summary>
        /// <param name="wasSuccessful">Inforamtion about success or failure of login operation.</param>
        public LoginResult(bool wasSuccessful)
        {
            WasSuccessful = wasSuccessful;
        }

        /// <summary>
        /// Inforamtion about success or failure of login operation.
        /// </summary>
        public bool WasSuccessful { get; private set; }
    }
}