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
using ExtendedXmlSerializer.ContentModel.Content.Composite.Members;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ContentModel.Content.Composite.Collections
{
	sealed class CollectionContentOption : DelegatedContentOption
	{
		public CollectionContentOption(IActivatingTypeSpecification specification, ICompositeCollectionContents contents)
			: base(specification, contents.Get) {}
	}

	public interface ICompositeCollectionContents : ISerializerSource {}

	sealed class CompositeCollectionContents : ICompositeCollectionContents
	{
		readonly ISerializers _serializers;
		readonly IMemberSerializations _members;
		readonly IInnerContents _contents;
		readonly IEnumerators _enumerators;

		public CompositeCollectionContents(ISerializers serializers, IMemberSerializations members, IInnerContents contents,
		                                   IEnumerators enumerators)
		{
			_serializers = serializers;
			_members = members;
			_contents = contents;
			_enumerators = enumerators;
		}

		public ISerializer Get(TypeInfo parameter)
		{
			var item = _serializers.Get(parameter);

			var members = _members.Get(parameter);
			var handler = new CompositeCollectionInnerContentCommand<object>(
				_contents,
				new InnerContentCommand<object>(members, _contents, _contents),
				new CollectionInnerContentCommand<object>(item, _contents));
			var reader = new InnerContentReader(_contents.Get(parameter), handler, _contents);
			var writer = new CompositeCollectionWriter<object>(new MemberListWriter(members),
			                                                   new EnumerableWriter<object>(_enumerators, item).Adapt());
			var result = new Serializer(reader, writer);
			return result;
		}
	}
}