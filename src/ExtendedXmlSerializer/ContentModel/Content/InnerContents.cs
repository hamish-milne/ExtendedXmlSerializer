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
using ExtendedXmlSerializer.ContentModel.Collections;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Members;

namespace ExtendedXmlSerializer.ContentModel.Content
{
	sealed class InnerContents : IInnerContents
	{
		readonly IListContentsSpecification _specification;
		readonly IMemberHandler _member;
		readonly ICollectionContentsHandler _collection;
		readonly IReaderFormatter _formatter;
		readonly IInnerContentReaders _readers;

		public InnerContents(IListContentsSpecification specification, IMemberHandler member,
		                     ICollectionContentsHandler collection, IReaderFormatter formatter,
		                     IInnerContentReaders readers)
		{
			_specification = specification;
			_member = member;
			_collection = collection;
			_formatter = formatter;
			_readers = readers;
		}

		public bool IsSatisfiedBy(IInnerContent parameter) => _specification.IsSatisfiedBy(parameter);

		public string Get(IFormatReader parameter) => _formatter.Get(parameter);

		public Func<IInnerContentHandler, IReader> Get(TypeInfo parameter) => _readers.Get(parameter);
		public void Handle(IInnerContent contents, IMemberSerializer member) => _member.Handle(contents, member);
		public void Handle(IListInnerContent contents, IReader reader) => _collection.Handle(contents, reader);
	}
}