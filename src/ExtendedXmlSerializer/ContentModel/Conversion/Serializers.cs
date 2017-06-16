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
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ContentModel.Conversion
{
	sealed class Serializers : ISerializers
	{
		readonly IConverters _converters;
		readonly IGenericAdapter<IConverter, ISerializer> _activators;

		public Serializers(IConverters converters) : this(converters, Adapter.Default) {}

		public Serializers(IConverters converters, IGenericAdapter<IConverter, ISerializer> activators)
		{
			_converters = converters;
			_activators = activators;
		}

		public ISerializer Get(TypeInfo parameter) => _activators.Get(parameter).Invoke(_converters.Get(parameter));

		sealed class Adapter : GenericAdapter<IConverter, ISerializer>
		{
			public static Adapter Default { get; } = new Adapter();
			Adapter() : base(typeof(Serializer<>)) {}

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
				readonly IConverter<T> _serializer;

				public Serializer(IConverter<T> serializer)
				{
					_serializer = serializer;
				}

				public T Get(IFormatReader parameter)
				{
					throw new NotImplementedException();
				}

				public void Write(IFormatWriter writer, T instance) => writer.Content(_serializer.Format(instance));
			}
		}
	}
}