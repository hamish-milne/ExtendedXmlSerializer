// MIT License
//
// Copyright (c) 2016 Wojciech Nag�rski
//                    Michael DeMond
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using ExtendedXmlSerializer.ExtensionModel.Format.Xml;
using ExtendedXmlSerializer.Tests.Support;
using ExtendedXmlSerializer.Tests.TestObject;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ExtensionModel.Format.Xml
{
	public class ExtendedXmlSerializerTests
	{
		const string HelloWorld = "Hello World!";
		readonly IExtendedXmlSerializer _serializer = new ConfigurationContainer().Create();

		[Fact]
		public void Primitive()
		{
			const int expected = 6776;

			var data = _serializer.Serialize(expected);
			Assert.Equal(
				@"<?xml version=""1.0"" encoding=""utf-8""?><int xmlns=""https://extendedxmlserializer.github.io/system"">6776</int>",
				data);
			var actual = _serializer.Deserialize<int>(data);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void SimpleInstance()
		{
			var instance = new SimpleNestedClass {PropertyName = HelloWorld};
			var data = _serializer.Serialize(instance);
			Assert.Equal(
				@"<?xml version=""1.0"" encoding=""utf-8""?><ExtendedXmlSerializerTests-SimpleNestedClass xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.Format.Xml;assembly=ExtendedXmlSerializer.Tests""><PropertyName>Hello World!</PropertyName></ExtendedXmlSerializerTests-SimpleNestedClass>",
				data);
			var read = _serializer.Deserialize<SimpleNestedClass>(data);
			Assert.NotNull(read);
			Assert.Equal(HelloWorld, read.PropertyName);
		}

		[Fact]
		public void ComplexInstance()
		{
			const string expected =
				@"<?xml version=""1.0"" encoding=""utf-8""?><TestClassOtherClass xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.TestObject;assembly=ExtendedXmlSerializer.Tests""><Other><Test><Id>2</Id><Name>Other Name</Name></Test><Double>7.3453145324</Double></Other><Primitive1><PropString>TestString</PropString><PropInt>-1</PropInt><PropuInt>2234</PropuInt><PropDecimal>3.346</PropDecimal><PropDecimalMinValue>-79228162514264337593543950335</PropDecimalMinValue><PropDecimalMaxValue>79228162514264337593543950335</PropDecimalMaxValue><PropFloat>7.4432</PropFloat><PropFloatNaN>NaN</PropFloatNaN><PropFloatPositiveInfinity>INF</PropFloatPositiveInfinity><PropFloatNegativeInfinity>-INF</PropFloatNegativeInfinity><PropFloatMinValue>-3.40282347E+38</PropFloatMinValue><PropFloatMaxValue>3.40282347E+38</PropFloatMaxValue><PropDouble>3.4234</PropDouble><PropDoubleNaN>NaN</PropDoubleNaN><PropDoublePositiveInfinity>INF</PropDoublePositiveInfinity><PropDoubleNegativeInfinity>-INF</PropDoubleNegativeInfinity><PropDoubleMinValue>-1.7976931348623157E+308</PropDoubleMinValue><PropDoubleMaxValue>1.7976931348623157E+308</PropDoubleMaxValue><PropEnum>EnumValue1</PropEnum><PropLong>234234142</PropLong><PropUlong>2345352534</PropUlong><PropShort>23</PropShort><PropUshort>2344</PropUshort><PropDateTime>2014-01-23T00:00:00</PropDateTime><PropByte>23</PropByte><PropSbyte>33</PropSbyte><PropChar>g</PropChar></Primitive1><Primitive2><PropString>TestString</PropString><PropInt>-1</PropInt><PropuInt>2234</PropuInt><PropDecimal>3.346</PropDecimal><PropDecimalMinValue>-79228162514264337593543950335</PropDecimalMinValue><PropDecimalMaxValue>79228162514264337593543950335</PropDecimalMaxValue><PropFloat>7.4432</PropFloat><PropFloatNaN>NaN</PropFloatNaN><PropFloatPositiveInfinity>INF</PropFloatPositiveInfinity><PropFloatNegativeInfinity>-INF</PropFloatNegativeInfinity><PropFloatMinValue>-3.40282347E+38</PropFloatMinValue><PropFloatMaxValue>3.40282347E+38</PropFloatMaxValue><PropDouble>3.4234</PropDouble><PropDoubleNaN>NaN</PropDoubleNaN><PropDoublePositiveInfinity>INF</PropDoublePositiveInfinity><PropDoubleNegativeInfinity>-INF</PropDoubleNegativeInfinity><PropDoubleMinValue>-1.7976931348623157E+308</PropDoubleMinValue><PropDoubleMaxValue>1.7976931348623157E+308</PropDoubleMaxValue><PropEnum>EnumValue1</PropEnum><PropLong>234234142</PropLong><PropUlong>2345352534</PropUlong><PropShort>23</PropShort><PropUshort>2344</PropUshort><PropDateTime>2014-01-23T00:00:00</PropDateTime><PropByte>23</PropByte><PropSbyte>33</PropSbyte><PropChar>g</PropChar></Primitive2><ListProperty><Capacity>32</Capacity><TestClassItem><Id>0</Id><Name>Name 000</Name></TestClassItem><TestClassItem><Id>1</Id><Name>Name 001</Name></TestClassItem><TestClassItem><Id>2</Id><Name>Name 002</Name></TestClassItem><TestClassItem><Id>3</Id><Name>Name 003</Name></TestClassItem><TestClassItem><Id>4</Id><Name>Name 004</Name></TestClassItem><TestClassItem><Id>5</Id><Name>Name 005</Name></TestClassItem><TestClassItem><Id>6</Id><Name>Name 006</Name></TestClassItem><TestClassItem><Id>7</Id><Name>Name 007</Name></TestClassItem><TestClassItem><Id>8</Id><Name>Name 008</Name></TestClassItem><TestClassItem><Id>9</Id><Name>Name 009</Name></TestClassItem><TestClassItem><Id>10</Id><Name>Name 0010</Name></TestClassItem><TestClassItem><Id>11</Id><Name>Name 0011</Name></TestClassItem><TestClassItem><Id>12</Id><Name>Name 0012</Name></TestClassItem><TestClassItem><Id>13</Id><Name>Name 0013</Name></TestClassItem><TestClassItem><Id>14</Id><Name>Name 0014</Name></TestClassItem><TestClassItem><Id>15</Id><Name>Name 0015</Name></TestClassItem><TestClassItem><Id>16</Id><Name>Name 0016</Name></TestClassItem><TestClassItem><Id>17</Id><Name>Name 0017</Name></TestClassItem><TestClassItem><Id>18</Id><Name>Name 0018</Name></TestClassItem><TestClassItem><Id>19</Id><Name>Name 0019</Name></TestClassItem></ListProperty></TestClassOtherClass>";
			var instance = TestClassOtherClass.Create();
			var data = _serializer.Serialize(instance);

			Assert.Equal(expected, data);
			var read = _serializer.Deserialize<TestClassOtherClass>(data);
			Assert.NotNull(read);
			Assert.Equal(expected, _serializer.Serialize(read));
		}

		[Fact]
		public void BasicArray()
		{
			var expected = new[] {1, 2, 3, 4, 5};
			var data = _serializer.Serialize(expected);
			var actual = _serializer.Deserialize<int[]>(data);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void BasicList()
		{
			var expected = new List<string> {"Hello", "World", "Hope", "This", "Works!"};
			var data = _serializer.Serialize(expected);
			var actual = _serializer.Deserialize<List<string>>(data);
			actual.Capacity.Should().Be(8);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void BasicHashSet()
		{
			var expected = new HashSet<string> {"Hello", "World", "Hope", "This", "Works!"};
			var data = _serializer.Serialize(expected);
			var actual = _serializer.Deserialize<HashSet<string>>(data);
			Assert.True(actual.SetEquals(expected));
		}

		[Fact]
		public void Dictionary()
		{
			var expected = new Dictionary<int, string>{
				               {1, "First"},
				               {2, "Second"},
				               {3, "Other"}
			               };
			var data = _serializer.Serialize(expected);
			var actual = _serializer.Deserialize<Dictionary<int, string>>(data);
			Assert.NotNull(actual);
			Assert.Equal(expected.Count, actual.Count);
			foreach (var entry in actual)
			{
				Assert.Equal(expected[entry.Key], entry.Value);
			}
		}

		[Fact]
		public void Generic()
		{
			var expected = new TestClassGenericThree<string, int, TestClassPrimitiveTypes>();
			expected.Init("StringValue", 1, TestClassPrimitiveTypes.Create());

			var data = _serializer.Serialize(expected);
			var actual = _serializer.Deserialize<TestClassGenericThree<string, int, TestClassPrimitiveTypes>>(data);
			Assert.NotNull(actual);
			Assert.Equal(expected.GenericProp, actual.GenericProp);
			Assert.Equal(expected.GenericProp2, actual.GenericProp2);
			Assert.Equal(expected.GenericProp3.PropDateTime, actual.GenericProp3.PropDateTime);
			Assert.Equal(data, _serializer.Serialize(actual));
		}

	    [Fact]
	    public void GenericProperty()
	    {
	        var obj = new TestClassGenericThree<string, int, decimal> ();
	        obj.Init("StringValue", 1, 123.1m);

	        var expected = new TestClassPropGeneric() {PropGenric = obj};


            var data = _serializer.Serialize(expected);
	        var actual = _serializer.Deserialize<TestClassPropGeneric>(data);
	        Assert.NotNull(actual);
	        Assert.Equal(obj.GenericProp, actual.PropGenric.GenericProp);
	        Assert.Equal(obj.GenericProp2, actual.PropGenric.GenericProp2);
	        Assert.Equal(obj.GenericProp3, actual.PropGenric.GenericProp3);
	        Assert.Equal(data, _serializer.Serialize(actual));
	    }


        [Fact]
		public void GenericEnumeration()
		{
			var expected = new GenericSubject<TypeCode> { PropertyName = TypeCode.SByte};
			_serializer.Cycle(expected).ShouldBeEquivalentTo(expected);
		}

		[Fact]
		public void DifferingProperty()
		{
			const string message = "Hello World!  This is a value set in a property with a variable type.";
			var expected = new ClassWithDifferingPropertyType { Interface = new Implementation { PropertyName = message } };
			var data = _serializer.Serialize(expected);
			var actual = _serializer.Deserialize<ClassWithDifferingPropertyType>(data);
			var implementation = Assert.IsType<Implementation>(actual.Interface);
			Assert.Equal(message, implementation.PropertyName);
		}

		[Fact]
		public void Nullable()
		{
			var expected = new NullableSubject { Number =  6776 };
			var actual = new SerializationSupport().Assert(expected, @"<?xml version=""1.0"" encoding=""utf-8""?><ExtendedXmlSerializerTests-NullableSubject xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.Format.Xml;assembly=ExtendedXmlSerializer.Tests""><Number>6776</Number></ExtendedXmlSerializerTests-NullableSubject>");
			Assert.Equal(expected.Number, actual.Number);
		}


		[Fact]
		public void NullNullable()
		{
			var expected = new NullableSubject { Number = null };
			var actual = new SerializationSupport().Assert(expected, @"<?xml version=""1.0"" encoding=""utf-8""?><ExtendedXmlSerializerTests-NullableSubject xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.Format.Xml;assembly=ExtendedXmlSerializer.Tests"" />");
			Assert.Equal(expected.Number, actual.Number);
		}

	    [Fact]
	    public void Guid()
	    {
	        var expected = new GuidProperty { Guid = new Guid("7db85a35-1f66-4e5c-9c4a-33a937a9258b") };
            var actual = new SerializationSupport().Assert(expected, @"<?xml version=""1.0"" encoding=""utf-8""?><ExtendedXmlSerializerTests-GuidProperty xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.Xml;assembly=ExtendedXmlSerializer.Tests""><Guid>7db85a35-1f66-4e5c-9c4a-33a937a9258b</Guid></ExtendedXmlSerializerTests-GuidProperty>");
            Assert.Equal(expected.Guid, actual.Guid);
        }

#if CLASSIC
	    [Fact]
	    public void Point()
	    {
	        var expected = new PointProperty { Point = new System.Windows.Point(10, 20) };
            var actual = new SerializationSupport().Assert(expected, @"<?xml version=""1.0"" encoding=""utf-8""?><ExtendedXmlSerializerTests-PointProperty xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.Xml;assembly=ExtendedXmlSerializer.Tests""><Point><X>10</X><Y>20</Y></Point></ExtendedXmlSerializerTests-PointProperty>");
            Assert.Equal(expected.Point, actual.Point);
	    }
#endif

        [Fact]
        public void PropertyInterfaceOfList()
        {
            var expected = new ClassWithPropertyInterfaceOfList
            {
                List = new List<string> { "Item1" },
                Set = new HashSet<string> { "Item1" },
                Dictionary = new Dictionary<string, string> { { "Key", "Value" } }
            };
            
            var actual = new SerializationSupport().Assert(expected, @"<?xml version=""1.0"" encoding=""utf-8""?><ExtendedXmlSerializerTests-ClassWithPropertyInterfaceOfList xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.Xml;assembly=ExtendedXmlSerializer.Tests""></ExtendedXmlSerializerTests-ClassWithPropertyInterfaceOfList>");
            actual.ShouldBeEquivalentTo(expected);
        }

        [Fact]
	    public void CustomCollection()
        {
            var expected = new TestClassCollection { TestClassPrimitiveTypes.Create() };
            var actual = new SerializationSupport().Assert(expected, @"<?xml version=""1.0"" encoding=""utf-8""?><TestClassCollection xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.TestObject;assembly=ExtendedXmlSerializer.Tests""><TestClassPrimitiveTypes><PropString>TestString</PropString><PropInt>-1</PropInt><PropuInt>2234</PropuInt><PropDecimal>3.346</PropDecimal><PropDecimalMinValue>-79228162514264337593543950335</PropDecimalMinValue><PropDecimalMaxValue>79228162514264337593543950335</PropDecimalMaxValue><PropFloat>7.4432</PropFloat><PropFloatNaN>NaN</PropFloatNaN><PropFloatPositiveInfinity>INF</PropFloatPositiveInfinity><PropFloatNegativeInfinity>-INF</PropFloatNegativeInfinity><PropFloatMinValue>-3.40282347E+38</PropFloatMinValue><PropFloatMaxValue>3.40282347E+38</PropFloatMaxValue><PropDouble>3.4234</PropDouble><PropDoubleNaN>NaN</PropDoubleNaN><PropDoublePositiveInfinity>INF</PropDoublePositiveInfinity><PropDoubleNegativeInfinity>-INF</PropDoubleNegativeInfinity><PropDoubleMinValue>-1.7976931348623157E+308</PropDoubleMinValue><PropDoubleMaxValue>1.7976931348623157E+308</PropDoubleMaxValue><PropEnum>EnumValue1</PropEnum><PropLong>234234142</PropLong><PropUlong>2345352534</PropUlong><PropShort>23</PropShort><PropUshort>2344</PropUshort><PropDateTime>2014-01-23T00:00:00</PropDateTime><PropByte>23</PropByte><PropSbyte>33</PropSbyte><PropChar>g</PropChar></TestClassPrimitiveTypes></TestClassCollection>");
            actual.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void DifferentTypeOfProperty()
	    {
	        var expected = new TestClassPropertyType();
            expected.Init();
            _serializer.Cycle(expected).ShouldBeEquivalentTo(expected);
	    }

        [Fact]
        public void ClassWithHashSet()
	    {
	        var expected = new TestClassWithHashSet();
            expected.Init();
            _serializer.Cycle(expected).ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ClassTimeSpan()
	    {
	        var expected = new TestClassTimeSpan();
            expected.Init();
            _serializer.Cycle(expected).ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ClassWithArray()
        {
            var expected = new TestClassWithArray() {ArrayOfInt = new int[] {1, 2, 3}};
            _serializer.Cycle(expected).ShouldBeEquivalentTo(expected);
        }

	    [Fact]
	    public void ClassWithObjectProperty()
	    {
            var expected = new List<TestClassWithObjectProperty>
                      {
                          new TestClassWithObjectProperty {TestProperty = 1234},
                          new TestClassWithObjectProperty {TestProperty = "Abc"},
                          new TestClassWithObjectProperty {TestProperty = new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 2)}
                      };
            _serializer.Cycle(expected).ShouldBeEquivalentTo(expected);
        }

        class NullableSubject
		{
			public int? Number { get; set; }
		}

		class SimpleNestedClass
		{
			public string PropertyName { get; set; }
		}

		class ClassWithDifferingPropertyType
		{
			public IInterface Interface { get; set; }
		}

		class GenericSubject<T>
		{
			public T PropertyName { [UsedImplicitly] get; set; }
		}

		public interface IInterface {}

		class Implementation : IInterface
		{
			public string PropertyName { get; set; }
		}

        class GuidProperty
        {
            public Guid Guid { get; set; }
        }
#if CLASSIC
        class PointProperty
	    {
	        public System.Windows.Point Point { get; set; }
	    }
#endif

        class ClassWithPropertyInterfaceOfList
        {
            public IList<string> List { get; set; }
            public IDictionary<string, string> Dictionary { get; set; }

            public ISet<string> Set { get; set; }
        }
    }
}