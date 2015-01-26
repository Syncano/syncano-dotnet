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
        UpdatedAt,
        /// <summary>
        /// Order by values in field DataOne
        /// </summary>
        DataOne,
        /// <summary>
        /// Order by values in field DataTwo
        /// </summary>
        DataTwo,
        /// <summary>
        /// Order by values in field DataThree
        /// </summary>
        DataThree
    }


    /// <summary>
    /// Possible keys to order data objects by.
    /// </summary>
    public enum DataObjectOperator
    {

        /// <summary>
        /// Equals
        /// </summary>
        Equals,
        /// <summary>
        /// Not Equals
        /// </summary>
        NotEquals,
        /// <summary>
        /// Lower Than
        /// </summary>
        LowerThan,
        /// <summary>
        /// Lower Than or equals
        /// </summary>
        LowerThanOrEquals,
        /// <summary>
        /// Greater Than
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Greater Than or equals
        /// </summary>
        GreaterThanOrEquals,
        
    }

    /// <summary>
    /// Possible fields to add additional filters for.
    /// </summary>
    public enum DataObjectSpecialField
    {

        /// <summary>
        /// Field DataOne
        /// </summary>
        DataOne,
        /// <summary>
        /// Field DataTwo
        /// </summary>
        DataTwo,
        /// <summary>
        /// Field DataThree
        /// </summary>
        DataThree
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

                case DataObjectOrderBy.DataOne:
                    result = "data1";
                    break;

                case DataObjectOrderBy.DataTwo:
                    result = "data2";
                    break;

                case DataObjectOrderBy.DataThree:
                    result = "data3";
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
