using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Syncano4.Shared;
using Shouldly;
using Syncano4.Shared.Query;
using Syncano4.Shared.Serialization;
#if Unity3d
using Syncano4.Unity3d;
using Syncano4.Tests.Unity3d;
#endif
#if dotNET
using Syncano4.Net;
#endif
using Xunit;

namespace Syncano4.Tests.Shared
{
    public class QueryTests
    {
        
        public class TestObject : DataObject
        {
            [SyncanoField("myid", CanBeFiltered = true, CanBeOrdered = true)]
            public long MyId { get; set; }

            [SyncanoField("name")]
            public string Name { get; set; }

            [SyncanoField("current_time")]
            public DateTime CurrentTime { get; set; }

            [SyncanoField("ischecked")]
            public bool IsChecked { get; set; }

            [SyncanoField("float")]
            public float Float { get; set; }

            [SyncanoField("longtext")]
            public string LongText { get; set; }
            }

        [Fact]
        public void GreaterThan()
        {
            var q = new SyncanoQuery<TestObject>(null);

             var fieldQueries = q.Where( t => t.MyId > 0).FieldQueries;

            fieldQueries.ShouldContain(f => f.FieldName == "myid" && f.OperatorName == "_gt" && f.Value.Equals(0L));

        }


        [Fact]
        public void MultipleOperators_for_one_field()
        {
            var q = new SyncanoQuery<TestObject>(null);

            var fieldQueries = q.Where(t => t.MyId > 0).Where(t => t.MyId <= 10).FieldQueries;

            fieldQueries.ShouldContain(f => f.FieldName == "myid" && f.OperatorName == "_gt" && f.Value.Equals(0L));
            fieldQueries.ShouldContain(f => f.FieldName == "myid" && f.OperatorName == "_lte" && f.Value.Equals(10L));

        }

    }
}
