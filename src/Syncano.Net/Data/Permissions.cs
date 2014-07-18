namespace Syncano.Net.Data
{
    /// <summary>
    /// User API client's permission to add. 
    /// </summary>
    public enum Permissions
    {
        /// <summary>
        /// Can create new Data Objects within container.
        /// </summary>
        CreateData,

        /// <summary>
        /// Can read all Data Objects within container.
        /// </summary>
        ReadData,

        /// <summary>
        /// Can read only Data Objects within container that were created by associated user.
        /// </summary>
        ReadOwnData,

        /// <summary>
        /// Can update all Data Objects within container.
        /// </summary>
        UpdateData,

        /// <summary>
        /// Can update only Data Objects within container that were created by associated user.
        /// </summary>
        UpdateOwnData,

        /// <summary>
        /// Can delete all Data Objects within container.
        /// </summary>
        DeleteData,

        /// <summary>
        /// Can delete only Data Objects within container that were created by associated user.
        /// </summary>
        DeleteOwnData
    };

    /// <summary>
    /// Static class providing 
    /// </summary>
    public static class PermissionsParser
    {
        /// <summary>
        /// Converts Permission to string.
        /// </summary>
        /// <param name="permission">Permission to convert.</param>
        /// <returns>String describing permission.</returns>
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
