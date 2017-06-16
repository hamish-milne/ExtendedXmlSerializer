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
using ExtendedXmlSerializer.Core.Specifications;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ContentModel.Conversion
{
	public abstract class ConverterBase<T> : DecoratedSpecification<TypeInfo>, IConverter<T>, IGenericAware
	{
		readonly protected static TypeEqualitySpecification<T> Specification = TypeEqualitySpecification<T>.Default;

		protected ConverterBase() : this(Specification) {}

		protected ConverterBase(ISpecification<TypeInfo> specification) : base(specification) {}

		public abstract T Parse(string data);

		public abstract string Format(T instance);

		public TypeInfo Get() => Support<T>.Key;
	}

	public abstract class ConverterMarker : IConverter
	{
		readonly ISpecification<TypeInfo> _specification;

		protected ConverterMarker(ISpecification<TypeInfo> specification)
		{
			_specification = specification;
		}

		public bool IsSatisfiedBy(TypeInfo parameter) => _specification.IsSatisfiedBy(parameter);

		object IConverter<object>.Parse(string data)
		{
			throw new InvalidOperationException("This exists for static type checking purposes only.");
		}

		string IConverter<object>.Format(object instance)
		{
			throw new InvalidOperationException("This exists for static type checking purposes only.");
		}
	}
}