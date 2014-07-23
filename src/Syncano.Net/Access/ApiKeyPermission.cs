namespace Syncano.Net.Access
{
    /// <summary>
    /// Permissions for api key.
    /// </summary>
    public enum ApiKeyPermission
    {
        /// <summary>
        /// Can send notifications through notification.send().
        /// </summary>
        SendNotification,

        /// <summary>
        /// Can create new users through user.new().
        /// </summary>
        AddUser,

        /// <summary>
        /// Can access Sync Server.
        /// </summary>
        AccessSync,

        /// <summary>
        /// Can subscribe to data through Sync Server.
        /// </summary>
        Subscribe
    }

    /// <summary>
    /// Class providing convertion from ApiKeyPermission to String.
    /// </summary>
    public class ApiKeyPermissionByStringConverter
    {
        /// <summary>
        /// Gets string representing ApiKeyPermission.
        /// </summary>
        /// <param name="permission">Permission to convertion.</param>
        /// <returns>String representing permission.</returns>
        public static string GetString(ApiKeyPermission permission)
        {
            string result;
            switch (permission)
            {
                case ApiKeyPermission.AccessSync:
                    result = "access_sync";
                    break;

                case ApiKeyPermission.AddUser:
                    result = "add_user";
                    break;

                case ApiKeyPermission.SendNotification:
                    result = "send_notification";
                    break;

                case ApiKeyPermission.Subscribe:
                    result = "subscribe";
                    break;

                default:
                    result = "";
                    break;
            }

            return result;
        }
    }
}
