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

using ExtendedXmlSerializer.ContentModel.Format;

namespace ExtendedXmlSerializer.ContentModel.Content.Composite
{
	sealed class InnerContentReader : DecoratedReader<object>, IReader
	{
		public InnerContentReader(IInnerContentActivator<object> activator, IInnerContentCommand<object> content, IInnerContentResult<object> result)
			: base(new InnerContentReader<object>(activator, content, result)) {}
	}

	sealed class InnerContentReader<T> : IReader<T>
	{
		readonly IInnerContentActivator<T> _activator;
		readonly IInnerContentCommand<T> _content;
		readonly IInnerContentResult<T> _result;

		public InnerContentReader(IInnerContentActivator<T> activator, IInnerContentCommand<T> content, IInnerContentResult<T> result)
		{
			_activator = activator;
			_content = content;
			_result = result;
		}

		public T Get(IFormatReader parameter)
		{
			var content = _activator.Get(parameter);
			if (content != null)
			{
				while (content.MoveNext())
				{
					_content.Execute(content);
				}
				return _result.Get(content);
			}

			return default(T);
		}
	}
}