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

using System.Collections.Generic;
using System.Reflection;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ContentModel.Content
{
	sealed class RecursionAwareContents : Generic<IContents, ISerializer>, IContents
	{
		readonly IContents _contents;
		readonly ISet<TypeInfo> _types;

		public RecursionAwareContents(IContents contents) : this(contents, new HashSet<TypeInfo>()) {}

		public RecursionAwareContents(IContents contents, ISet<TypeInfo> types) : base(typeof(Serializer<>))
		{
			_contents = contents;
			_types = types;
		}

		public new ISerializer Get(TypeInfo parameter)
		{
			var result = _types.Add(parameter)
				? _contents.Get(parameter)
				: base.Get(parameter).Invoke(_contents);
			_types.Remove(parameter);
			return result;
		}

		sealed class Serializer<T> : GenericSerializerAdapter<T>
		{
			public Serializer(IContents contents) : base(new DeferredSerializer<T>(new Contents(contents).Build(Support<T>.Key))) {}

			sealed class Contents : IParameterizedSource<TypeInfo, ISerializer<T>>
			{
				readonly IContents _contents;

				public Contents(IContents contents)
				{
					_contents = contents;
				}

				public ISerializer<T> Get(TypeInfo parameter) => _contents.Get(parameter).AsValid<ISerializer<T>>();
			}
		}
	}
}