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

using System.Text;
using ExtendedXmlSerializer.ContentModel.Collections;
using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Specifications;
using ExtendedXmlSerializer.ReflectionModel;
using Newtonsoft.Json;

namespace ExtendedXmlSerializer.ExtensionModel.Format.Json
{
	sealed class JsonFormatExtension : IFormatExtension
	{
		readonly static ISpecification<IInnerContent> Specification =
			IsTypeSpecification<IListInnerContent>.Default.And(ElementSpecification.Default);

		readonly ISpecification<IInnerContent> _specification;

		public JsonFormatExtension() : this(Specification) {}

		public JsonFormatExtension(ISpecification<IInnerContent> specification)
		{
			_specification = specification;
		}

		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.RegisterInstance(Encoding.UTF8)
			            .RegisterInstance<IIdentifierFormatter>(IdentifierFormatter.Default)
			            .RegisterInstance<IReaderFormatter>(ReaderFormatter.Default)
			            .RegisterInstance<IFormattedContentSpecification>(FormattedContentSpecification.Default)
			            /*.RegisterInstance<IListContentsSpecification>(new ListContentsSpecification(_specification))*/
			            /*.Register<IInnerContentActivation, XmlInnerContentActivation>()*/
			            .Register<IFormatWriters<JsonTextWriter>, FormatWriterContext>()
		/*.Register<IFormatReaderContexts<XmlNameTable>, FormatReaderContexts>()
		.Register<IFormatReaders<System.Xml.XmlReader>, FormatReaders>()*/;

		void ICommand<IServices>.Execute(IServices parameter) {}
	}
}