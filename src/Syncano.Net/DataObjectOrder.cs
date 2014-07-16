namespace Syncano.Net
{
    public enum DataObjectOrder
    {
        Ascending,
        Descending
    }

    public enum DataObjectOrderBy
    {
        CreatedAt,
        Id,
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
