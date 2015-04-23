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
        public const string NamePropertyName = "name";
        public const string DatePropertyName = "date";
        public const string ValuePropertyName = "value";
        public const string IsCheckedPropertyName = "is_checked";

        [JsonProperty(OrderNumberPropertyName)]
        public int OrderNumber { get; set; }

        [JsonProperty(NamePropertyName)]
        public string Name { get; set; }

        [JsonProperty(DatePropertyName)]
        public DateTime Date { get; set; }

        [JsonProperty(ValuePropertyName)]
        public float Value { get; set; }

        [JsonProperty(IsCheckedPropertyName)]
        public bool IsChecked { get; set; }
    }

   


    [Fact]
    public void MapSimpleClass()
    {
        var fields = SchemaMapping.GetSchema<SimpleClass>();

        fields.Count.ShouldBe(5);
        fields.ShouldContain(new FieldDef() { Name = SimpleClass.OrderNumberPropertyName, Type = FieldType.Integer});
        fields.ShouldContain(new FieldDef() { Name = SimpleClass.NamePropertyName, Type = FieldType.String });
        fields.ShouldContain(new FieldDef() { Name = SimpleClass.DatePropertyName, Type = FieldType.Datetime });
        fields.ShouldContain(new FieldDef() { Name = SimpleClass.ValuePropertyName, Type = FieldType.Float });
        fields.ShouldContain(new FieldDef() { Name = SimpleClass.IsCheckedPropertyName, Type = FieldType.Boolean });
    }

    }
}
