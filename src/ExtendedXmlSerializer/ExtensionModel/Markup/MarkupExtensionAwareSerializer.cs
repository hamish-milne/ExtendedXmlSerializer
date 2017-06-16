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
using ExtendedXmlSerializer.ContentModel;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sprache;

namespace ExtendedXmlSerializer.ExtensionModel.Markup
{
	sealed class MarkupExtensionAwareSerializer<T> : ISerializer<T>
	{
		readonly Parser<MarkupExtensionParts> _parser;
		readonly IMarkupExtensions _container;
		readonly ISerializer<T> _serializer;

		public MarkupExtensionAwareSerializer(IMarkupExtensions container, ISerializer<T> serializer)
			: this(MarkupExtensionParser.Default, container, serializer) {}

		public MarkupExtensionAwareSerializer(Parser<MarkupExtensionParts> parser, IMarkupExtensions container,
		                                      ISerializer<T> serializer)
		{
			_parser = parser;
			_container = container;
			_serializer = serializer;
		}

		public T Get(IFormatReader parameter)
		{
			try
			{
				return _serializer.Get(parameter);
			}
			catch (Exception)
			{
				var parts = _parser.TryParse(parameter.Content());
				if (parts.WasSuccessful)
				{
					var result = _container.Get(parameter)
					                       .Get(parts.Value).AsValid<T>();
					return result;
				}
				throw;
			}
		}

		public void Write(IFormatWriter writer, T instance) => _serializer.Write(writer, instance);
	}
}