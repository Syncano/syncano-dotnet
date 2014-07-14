﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public Task<DataObject> New(NewDataObjectRequest request)
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

        public Task<bool> Delete(DeleteDataObjectRequest request)
        {
            if(request.ProjectId == null)
                throw new ArgumentNullException();

            if (request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            if (request.Limit > MaxVauluesPerRequest)
                throw new ArgumentException();

            var dataIds = request.DataIds == null ? new List<string>() : new List<string>(request.DataIds);
            if(request.DataId != null)
                dataIds.Add(request.DataId);

            var folders = request.Folders == null ? new List<string>() : new List<string>(request.Folders);
            if (request.Folder != null)
                dataIds.Add(request.Folder);

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


    }
}