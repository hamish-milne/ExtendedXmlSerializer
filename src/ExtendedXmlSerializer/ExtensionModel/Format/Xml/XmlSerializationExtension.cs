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

using System.Text;
using System.Xml;
using ExtendedXmlSerializer.ContentModel.Content.Composite;
using ExtendedXmlSerializer.ContentModel.Content.Composite.Collections;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Specifications;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ExtensionModel.Format.Xml
{
	sealed class XmlSerializationExtension : IFormatExtension
	{
		readonly static ISpecification<IInnerContent<object>> Specification =
			IsTypeSpecification<IListInnerContent>.Default.And(ElementSpecification.Default);

		readonly ISpecification<IInnerContent<object>> _specification;
		readonly XmlReaderSettings _reader;
		readonly XmlWriterSettings _writer;
		readonly XmlNameTable _names;

		public XmlSerializationExtension()
			: this(Specification, Defaults.ReaderSettings, Defaults.WriterSettings, new NameTable()) {}

		public XmlSerializationExtension(ISpecification<IInnerContent<object>> specification, XmlReaderSettings reader,
		                                 XmlWriterSettings writer, XmlNameTable names)
		{
			_specification = specification;
			_reader = reader;
			_writer = writer;
			_names = names;
		}

		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.RegisterInstance(Encoding.UTF8)
			            .RegisterInstance(_names)
			            .RegisterInstance(_reader.Clone())
			            .RegisterInstance(_writer.Clone())
			            .RegisterInstance<IIdentifierFormatter>(IdentifierFormatter.Default)
			            .RegisterInstance<IReaderFormatter>(ReaderFormatter.Default)
			            .RegisterInstance<IFormattedContentSpecification>(FormattedContentSpecification.Default)
			            .RegisterInstance<IListContentsSpecification>(new ListContentsSpecification(_specification))
			            //.Decorate<ISerializes<System.Xml.XmlWriter>, Serializes>()
			            .Register<IInnerContentActivation, XmlInnerContentActivation>()
			            .Register<IFormatWriters<System.Xml.XmlWriter>, FormatWriterContext>()
			            .Register<IFormatReaderContexts<XmlNameTable>, FormatReaderContexts>()
			            .Register<IFormatReaders<System.Xml.XmlReader>, FormatReaders>()
			            .Register<IExtendedXmlSerializer, ExtendedXmlSerializer>();

		void ICommand<IServices>.Execute(IServices parameter) {}
	}

/*
	sealed class Serializes : ISerializes<System.Xml.XmlWriter>
	{
		readonly ISerialize<System.Xml.XmlWriter> _serialize;

		public Serialize(ISerialize<System.Xml.XmlWriter> serialize)
		{
			_serialize = serialize;
		}

		void ISerialize<System.Xml.XmlWriter>.Serialize<TInstance>(System.Xml.XmlWriter parameter, TInstance instance)
		{
			_serialize.Serialize(parameter, instance);
			parameter.Flush();
		}

		public ISerialize<System.Xml.XmlWriter, TInstance> Get<TInstance>()
		{
			throw new System.NotImplementedException();
		}

		sealed class Serialize<T> : ISerialize<System.Xml.XmlWriter, T>
		{
			public Serialize() {}

			void ISerialize<System.Xml.XmlWriter, T>.Serialize(System.Xml.XmlWriter writer, T instance)
			{
				throw new System.NotImplementedException();
			}
		}
	}
*/
}