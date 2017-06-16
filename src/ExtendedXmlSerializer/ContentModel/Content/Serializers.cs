﻿// MIT License
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

using System;
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ReflectionModel;
using JetBrains.Annotations;

namespace ExtendedXmlSerializer.ContentModel.Content
{
	[UsedImplicitly]
	sealed class Serializers : GenericAdapter<IWriter, ISerializer, ISerializer>, ISerializers
	{
		readonly IElements _elements;
		readonly IContents _contents;

		public Serializers(IElements elements, IContents contents) : base(typeof(Serializer<>))
		{
			_elements = elements;
			_contents = contents;
		}

		public new ISerializer Get(TypeInfo parameter)
		{
			var type = parameter.AccountForNullable();
			var result = base.Get(parameter).Invoke(_elements.Get(type), _contents.Get(type));
			return result;
		}

		sealed class Serializer<T> : ContentModel.Serializer<T>, ISerializer
		{
			public Serializer(IWriter<T> element, ISerializer<T> content) : base(content, new Enclosure<T>(element, content)) {}

			object IParameterizedSource<IFormatReader, object>.Get(IFormatReader parameter)
			{
				throw new InvalidOperationException("This exists for static type checking purposes only.");
			}

			void IWriter<object>.Write(IFormatWriter writer, object instance)
			{
				throw new InvalidOperationException("This exists for static type checking purposes only.");
			}
		}
	}
}