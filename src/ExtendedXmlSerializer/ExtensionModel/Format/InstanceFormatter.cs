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
using System.IO;

namespace ExtendedXmlSerializer.ExtensionModel.Format
{
	class InstanceFormatter<TRead, TWrite> : IInstanceFormatter where TWrite : IDisposable
	{
		readonly ISerializer<TRead, TWrite> _serializer;
		readonly IWriterFactory<TWrite> _factory;
		readonly Func<Stream> _stream;

		public InstanceFormatter(ISerializer<TRead, TWrite> serializer, IWriterFactory<TWrite> factory, Func<Stream> stream)
		{
			_serializer = serializer;
			_factory = factory;
			_stream = stream;
		}

		public string Get(object parameter)
		{
			var stream = _stream();
			using (var writer = _factory.Get(stream))
			{
				_serializer.Serialize(writer, parameter);
				stream.Seek(0, SeekOrigin.Begin);
				var result = new StreamReader(stream).ReadToEnd();
				return result;
			}
		}
	}
}