namespace Syncano.Net
{
    /// <summary>
    /// Possible orders for querying data objects.
    /// </summary>
    public enum DataObjectOrder
    {
        /// <summary>
        /// Older first.
        /// </summary>
        Ascending,

        /// <summary>
        /// Newest first.
        /// </summary>
        Descending
    }

    /// <summary>
    /// Possible keys to order data objects by.
    /// </summary>
    public enum DataObjectOrderBy
    {
        /// <summary>
        ///  Order by creation date.
        /// </summary>
        CreatedAt,

        /// <summary>
        /// Order by id.
        /// </summary>
        Id,

        /// <summary>
        /// Order by update date.
        /// </summary>
        UpdatedAt
    }

    public class DataObjectOrderByStringConverter
    {
        public static string GetString(DataObjectOrderBy orderBy)
        {
            string result;
            switch (orderBy)
            {
                case DataObjectOrderBy.CreatedAt:
                    result = "created_at";
                    break;

                case DataObjectOrderBy.Id:
                    result = "id";
                    break;

                case DataObjectOrderBy.UpdatedAt:
                    result = "updated_at";
                    break;

                default:
                    result = "";
                    break;
            }

            return result;
        }
    }

    public class DataObjectOrderStringConverter
    {
        public static string GetString(DataObjectOrder order)
        {
            string result;
            switch (order)
            {
                case DataObjectOrder.Ascending:
                    result = "ASC";
                    break;

                case DataObjectOrder.Descending:
                    result = "DESC";
                    break;

                default:
                    result = "";
                    break;
            }

            return result;
        }
    }
}
