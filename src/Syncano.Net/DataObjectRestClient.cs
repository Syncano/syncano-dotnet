using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncano.Net
{
    public class DataObjectRestClient
    {
        private static readonly int MAX_VALUES_PER_REQUEST = 100;

        private readonly SyncanoRestClient _restClient;

        public DataObjectRestClient(SyncanoRestClient restClient)
        {
            _restClient = restClient;
        }

        public Task<DataObject> New(NewDataObjectRequest request)
        {
            return New(request.ProjectId, request.CollectionId, request.CollectionKey, request.DataKey, request.UserName,
                request.SourceUrl, request.Title, request.Text, request.Link, request.ImageBase64, request.ImageUrl,
                request.DataOne, request.DataTwo, request.DataThree, request.Folder, request.State, request.ParentId);
        }

        private Task<DataObject> New(string projectId, string collectionId = null, string collectionKey = null,
            string dataKey = null, string userName = null, string sourceUrl = null, string title = null,
            string text = null, string link = null, string imageBase64 = null, string imageUrl = null,
            int? dataOne = null, int? dataTwo = null, int? dataThree = null, string folder = "Default",
            DataObjectState state = DataObjectState.Pending, string parentId = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("data.new",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    data_key = dataKey,
                    user_name = userName,
                    source_url = sourceUrl,
                    title = title,
                    text = text,
                    link = link,
                    image = imageBase64,
                    image_url = imageUrl,
                    folder = folder,
                    state = state,
                    parent_id = parentId,
                    data1 = dataOne,
                    data2 = dataTwo,
                    data3 = dataThree
                }, "data", t => t.ToObject<DataObject>());
        }

        public Task<bool> Delete(string projectId, string[] dataIds, string[] folders, string collectionId = null,
            string collectionKey = null,
            DataObjectState state = DataObjectState.All, DataObjectContentFilter? filter = null, string byUser = null,
            int limit = 100)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (limit > MAX_VALUES_PER_REQUEST)
                throw new ArgumentException();
            if (dataIds != null)
            {
                if (dataIds.Length > MAX_VALUES_PER_REQUEST)
                    throw new ArgumentException();
                if (dataIds.Length == 0)
                    dataIds = null;
            }

            if (folders != null)
            {
                if (folders.Length > MAX_VALUES_PER_REQUEST)
                    throw new ArgumentException();
                if (folders.Length == 0)
                    folders = null;
            }

            return _restClient.GetAsync("data.delete",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    data_ids = dataIds,
                    state = state,
                    folders = folders,
                    filter = filter,
                    by_user = byUser,
                    limit = limit
                });
        }


    }
}
