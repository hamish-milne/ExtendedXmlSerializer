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
		readonly ISerializers<System.Xml.XmlReader, System.Xml.XmlWriter> _serializers;
		readonly IFormatWriters<System.Xml.XmlWriter> _writers;
		readonly ISerializerStore _store;

		public ExtendedXmlSerializer(ISerializers<System.Xml.XmlReader, System.Xml.XmlWriter> serializers, IFormatWriters<System.Xml.XmlWriter> writers, ISerializerStore store)
		{
			_serializers = serializers;
			_writers = writers;
			_store = store;
		}

		/*public ISerialize<System.Xml.XmlWriter, TInstance> Get<TInstance>()
			=> new Serializer<TInstance>(_writers, _store.Get<TInstance>());*/

		/*sealed class Serializer<T> : ISerialize<System.Xml.XmlWriter, T>
		{
			readonly IWriter<T> _writer;
			readonly IFormatWriters<System.Xml.XmlWriter> _source;

			public Serializer(IFormatWriters<System.Xml.XmlWriter> source, IWriter<T> writer)
			{
				_writer = writer;
				_source = source;
			}

			public void Serialize(System.Xml.XmlWriter writer, T instance)
			{
				_writer.Write(_source.Get(writer), instance);
				writer.Flush();
			}
		}*/
		public ISerializer<System.Xml.XmlReader, System.Xml.XmlWriter, TInstance> Get<TInstance>()
			=> _serializers.Get<TInstance>();

		public ISerialize<System.Xml.XmlWriter, int> Get() => new Serialize<System.Xml.XmlWriter, int>(_writers, _store.Get<int>());
	}
}