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

using System;
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Content.Composite.Collections;
using ExtendedXmlSerializer.ContentModel.Content.Composite.Members;
using ExtendedXmlSerializer.ContentModel.Format;

namespace ExtendedXmlSerializer.ContentModel.Content.Composite
{
	sealed class InnerContents<T> : IInnerContents<T>
	{
		readonly IListContentsSpecification _specification;
		readonly IInnerContentActivation _activation;
		readonly IMemberHandler<T> _member;
		readonly IContentsHandler<T> _collection;
		readonly IReaderFormatter _formatter;
		readonly IInnerContentResult<T> _result;

		public InnerContents(IListContentsSpecification specification, IInnerContentActivation activation,
		                     IMemberHandler<T> member, IContentsHandler<T> collection, IReaderFormatter formatter,
							 IInnerContentResult<T> result)
		{
			_specification = specification;
			_activation = activation;
			_member = member;
			_collection = collection;
			_formatter = formatter;
			_result = result;
		}

		public bool IsSatisfiedBy(IInnerContent<object> parameter) => _specification.IsSatisfiedBy(parameter);
		public IInnerContentActivator Get(TypeInfo parameter) => _activation.Get(parameter);

		public string Get(IFormatReader parameter) => _formatter.Get(parameter);

		public void Handle(IInnerContent<T> contents, IMemberSerializer member) => _member.Handle(contents, member);
		public void Handle(IListInnerContent contents, IReader<T> reader) => _collection.Handle(contents, reader);

		public T Get(IInnerContent<T> parameter) => _result.Get(parameter);
		public void Execute(IInnerContent<T> parameter)
		{
			throw new NotImplementedException();
		}
	}
}