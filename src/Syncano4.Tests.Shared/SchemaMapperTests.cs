using System;
using Newtonsoft.Json;
using Syncano4.Shared;
using Xunit;
using Shouldly;

namespace Syncano4.Tests.Shared
{
public  class SchemaMapperTests
    {

    public class SimpleClass:DataObject
    {
        public const string OrderNumberPropertyName = "order_number";

        [JsonProperty(OrderNumberPropertyName)]
        public int OrderNumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("is_checked")]
        public bool IsChecked { get; set; }
    }

   


    [Fact]
    public void MapSimpleClass()
    {
        var fields = SchemaMapping.GetSchema<SimpleClass>();

        fields.Count.ShouldBe(5);
        fields.ShouldContain(new FieldDef() { Name = SimpleClass.OrderNumberPropertyName, Type = FieldType.Integer});
    }

    }
}
