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
using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.Core.Sources;
using JetBrains.Annotations;

namespace ExtendedXmlSerializer.ExtensionModel.Format.Xml
{
	sealed class XmlInnerContentActivation : IInnerContentActivation
	{
		readonly IActivation _activation;
		readonly IParameterizedSource<TypeInfo, IXmlContentsActivator> _activator;

		[UsedImplicitly]
		public XmlInnerContentActivation(IActivation activation) : this(activation, XmlContentsActivatorSelector.Default) {}

		public XmlInnerContentActivation(IActivation activation, IParameterizedSource<TypeInfo, IXmlContentsActivator> activator)
		{
			_activation = activation;
			_activator = activator;
		}

		public IInnerContentActivator Get(TypeInfo parameter)
			=> new XmlInnerContentActivator(_activation.Get(parameter), _activator.Get(parameter));
	}
}