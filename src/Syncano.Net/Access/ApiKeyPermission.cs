namespace Syncano.Net.Access
{
    public enum ApiKeyPermission
    {
        SendNotification,
        AddUser,
        AccessSync,
        Subscribe
    }

    public class ApiKeyPermissionByStringConverter
    {
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
