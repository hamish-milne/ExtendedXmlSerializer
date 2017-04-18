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

namespace ExtendedXmlSerializer.ContentModel.Content.Composite.Members
{
	sealed class CompositeContent : IContent
	{
		readonly IMemberSerializations _members;
		readonly IInnerContents _services;

		public CompositeContent(IMemberSerializations members, IInnerContents services)
		{
			_members = members;
			_services = services;
		}

		public ISerializer Get(TypeInfo parameter)
		{
			var members = _members.Get(parameter);
			var reader = new InnerContentReader(
				_services.Get(parameter),
				new InnerContentCommand<object>(members, _services, _services),
				_services
			);
			var writer = new MemberListWriter(members);
			var result = new Serializer(reader, writer);
			return result;
		}

		/*sealed class Adapter : Generic<IMemberSerializations, IInnerContents, ISource<ISerializer>>
		{
			public static Adapter Default { get; } = new Adapter();
			Adapter() : base(typeof(Source<>)) {}
		}

		sealed class Source<T> : Serializer, ISource<ISerializer<T>>
		{
			readonly static TypeInfo TypeInfo = Support<T>.Key;

			readonly IMemberSerializations _members;
			readonly IInnerContents _services;
			readonly TypeInfo _type;

			public Source(IMemberSerializations members, IInnerContents services) : this(members, services, TypeInfo) {}

			Source(IMemberSerializations members, IInnerContents services, TypeInfo type)
			{
				_members = members;
				_services = services;
				_type = type;
			}

			public ISerializer<T> Get()
			{
				var members = _members.Get(_type);
				var reader = new InnerContentReader<T>(
					_services.Get(_type),
					new MemberInnerContentHandler(members, _services, _services),
					_services
				);
				var writer = new MemberListWriter<T>(members);
				var result = new Serializer<T>(reader, writer);
				return result;
			}
		}*/
	}
}