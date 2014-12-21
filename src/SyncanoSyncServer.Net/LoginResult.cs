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
        /// <param name="wasSuccessful">Information about success or failure of login operation.</param>
        /// <param name="reason">reason in case of unsuccessful login</param>
        public LoginResult(bool wasSuccessful, string reason)
        {
            WasSuccessful = wasSuccessful;
            Reason = reason;
        }

        /// <summary>
        /// Information about success or failure of login operation.
        /// </summary>
        public bool WasSuccessful { get; private set; }

        /// <summary>
        /// Reason of unsuccessful login
        /// </summary>
        public string Reason { get; private set; }
    }
}