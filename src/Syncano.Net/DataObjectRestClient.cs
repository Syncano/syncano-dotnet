using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncano.Net
{
    public class DataObjectRestClient
    {
        public static readonly int MaxVauluesPerRequest = 100;

        private readonly SyncanoRestClient _restClient;

        public DataObjectRestClient(SyncanoRestClient restClient)
        {
            _restClient = restClient;
        }


        public Task<DataObject> New(DataObjectDefinitionRequest request)
        {
            if(request.ProjectId == null)
                throw new ArgumentNullException();

            if(request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            return _restClient.PostAsync("data.new",
                new
                {
                    project_id = request.ProjectId,
                    collection_id = request.CollectionId,
                    collection_key = request.CollectionKey,
                    data_key = request.DataKey,
                    user_name = request.UserName,
                    source_url = request.SourceUrl,
                    title = request.Title,
                    text = request.Text,
                    link = request.Link,
                    image = request.ImageBase64,
                    image_url = request.ImageUrl,
                    folder = request.Folder,
                    state = request.State,
                    parent_id = request.ParentId,
                    data1 = request.DataOne,
                    data2 = request.DataTwo,
                    data3 = request.DataThree
                }, "data", t => t.ToObject<DataObject>());
        }

        public async Task<List<DataObject>> Get(DataObjectRichQueryRequest request)
        {
            if (request.ProjectId == null)
                throw new ArgumentNullException();

            if (request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            if (request.Limit > MaxVauluesPerRequest || request.Limit < 0)
                throw new ArgumentException();

            if (request.ChildrenLimit > MaxVauluesPerRequest || request.ChildrenLimit < 0)
                throw new ArgumentException();

            var dataIds = request.DataIds == null ? new List<string>() : new List<string>(request.DataIds);
            if (dataIds.Count + (request.DataId != null ? 1 : 0) > MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.DataId != null)
                dataIds.Add(request.DataId);

            var folders = request.Folders == null ? new List<string>() : new List<string>(request.Folders);
            if (folders.Count + (request.Folder != null ? 1 : 0) >
                MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.Folder != null)
                folders.Add(request.Folder);

            var parentIds = request.ParentIds == null ? new List<string>() : new List<string>(request.ParentIds);
            if (parentIds.Count + (request.ParentId != null ? 1 : 0) > MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.ParentId != null)
                parentIds.Add(request.ParentId);

            var childIds = request.ChildIds == null ? new List<string>() : new List<string>(request.ChildIds);
            if (childIds.Count + (request.ChildId != null ? 1 : 0) > MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.ChildId != null)
                childIds.Add(request.ChildId);

            return
                await
                    _restClient.PostAsync("data.get",
                        new
                        {
                            project_id = request.ProjectId,
                            collection_id = request.CollectionId,
                            collection_key = request.CollectionKey,
                            data_ids = dataIds.ToArray(),
                            state = request.State,
                            folders = folders.ToArray(),
                            since = request.Since,
                            max_id = request.MaxId,
                            limit = request.Limit,
                            order = DataObjectOrderStringConverter.GetString(request.Order),
                            order_by = DataObjectOrderByStringConverter.GetString(request.OrderBy),
                            filter = request.Filter,
                            include_children = request.IncludeChildren,
                            depth = request.Depth,
                            children_limit = request.ChildrenLimit,
                            parent_ids = parentIds.ToArray(),
                            child_ids = childIds.ToArray(),
                            by_user = request.ByUser
                        }, "data", t => t.ToObject<List<DataObject>>());
        }

        public Task<DataObject> GetOne(string projectId, string collectionId = null, string collectionKey = null,
            string dataId = null, string dataKey = null, bool includeChildren = false, int? depth = null,
            int childrenLimit = 100)
        {
            if (projectId == null)
                throw new ArgumentNullException();

            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(childrenLimit < 0 || childrenLimit > MaxVauluesPerRequest)
                throw new ArgumentException();

            return _restClient.GetAsync("data.get_one",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    data_id = dataId,
                    data_key = dataKey,
                    include_children = includeChildren,
                    depth,
                    children_limit = childrenLimit
                }, "data", t => t.ToObject<DataObject>());
        }

        public Task<DataObject> Update(DataObjectDefinitionRequest request, string dataId = null)
        {
            if (request.ProjectId == null)
                throw new ArgumentNullException();

            if (request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            return _restClient.PostAsync("data.update",
                new
                {
                    project_id = request.ProjectId,
                    collection_id = request.CollectionId,
                    collection_key = request.CollectionKey,
                    data_id = dataId,
                    data_key = request.DataKey,
                    update_method = "replace",
                    user_name = request.UserName,
                    source_url = request.SourceUrl,
                    title = request.Title,
                    text = request.Text,
                    link = request.Link,
                    image = request.ImageBase64,
                    image_url = request.ImageUrl,
                    data1 = request.DataOne,
                    data2 = request.DataTwo,
                    data3 = request.DataThree,
                    folder = request.Folder,
                    state = request.State,
                    parent_id = request.ParentId
                }, "data", t => t.ToObject<DataObject>());
        }

        public Task<DataObject> Merge(DataObjectDefinitionRequest request, string dataId = null)
        {
            if (request.ProjectId == null)
                throw new ArgumentNullException();

            if (request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            return _restClient.PostAsync("data.update",
                new
                {
                    project_id = request.ProjectId,
                    collection_id = request.CollectionId,
                    collection_key = request.CollectionKey,
                    data_id = dataId,
                    data_key = request.DataKey,
                    update_method = "merge",
                    user_name = request.UserName,
                    source_url = request.SourceUrl,
                    title = request.Title,
                    text = request.Text,
                    link = request.Link,
                    image = request.ImageBase64,
                    image_url = request.ImageUrl,
                    data1 = request.DataOne,
                    data2 = request.DataTwo,
                    data3 = request.DataThree,
                    folder = request.Folder,
                    state = request.State,
                    parent_id = request.ParentId
                }, "data", t => t.ToObject<DataObject>());
        }

        public Task<bool> Move(DataObjectSimpleQueryRequest request, string newFolder = null,
            DataObjectState? newState = null)
        {
            if (request.ProjectId == null)
                throw new ArgumentNullException();

            if (request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            if (request.Limit > MaxVauluesPerRequest || request.Limit < 0)
                throw new ArgumentException();

            var dataIds = request.DataIds == null ? new List<string>() : new List<string>(request.DataIds);
            if (dataIds.Count + (request.DataId != null ? 1 : 0) > MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.DataId != null)
                dataIds.Add(request.DataId);

            var folders = request.Folders == null ? new List<string>() : new List<string>(request.Folders);
            if (folders.Count + (request.Folder != null ? 1 : 0) >
                MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.Folder != null)
                folders.Add(request.Folder);

            return _restClient.PostAsync("data.move",
                new
                {
                    project_id = request.ProjectId,
                    collection_id = request.CollectionId,
                    collection_key = request.CollectionKey,
                    data_ids = dataIds.ToArray(),
                    folders = folders.ToArray(),
                    state = request.State,
                    filter = request.Filter,
                    by_user = request.ByUser,
                    limit = request.Limit,
                    new_folder = newFolder,
                    new_state = newState
                });
        }


        public async Task<List<DataObject>> Copy(CopyDataObjectRequest request)
        {
            if (request.ProjectId == null)
                throw new ArgumentNullException();

            if (request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            var dataIds = request.DataIds == null ? new List<string>() : new List<string>(request.DataIds);
            if (dataIds.Count + (request.DataId != null ? 1 : 0) > MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.DataId != null)
                dataIds.Add(request.DataId);

            if(dataIds.Count < 1)
                throw new ArgumentException();

            return await _restClient.GetAsync("data.copy", new
            {
                project_id = request.ProjectId,
                collection_id = request.CollectionId,
                collection_key = request.CollectionKey,
                data_ids = dataIds.ToArray()
            }, "data", t => t.ToObject<List<DataObject>>());
        }

        public Task<bool> AddParent(string projectId, string dataId, string parentId, string collectionId = null,
            string collectionKey = null, bool removeOther = false)
        {
            if (projectId == null)
                throw new ArgumentNullException();

            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (dataId == null || parentId == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("data.add_parent",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    data_id = dataId,
                    parent_id = parentId,
                    remove_other = removeOther
                });
        }

        public Task<bool> RemoveParent(string projectId, string dataId, string parentId = null, string collectionId = null,
            string collectionKey = null)
        {
            if (projectId == null)
                throw new ArgumentNullException();

            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (dataId == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("data.remove_parent",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    data_id = dataId,
                    parent_id = parentId
                });
        }

        public Task<bool> AddChild(string projectId, string dataId, string childId, string collectionId = null,
            string collectionKey = null, bool removeOther = false)
        {
            if (projectId == null)
                throw new ArgumentNullException();

            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(dataId == null || childId == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("data.add_child",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    data_id = dataId,
                    child_id = childId,
                    remove_other = removeOther
                });
        }

        public Task<bool> RemoveChild(string projectId, string dataId, string childId, string collectionId = null,
            string collectionKey = null)
        {
            if (projectId == null)
                throw new ArgumentNullException();

            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (dataId == null || childId == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("data.remove_child",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    data_id = dataId,
                    child_id = childId
                });
        }

        public Task<bool> Delete(DataObjectSimpleQueryRequest request)
        {
            if(request.ProjectId == null)
                throw new ArgumentNullException();

            if (request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            if (request.Limit > MaxVauluesPerRequest || request.Limit < 0)
                throw new ArgumentException();

            var dataIds = request.DataIds == null ? new List<string>() : new List<string>(request.DataIds);
            if (dataIds.Count + (request.DataId != null ? 1 : 0) > MaxVauluesPerRequest)
                throw new ArgumentException();
            if(request.DataId != null)
                dataIds.Add(request.DataId);

            var folders = request.Folders == null ? new List<string>() : new List<string>(request.Folders);
            if (folders.Count + (request.Folder != null ? 1 : 0) >
                MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.Folder != null)
                folders.Add(request.Folder);

            return _restClient.GetAsync("data.delete",
                new
                {
                    project_id = request.ProjectId,
                    collection_id = request.CollectionId,
                    collection_key = request.CollectionKey,
                    data_ids = dataIds.ToArray(),
                    state = request.State,
                    folders = folders.ToArray(),
                    filter = request.Filter,
                    by_user = request.ByUser,
                    limit = request.Limit
                });
        }

        public Task<int> Count(CountDataObjectRequest request)
        {
            if (request.ProjectId == null)
                throw new ArgumentNullException();

            if (request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            var folders = request.Folders == null ? new List<string>() : new List<string>(request.Folders);
            if (folders.Count + (request.Folder != null ? 1 : 0) >
                MaxVauluesPerRequest)
                throw new ArgumentException();
            if (request.Folder != null)
                folders.Add(request.Folder);

            return _restClient.GetAsync("data.count",
                new
                {
                    project_id = request.ProjectId,
                    collection_id = request.CollectionId,
                    collection_key = request.CollectionKey,
                    state = request.State,
                    folders = folders.ToArray(),
                    filter = request.Filter,
                    by_user = request.ByUser
                }, "count", t => t.ToObject<int>());
        }
    }
}
