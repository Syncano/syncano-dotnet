﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano4.Shared;

namespace Syncano4.Tests.Unity3d
{
    public static class Extensions
    {
        public static Task<IList<Instance>> GetAsync(this SyncanoInstances instances)
        {
            return Task.FromResult(instances.Get());
        }

        public static Task<IList<SyncanoClass>> GetAsync(this ClassDefinitions instances)
        {
            return Task.FromResult(instances.Get());
        }

        public static Task<SyncanoClass> AddAsync(this ClassDefinitions instances, string name, string description, IList<SyncanoFieldSchema> schema)
        {
            return Task.FromResult(instances.Add(name, description,schema));
        }

        public static Task<Instance> AddAsync(this SyncanoInstances instances, string name, string description)
        {
            return Task.FromResult(instances.Add(name, description));
        }

        public static Task<T> AddAsync<T>(this SyncanoDataObjects instances, T dataobject)
        {
            return Task.FromResult(instances.Add(dataobject));
        }

        public static Task<IList<T>> GetAsync<T>(this SyncanoDataObjects instances, int pageSize)
        {
            return Task.FromResult(instances.Get<T>(pageSize:pageSize));
        }

    }
}
