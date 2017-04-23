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

using System.Reflection;
using ExtendedXmlSerializer.Core.Specifications;
using ExtendedXmlSerializer.ExtensionModel.References;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ExtensionModel.Roots
{
	sealed class Serializer<TParameter, TInstance> : ISerialize<TParameter, TInstance>
	{
		readonly ISpecification<TypeInfo> _specification;
		readonly IRoots _roots;
		readonly ISerialize<TParameter, TInstance> _serialize;

		public Serializer(IRoots roots, ISerialize<TParameter, TInstance> serialize)
			: this(IsReferenceSpecification.Default, roots, serialize) {}

		public Serializer(ISpecification<TypeInfo> specification, IRoots roots, ISerialize<TParameter, TInstance> serialize)
		{
			_specification = specification;
			_roots = roots;
			_serialize = serialize;
		}

		public void Serialize(TParameter writer, TInstance instance)
		{
			if (_specification.IsSatisfiedBy(Support<TInstance>.Key))
			{
				_roots.Assign(writer, instance);
			}

			_serialize.Serialize(writer, instance);
		}
	}
}