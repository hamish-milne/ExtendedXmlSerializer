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

using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.ExtensionModel;

namespace ExtendedXmlSerializer.Configuration
{
	public class ConfigurationContainer<T> : KeyedByTypeCollection<ISerializerExtension>, IConfigurationContainer
	{
		readonly IServicesFactory _source;

		public ConfigurationContainer(params ISerializerExtension[] extensions) : this(ServicesFactory.Default, extensions) {}

		public ConfigurationContainer(IServicesFactory source, params ISerializerExtension[] extensions) : base(extensions)
		{
			_source = source;
		}

		public T Create()
		{
			using (var services = _source.Get(this))
			{
				var result = services.Get<T>();
				return result;
			}
		}

		TExtension IConfigurationContainer.Find<TExtension>() => Find<TExtension>();
	}
}