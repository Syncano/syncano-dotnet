using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Syncano4.Shared;
using Xunit;
using Shouldly;
using Syncano4.Shared.Serialization;
using Xunit.Abstractions;

namespace Syncano4.Tests.Shared
{
    public class SchemaMapperTests
    {
        private readonly ITestOutputHelper _testOutput;

        public SchemaMapperTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        public class SimpleClass : DataObject
        {
            public const string OrderNumberPropertyName = "order_number";
            public const string NamePropertyName = "name";
            public const string DatePropertyName = "date";
            public const string ValuePropertyName = "value";
            public const string IsCheckedPropertyName = "is_checked";

            [SyncanoField(Name = OrderNumberPropertyName)]
            public int OrderNumber { get; set; }

            [SyncanoField(Name = NamePropertyName)]
            public string Name { get; set; }

            [SyncanoField(Name = DatePropertyName)]
            public DateTime Date { get; set; }

            [SyncanoField(Name = ValuePropertyName)]
            public float Value { get; set; }

            [SyncanoField(Name = IsCheckedPropertyName)]
            public bool IsChecked { get; set; }
        }

        [Fact]
        public void MapSimpleClass()
        {
            var fields = SchemaMapping.GetSchema<SimpleClass>();

            fields.Count.ShouldBe(5);
            fields.ShouldContain(new FieldDef() {Name = SimpleClass.OrderNumberPropertyName, Type = FieldType.Integer});
            fields.ShouldContain(new FieldDef() {Name = SimpleClass.NamePropertyName, Type = FieldType.String});
            fields.ShouldContain(new FieldDef() {Name = SimpleClass.DatePropertyName, Type = FieldType.Datetime});
            fields.ShouldContain(new FieldDef() {Name = SimpleClass.ValuePropertyName, Type = FieldType.Float});
            fields.ShouldContain(new FieldDef() {Name = SimpleClass.IsCheckedPropertyName, Type = FieldType.Boolean});
        }

        [Fact]
        public void Serialize()
        {
            var simpleClass = new SimpleClass() {Name = "ABC", OrderNumber = 1, Date = DateTime.Today};


            string json = SyncanoJsonConverter.Serialize(simpleClass);


            _testOutput.WriteLine(json);
            json.ShouldContain("ABC");
        }
    }
}