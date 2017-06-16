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

using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using ExtendedXmlSerializer.ContentModel;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using JetBrains.Annotations;
using IContents = ExtendedXmlSerializer.ContentModel.Content.IContents;

namespace ExtendedXmlSerializer.ExtensionModel.Format.Xml
{
	sealed class CustomXmlExtension : TableSource<TypeInfo, IExtendedXmlCustomSerializer>, ISerializerExtension
	{
		[UsedImplicitly]
		public CustomXmlExtension() : this(new Dictionary<TypeInfo, IExtendedXmlCustomSerializer>()) {}

		public CustomXmlExtension(IDictionary<TypeInfo, IExtendedXmlCustomSerializer> store) : base(store) {}

		public IServiceRepository Get(IServiceRepository parameter) => parameter.Decorate<IContents>(Register);

		IContents Register(IServiceProvider _, IContents contents) => new Contents(this, contents);

		void ICommand<IServices>.Execute(IServices parameter) {}

		sealed class Contents : IContents
		{
			readonly IParameterizedSource<TypeInfo, IExtendedXmlCustomSerializer> _custom;
			readonly IContents _contents;

			public Contents(IParameterizedSource<TypeInfo, IExtendedXmlCustomSerializer> custom, IContents contents)
			{
				_custom = custom;
				_contents = contents;
			}

			public ISerializer Get(TypeInfo parameter)
			{
				var custom = _custom.Get(parameter);
				var result = custom != null ? new Serializer(custom) : _contents.Get(parameter);
				return result;
			}

			sealed class Serializer : ISerializer
			{
				readonly IExtendedXmlCustomSerializer _custom;

				public Serializer(IExtendedXmlCustomSerializer custom)
				{
					_custom = custom;
				}

				public object Get(IFormatReader parameter)
					=> _custom.Deserialize(XElement.Load(parameter.Get().AsValid<System.Xml.XmlReader>()));

				public void Write(IFormatWriter writer, object instance)
					=> _custom.Serializer(writer.Get().AsValid<System.Xml.XmlWriter>(), instance);
			}
		}
	}
}