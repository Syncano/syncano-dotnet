using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Syncano.Net.Data;

namespace Syncano.Net.DataRequests
{
    /// <summary>
    /// Request for querying data objects.
    /// </summary>
    public class DataObjectRichQueryRequest : DataObjectSimpleQueryRequest
    {
        /// <summary>
        /// Creates DataObjectRichQueryRequest object.
        /// </summary>
        public DataObjectRichQueryRequest()
        {
            Order = DataObjectOrder.Ascending;
            OrderBy = DataObjectOrderBy.CreatedAt;
            ChildrenLimit = 100;
        }

        /// <summary>
        /// Used for paginating. Note: has no effect on returned data object's children. To paginate results ordered by created_at, id or updated_at: pass value of newest (when order is asc) or oldest (when order is desc) known value from the results in relevant type. E.g. when sorted by created_at, pass string with datetime with created_at value you want to filter from.
        /// </summary>
        public DateTime? Since { get; set; }

        /// <summary>
        /// If specified, will only return data with id lower than max_id (older).
        /// </summary>
        public string MaxId { get; set; }

        /// <summary>
        /// Sets order of data that will be returned.
        /// </summary>
        public DataObjectOrder Order { get; set; }

        /// <summary>
        /// Orders by specified criteria.
        /// </summary>
        public DataObjectOrderBy OrderBy { get; set; }

        /// <summary>
        /// If true, include Data Object children as well (recursively). Default value: True. Max 100 of children are shown in one request.
        /// </summary>
        public bool IncludeChildren { get; set; }

        /// <summary>
        /// Max depth of children to follow. If not specified, will follow all levels until children limit is reached.
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Limit of children to show (if include_children is True). Default and max value: 100 (some children levels may be incomplete if there are more than this limit).
        /// </summary>
        public int ChildrenLimit { get; set; }

        /// <summary>
        /// Data Object id. If specified, only children of specific Data Object parents will be listed.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Data Object ids. If specified, only children of specific Data Object parents will be listed.
        /// </summary>
        public List<string> ParentIds { get; set; }

        /// <summary>
        /// Data Object id. If specified, only parents of specific Data Object children will be listed.
        /// </summary>
        public string ChildId { get; set; }

        /// <summary>
        /// Data Object ids. If specified, only parents of specific Data Object children will be listed.
        /// </summary>
        public List<string> ChildIds { get; set; }


        private readonly Dictionary<string, string> _dataFieldFilters = new Dictionary<string, string>();

        /// <summary>
        /// Date field Filters. To add user <see cref="AddDataFieldFilter"/>
        /// </summary>
        public ReadOnlyCollection<KeyValuePair<string, string>> DataFieldFilters
        {
            get { return new ReadOnlyCollection<KeyValuePair<string, string>>(_dataFieldFilters.ToList()); }
        }


        /// <summary>
        /// Adds filters for special fields: DataOne,DataTwo and DataThree
        /// </summary>
        /// <param name="specialField"></param>
        /// <param name="operatorName"></param>
        /// <param name="value"></param>
        public void AddDataFieldFilter(DataObjectSpecialField specialField, DataObjectOperator operatorName, long value)
        {
            string specialFieldName = specialField == DataObjectSpecialField.DataOne ? "data1" : (specialField == DataObjectSpecialField.DataTwo) ? "data2" : "data3";

            string operatorString = "";
            switch (operatorName)
            {
                case DataObjectOperator.Equals:
                    operatorString = "eq";
                    break;
                case DataObjectOperator.GreaterThan:
                    operatorString = "gt";
                    break;
                case DataObjectOperator.GreaterThanOrEquals:
                    operatorString = "gte";
                    break;
                case DataObjectOperator.LowerThan:
                    operatorString = "lt";
                    break;
                case DataObjectOperator.LowerThanOrEquals:
                    operatorString = "lte";
                    break;
                case DataObjectOperator.NotEquals:
                    operatorString = "neq";
                    break;
            }
            _dataFieldFilters.Add(string.Format("{0}__{1}", specialFieldName, operatorString), value.ToString());
        }
    }
}