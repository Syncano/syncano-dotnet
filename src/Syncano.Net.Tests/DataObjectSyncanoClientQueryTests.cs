using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Syncano.Net.Data;
using Syncano.Net.DataRequests;
using Syncano.Net.Http;
using Xunit.Extensions;

namespace Syncano.Net.Tests
{
    public class DataObjectSyncanoClientQueryTests : IDisposable
    {

        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_Data1_OrderBy(DataObjectSyncanoClient client)
        {
            //given
            var newData1 = new DataObjectDefinitionRequest();
            newData1.ProjectId = TestData.ProjectId;
            newData1.CollectionId = TestData.CollectionId;
            newData1.DataOne = 1002;

            var newData2 = new DataObjectDefinitionRequest();
            newData2.ProjectId = TestData.ProjectId;
            newData2.CollectionId = TestData.CollectionId;
            newData2.DataOne = 1001;

            var dataObject1 = await client.New(newData1);
            var dataObject2 = await client.New(newData2);

            var getRequest = new DataObjectRichQueryRequest();
            getRequest.ProjectId = TestData.ProjectId;
            getRequest.CollectionId = TestData.CollectionId;
            getRequest.OrderBy  = DataObjectOrderBy.DataOne;
            getRequest.Order = DataObjectOrder.Ascending;
            getRequest.AddDataFieldFilter(DataObjectSpecialField.DataOne, DataObjectOperator.GreaterThan, 1000);

            //when
            var result =
                await client.Get(getRequest);

            //then
            result.ShouldNotBeNull();
            result.All(d => d.DataOne >1000).ShouldBeTrue();
            result.Count.ShouldEqual(2);
            result[0].Id.ShouldEqual(dataObject2.Id);
            result[1].Id.ShouldEqual(dataObject1.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }


        [Theory, PropertyData("DataObjectSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Get_Data2_OrderBy(DataObjectSyncanoClient client)
        {
            //given
            var newData1 = new DataObjectDefinitionRequest();
            newData1.ProjectId = TestData.ProjectId;
            newData1.CollectionId = TestData.CollectionId;
            newData1.DataTwo = 1;

            var newData2 = new DataObjectDefinitionRequest();
            newData2.ProjectId = TestData.ProjectId;
            newData2.CollectionId = TestData.CollectionId;
            newData2.DataTwo = 2;

            var dataObject1 = await client.New(newData1);
            var dataObject2 = await client.New(newData2);

            var getRequest = new DataObjectRichQueryRequest();
            getRequest.ProjectId = TestData.ProjectId;
            getRequest.CollectionId = TestData.CollectionId;
            getRequest.OrderBy = DataObjectOrderBy.DataOne;
            getRequest.Order = DataObjectOrder.Ascending;
            getRequest.AddDataFieldFilter(DataObjectSpecialField.DataTwo, DataObjectOperator.LowerThanOrEquals, 2);
            getRequest.AddDataFieldFilter(DataObjectSpecialField.DataTwo, DataObjectOperator.GreaterThanOrEquals, 1);

            //when
            var result =
                await client.Get(getRequest);

            //then
            result.ShouldNotBeNull();
            result.Count.ShouldEqual(2);
            result[0].Id.ShouldEqual(dataObject1.Id);
            result[1].Id.ShouldEqual(dataObject2.Id);

            //cleanup
            var deleteRequest = new DataObjectSimpleQueryRequest();
            deleteRequest.ProjectId = TestData.ProjectId;
            deleteRequest.CollectionId = TestData.CollectionId;
            await client.Delete(deleteRequest);
        }


        public void Dispose()
        {
           
        }
    }
}
