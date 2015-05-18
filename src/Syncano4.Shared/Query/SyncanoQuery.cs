﻿using System;
using System.Collections.Generic;
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
        private FieldQuery _fieldQuery;
        List<string> _orderBy = new List<string>();

        public Dictionary<string, Dictionary<string, object>> ToDictionary()
        {
            return _fieldQuery.ToDictionary();
        }

        public string ToJson()
        {
            if (_fieldQuery == null)
                return null;

            return JsonConvert.SerializeObject(_fieldQuery.ToDictionary());
        }

        public SyncanoQuery(SyncanoDataObjects<T> syncanoDataObjects)
        {
            _syncanoDataObjects = syncanoDataObjects;
        }

        public SyncanoQuery<T> Where(Expression<Func<T, object>> memberExpression, object i)
        {
            string fieldName = GetPropertyName(memberExpression).ToLower();
            _fieldQuery = new FieldQuery(fieldName, new SyncanoEqual(i));
            return this;
        }


        public static string GetPropertyName<K>(System.Linq.Expressions.Expression<Func<K, object>> property)
        {
            System.Linq.Expressions.LambdaExpression lambda = (System.Linq.Expressions.LambdaExpression)property;
            System.Linq.Expressions.MemberExpression memberExpression;

            if (lambda.Body is System.Linq.Expressions.UnaryExpression)
            {
                System.Linq.Expressions.UnaryExpression unaryExpression = (System.Linq.Expressions.UnaryExpression)(lambda.Body);
                memberExpression = (System.Linq.Expressions.MemberExpression)(unaryExpression.Operand);
            }
            else
            {
                memberExpression = (System.Linq.Expressions.MemberExpression)(lambda.Body);
            }

            return ((PropertyInfo)memberExpression.Member).Name;
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
            string fieldName = GetPropertyName(memberExpression).ToLower();
            _orderBy.Add($"-{fieldName}");
            return this;
        }

        public string GetOrderByFields()
        {
            return string.Join(",", _orderBy.ToArray());
        }
    }


    public abstract class SyncanoQueryExpression
    {
        public abstract Dictionary<string, object> ToDictionary();
    }

    class SyncanoEqual : SyncanoQueryExpression
    {
        private readonly object _value;

        public SyncanoEqual(object value)
        {
            _value = value;
        }


        public override Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>()
            {
                { "_eq", _value}
            };
        }
    }
}
