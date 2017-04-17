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

using System;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.Core.Sources;

namespace ExtendedXmlSerializer.ContentModel.Conversion
{
	class Serializer : ISerializer
	{
		object IParameterizedSource<IFormatReader, object>.Get(IFormatReader parameter)
		{
			throw new InvalidOperationException("This exists for static type checking purposes only.");
		}

		void IWriter<object>.Write(IFormatWriter writer, object instance)
		{
			throw new InvalidOperationException("This exists for static type checking purposes only.");
		}
	}

	sealed class Serializer<T> : Serializer, ISerializer<T>
	{
		readonly IConverter<T> _converter;

		public Serializer(IConverter<T> converter)
		{
			_converter = converter;
		}

		public T Get(IFormatReader parameter) => _converter.Parse(parameter.Content());

		public void Write(IFormatWriter writer, T instance) => writer.Content(_converter.Format(instance));
	}
}