// MIT License
//
// Copyright (c) 2016 Wojciech Nagórski
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

using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ContentModel.Content.Composite.Collections
{
	sealed class ArrayContent : IContent
	{
		readonly static CollectionItemTypeLocator Locator = CollectionItemTypeLocator.Default;
		/*readonly static IsArraySpecification Specification = IsArraySpecification.Default;*/

		readonly IInnerContents _contents;
		readonly IEnumerators _enumerators;
		readonly ISerializers _serializers;
		readonly IClassification _classification;
		readonly ICollectionItemTypeLocator _locator;

		public ArrayContent(IInnerContents contents, IEnumerators enumerators, ISerializers serializers,
		                          IClassification classification)
			: this(contents, enumerators, serializers, classification, Locator) {}

		public ArrayContent(IInnerContents contents, IEnumerators enumerators, ISerializers serializers,
		                          IClassification classification, ICollectionItemTypeLocator locator)
		{
			_contents = contents;
			_enumerators = enumerators;
			_serializers = serializers;
			_classification = classification;
			_locator = locator;
		}

		public ISerializer Get(TypeInfo parameter)
		{
			var itemType = _locator.Get(parameter);
			var item = _serializers.Get(itemType);
			var reader = new ArrayReader(_contents, _classification, item);
			var writer = new EnumerableWriter<object>(_enumerators, item).Adapt();
			var result = new Serializer(reader, writer);
			return result;
		}
	}
}