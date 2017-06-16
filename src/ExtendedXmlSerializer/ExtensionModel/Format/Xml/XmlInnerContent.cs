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
using System.Collections;
using ExtendedXmlSerializer.ContentModel.Content.Composite;
using ExtendedXmlSerializer.ContentModel.Format;

namespace ExtendedXmlSerializer.ExtensionModel.Format.Xml
{
	sealed class XmlInnerContent<T> : IInnerContent<T>
	{
		readonly IFormatReader _reader;
		readonly XmlContent _content;

		public XmlInnerContent(IFormatReader reader, T current, XmlContent content)
		{
			Current = current;
			_reader = reader;
			_content = content;
		}

		public T Current { get; }

		public IFormatReader Get() => _reader;

		public bool MoveNext()
		{
			var contents = _content;
			return contents.MoveNext();
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}

		object IEnumerator.Current => Current;

		public void Dispose() => Reset();
	}
}