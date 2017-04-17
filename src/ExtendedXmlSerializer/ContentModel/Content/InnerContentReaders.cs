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
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ContentModel.Content
{
	sealed class InnerContentReaders : IInnerContentReaders
	{
		readonly IInnerContentActivation _activation;
		readonly IAlteration<IInnerContentHandler> _alteration;
		readonly IInnerContentResult _results;

		public InnerContentReaders(IInnerContentActivation activation, IAlteration<IInnerContentHandler> alteration,
		                           IInnerContentResult results)
		{
			_activation = activation;
			_alteration = alteration;
			_results = results;
		}

		public Func<IInnerContentHandler, IReader> Get(TypeInfo parameter)
			=> Adapter.Default
			          .Get(parameter)
			          .Invoke(_activation.Get(parameter), _results)
			          .In(_alteration)
			          .Get;

		sealed class Adapter :
			Generic<IInnerContentActivator, IInnerContentResult, IParameterizedSource<IInnerContentHandler, IReader>>
		{
			public static Adapter Default { get; } = new Adapter();
			Adapter() : base(typeof(Factory<>)) {}
		}


		sealed class Factory<T> : IParameterizedSource<IInnerContentHandler, IReader<T>>
		{
			readonly IInnerContentActivator<T> _activator;
			readonly IInnerContentResult<T> _result;

			public Factory(IInnerContentActivator<T> activator, IInnerContentResult<T> result)
			{
				_activator = activator;
				_result = result;
			}

			public IReader<T> Get(IInnerContentHandler parameter) => new InnerContentReader<T>(_activator, parameter, _result);
		}
	}
}