namespace Syncano.Net.Data
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

    /// <summary>
    /// Class providing functionality of converting DataObjectOrderBy to String.
    /// </summary>
    public class DataObjectOrderByStringConverter
    {
        /// <summary>
        /// Gets string representing DataObjectOrderBy object.
        /// </summary>
        /// <param name="orderBy">OrderBy object.</param>
        /// <returns>String object.</returns>
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

    /// <summary>
    /// Class providing functionality of converting DataObjectOrder to String.
    /// </summary>
    public class DataObjectOrderStringConverter
    {
        /// <summary>
        /// Gets string representing DataObjectOrder object.
        /// </summary>
        /// <param name="order">Order object.</param>
        /// <returns>String object.</returns>
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
