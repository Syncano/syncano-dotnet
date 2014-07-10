namespace Syncano.Net
{
    public enum Permissions
    {
        CreateData,
        ReadData,
        ReadOwnData,
        UpdateData,
        UpdateOwnData,
        DeleteData,
        DeleteOwnData
    };

    public static class PermissionsParser
    {
        public static string GetString(Permissions permission)
        {
            string value = "";

            switch (permission)
            {
                case Permissions.CreateData:
                    value = "create_data";
                    break;
                case Permissions.ReadData:
                    value = "read_data";
                    break;
                case Permissions.ReadOwnData:
                    value = "read_own_data";
                    break;
                case Permissions.UpdateData:
                    value = "update_data";
                    break;
                case Permissions.UpdateOwnData:
                    value = "update_own_data";
                    break;
                case Permissions.DeleteData:
                    value = "delete_data";
                    break;
                case Permissions.DeleteOwnData:
                    value = "delete_own_data";
                    break;
            }

            return value;
        }

    }
}
