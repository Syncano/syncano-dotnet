using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
#if Unity3d
using Syncano4.Unity3d;

#endif
#if dotNET
using Syncano4.Net;
using System.Threading.Tasks;

#endif

namespace Syncano4.Shared.Query
{
    public class SyncanoQuery<T> where T:DataObject
    {
        private readonly SyncanoDataObjects<T> _syncanoDataObjects;
        private List<FieldQuery> _fieldQueries = new List<FieldQuery>();

        public IEnumerable<FieldQuery> FieldQueries
        {
            get { return _fieldQueries; }
        }

        List<string> _orderBy = new List<string>();
        private List<FieldDef> _schema;


        public string ToJson()
        {
            if (_fieldQueries.Count == 0)
                return null;

            Dictionary<string, Dictionary<string,object>> query = new Dictionary<string, Dictionary<string, object>>();
            foreach (var fieldQuery in _fieldQueries)
            {
                if(query.ContainsKey(fieldQuery.FieldName) == false)
                    query.Add(fieldQuery.FieldName, new Dictionary<string, object>());

                query[fieldQuery.FieldName].Add(fieldQuery.OperatorName,fieldQuery.Value);
            }
            return JsonConvert.SerializeObject(query);
        }

        public SyncanoQuery(SyncanoDataObjects<T> syncanoDataObjects)
        {
            _syncanoDataObjects = syncanoDataObjects;
        }


        string GetFieldName(string propertyName)
        {
            if(_schema == null)
                _schema = SchemaMapping.GetSchema<T>();

            return _schema.Single(f => f.PropertyInfo.Name == propertyName).Name;
        }
      
        public SyncanoQuery<T> Where(Expression<Func<T, bool>> memberExpression)
        {
            var binaryExpression = memberExpression.Body as BinaryExpression;

            if (binaryExpression != null)
            {
                var propertyName = ((MemberExpression) binaryExpression.Left).Member.Name;

                

                var constant = binaryExpression.Right as ConstantExpression;
                
                _fieldQueries.Add(new FieldQuery(GetFieldName(propertyName), GetSyncanoOperatorName(binaryExpression.NodeType), constant.Value));
            }

            return this;
        }

        static string GetSyncanoOperatorName(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                    case ExpressionType.GreaterThan:
                    return "_gt";
                case ExpressionType.GreaterThanOrEqual:
                    return "_gte";
                case ExpressionType.LessThan:
                    return "_lt";
                case ExpressionType.LessThanOrEqual:
                    return "_lte";
                case ExpressionType.Equal:
                    return "_eq";
                case ExpressionType.NotEqual:
                    return "_neq";
                default:
                    throw new NotSupportedException($"Expression type {expressionType} is not supported");
            }
        }


        public static string GetPropertyName<K>(System.Linq.Expressions.Expression<Func<K, object>> property)
        {
            LambdaExpression lambda = (System.Linq.Expressions.LambdaExpression)property;

            var memberExpression = GetMemberExpression(lambda);

            return ((PropertyInfo)memberExpression.Member).Name;
        }

        private static MemberExpression GetMemberExpression(LambdaExpression lambda)
        {
            MemberExpression memberExpression;
            if (lambda.Body is System.Linq.Expressions.UnaryExpression)
            {
                System.Linq.Expressions.UnaryExpression unaryExpression =
                    (System.Linq.Expressions.UnaryExpression) (lambda.Body);
                memberExpression = (System.Linq.Expressions.MemberExpression) (unaryExpression.Operand);
            }
            else
            {
                memberExpression = (System.Linq.Expressions.MemberExpression) (lambda.Body);
            }
            return memberExpression;
        }

#if dotNET
        public Task<PageableResult<T>> ToListAsync()
        {
           return _syncanoDataObjects.PageableListAsync(this);
        }
#endif

#if Unity3d
        public PageableResult<T> ToList()
        {
            return _syncanoDataObjects.PageableList(this);
        }
#endif

        public SyncanoQuery<T> OrderByDescending(Expression<Func<T, object>> memberExpression)
        {
            string fieldName = GetFieldName(GetPropertyName(memberExpression));
            _orderBy.Add($"-{fieldName}");
            return this;
        }

        public string GetOrderByFields()
        {
            return string.Join(",", _orderBy.ToArray());
        }


    }
    }
