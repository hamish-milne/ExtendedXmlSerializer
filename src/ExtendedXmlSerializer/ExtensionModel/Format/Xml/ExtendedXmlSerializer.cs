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

using ExtendedXmlSerializer.ContentModel;
using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Format;

namespace ExtendedXmlSerializer.ExtensionModel.Format.Xml
{
/*
	sealed class ExtendedXmlSerializer : IExtendedXmlSerializer
	{
		readonly ISerialize<System.Xml.XmlWriter> _serialize;

		public ExtendedXmlSerializer(ISerialize<System.Xml.XmlWriter> serialize)
		{
			_serialize = serialize;
		}

		public void Serialize<TInstance>(System.Xml.XmlWriter parameter, TInstance instance)
		{
			_serialize.Serialize(parameter, instance);
			parameter.Flush();
		}
	}
*/

	sealed class ExtendedXmlSerializer : IExtendedXmlSerializer
	{
		readonly IFormatWriters<System.Xml.XmlWriter> _writers;
		readonly ISerializerStore _store;

		public ExtendedXmlSerializer(IFormatWriters<System.Xml.XmlWriter> writers, ISerializerStore store)
		{
			_writers = writers;
			_store = store;
		}

		/*sealed class Serializer<T> : IWriter<T>
		{
			readonly ISerializer<T> _serializer;
			readonly IFormatWriters<System.Xml.XmlWriter> _writers;

			public Serializer(ISerializer<T> serializer, IFormatWriters<System.Xml.XmlWriter> writers)
			{
				_serializer = serializer;
				_writers = writers;
			}

			public void Serialize<TInstance>(System.Xml.XmlWriter parameter, TInstance instance)
			{
				_serializer.Write(_writers.Get(parameter), instance);
			}

			public void Write(IFormatWriter writer, T instance)
			{
				throw new System.NotImplementedException();
			}
		}*/

		public IWriter<T> For<T>() => _store.Get<T>();

		public void Serialize<TInstance>(System.Xml.XmlWriter parameter, TInstance instance)
		{
			_store.Get<TInstance>()
			      .Write(_writers.Get(parameter), instance);
			parameter.Flush();
		}

		public IFormatWriter Get(System.Xml.XmlWriter parameter) => _writers.Get(parameter);
	}
}