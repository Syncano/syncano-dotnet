using System.Collections.Generic;

namespace Syncano4.Shared.Query
{
    public class FieldQuery
    {
        public string OperatorName { get; }

        public object Value { get; }

        public string FieldName { get; }

        public FieldQuery(string fieldName, string operatorName, object value)
        {
            FieldName = fieldName;
            OperatorName = operatorName;
            Value = value;
        }
     }
}