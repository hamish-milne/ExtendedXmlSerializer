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

using System.Reflection;
using ExtendedXmlSerialization.ContentModel.Xml;
using ExtendedXmlSerialization.Core.Sources;
using ExtendedXmlSerialization.TypeModel;

namespace ExtendedXmlSerialization.ContentModel.Properties
{
	abstract class TypePropertyBase : FrameworkPropertyBase<TypeInfo>, ITypeProperty
	{
		readonly IParameterizedSource<IXmlReader, ITypeParser> _parsers;
		readonly IParameterizedSource<IXmlWriter, ITypeFormatter> _formatters;
		protected TypePropertyBase(string displayName) : this(displayName, TypeParser.Defaults, TypeFormatter.Defaults) {}

		protected TypePropertyBase(string displayName, IParameterizedSource<IXmlReader, ITypeParser> parsers,
		                           IParameterizedSource<IXmlWriter, ITypeFormatter> formatters) : base(displayName)
		{
			_parsers = parsers;
			_formatters = formatters;
		}

		protected sealed override string Format(IXmlWriter writer, TypeInfo instance) => _formatters.Get(writer).Get(instance);

		protected sealed override TypeInfo Parse(IXmlReader parameter, string data) => _parsers.Get(parameter).Get(data);
	}
}