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
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ContentModel.Content
{
	sealed class ElementOption : NamedElementOptionBase
	{
		readonly IGenericAdapter<IIdentity, IWriter> _adapter;

		public ElementOption(IIdentities identities) : this(identities, Adapter.Default) {}

		public ElementOption(IIdentities identities, IGenericAdapter<IIdentity, IWriter> adapter) : base(identities)
		{
			_adapter = adapter;
		}

		public override IWriter Create(IIdentity identity, TypeInfo classification)
			=> _adapter.Get(classification).Invoke(identity);

		sealed class Adapter : GenericAdapter<IIdentity, IWriter>
		{
			public static Adapter Default { get; } = new Adapter();
			Adapter() : base(typeof(Writer<>)) {}

			sealed class Writer<T> : Element<T>, IWriter
			{
				public Writer(IIdentity identity) : base(identity) {}

				void IWriter<object>.Write(IFormatWriter writer, object instance)
				{
					throw new InvalidOperationException("This exists for static type checking purposes only.");
				}
			}
		}
	}
}