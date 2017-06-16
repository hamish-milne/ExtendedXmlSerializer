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

using ExtendedXmlSerializer.ContentModel;
using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.ExtensionModel.References;

namespace ExtendedXmlSerializer.ExtensionModel
{
	public interface ISerializationExtension : ISerializerExtension {}

	sealed class SerializationExtension<TInterface, TImplementation> : ISerializationExtension
		where TImplementation : TInterface
	{
		public static SerializationExtension<TInterface, TImplementation> Default { get; } =
			new SerializationExtension<TInterface, TImplementation>();

		SerializationExtension() {}

		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.Register<ISerializer, RuntimeSerializer>()
			            .Register<ISerializers, Serializers>()
			            .Decorate<ISerializers, ReferenceAwareSerializers>()
			            .RegisterConstructorDependency<IContents>((provider, info) => provider.Get<DeferredContents>())
			            .Register<IContents, Contents>()
			            .Decorate<IContents>((factory, contents) => new RecursionAwareContents(contents))
			            .Register<TInterface, TImplementation>();

		void ICommand<IServices>.Execute(IServices parameter) {}
	}
}