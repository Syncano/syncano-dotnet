using System.Collections.Generic;

namespace Syncano4.Shared.Query
{
    public class FieldQuery
    {
        private readonly string _fieldName;
        private readonly SyncanoQueryExpression _expression;

        public string FieldName
        {
            get { return _fieldName; }
        }
        
        public FieldQuery(string fieldName, SyncanoQueryExpression expression)
        {
            _fieldName = fieldName;
            _expression = expression;
        }

        public Dictionary<string, Dictionary<string, object>> ToDictionary()
        {
            return new Dictionary<string, Dictionary<string, object>>() { { FieldName,_expression.ToDictionary() } };
        }

    }
}