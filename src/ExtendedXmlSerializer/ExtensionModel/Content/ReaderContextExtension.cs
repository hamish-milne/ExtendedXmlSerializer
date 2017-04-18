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

using ExtendedXmlSerializer.ContentModel.Content.Composite;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;

namespace ExtendedXmlSerializer.ExtensionModel.Content
{
	public sealed class ReaderContextExtension : ISerializerExtension
	{
		public static ReaderContextExtension Default { get; } = new ReaderContextExtension();
		ReaderContextExtension() {}

		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.Decorate<IAlteration<IInnerContentCommand>, Wrapper>();

		void ICommand<IServices>.Execute(IServices parameter) {}

		sealed class Wrapper : IAlteration<IInnerContentCommand>
		{
			readonly IAlteration<IInnerContentCommand> _handler;

			public Wrapper(IAlteration<IInnerContentCommand> handler)
			{
				_handler = handler;
			}

			public IInnerContentCommand Get(IInnerContentCommand parameter) => new Command(_handler.Get(parameter));

			sealed class Command : IInnerContentCommand
			{
				readonly IContentsHistory _contexts;
				readonly IInnerContentCommand _command;

				public Command(IInnerContentCommand command) : this(ContentsHistory.Default, command) {}

				public Command(IContentsHistory contexts, IInnerContentCommand command)
				{
					_contexts = contexts;
					_command = command;
				}

				public void Execute(IInnerContent<object> parameter)
				{
					var contexts = _contexts.Get(parameter.Get());
					contexts.Push(parameter);
					_command.Execute(parameter);
					contexts.Pop();
				}
			}
		}
	}
}