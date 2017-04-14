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

using ExtendedXmlSerializer.ExtensionModel.Content;
using ExtendedXmlSerializer.ExtensionModel.Format.Xml;
using ExtendedXmlSerializer.Tests.Support;
using Xunit;

namespace ExtendedXmlSerializer.Tests
{
	public class SerializeTests
	{
		/*readonly IXmlWriterFactory _writerFactory = XmlWriterFactory.Default;*/

		[Fact]
		public void Verify()
		{
			var sut = new ConfigurationContainer().OptimizeConverters().Create().ForTesting();
			sut.WriteLine(1234);
			/*First(sut);
			Measure(sut);*/
		}

		/*string First(IExtendedXmlSerializer sut)
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = _writerFactory.Get(stream))
				{
					sut.Serialize(writer, 6776);
					stream.Seek(0, SeekOrigin.Begin);
					var result = new StreamReader(stream).ReadToEnd();
					return result;
				}
			}
		}

		void Measure(IExtendedXmlSerializer sut) => First(sut);*/
	}
}