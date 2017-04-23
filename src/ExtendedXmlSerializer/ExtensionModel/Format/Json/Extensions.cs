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
using System.IO;
using ExtendedXmlSerializer.Core;

namespace ExtendedXmlSerializer.ExtensionModel.Format.Json
{
	public static class Extensions
	{
		public static void Serialize<T>(this ISerializer @this, T instance)
			=> Serialize(@this, WriterFactory.Default, Format.Defaults.Stream, instance);

		public static string Serialize<T>(this ISerializer @this, Stream stream, T instance)
			=> Serialize(@this, WriterFactory.Default, stream.Self, instance);

		static string Serialize<T>(this ISerializer @this, IWriterFactory factory, Func<Stream> stream, T instance)
			=> new InstanceFormatter<T>(@this.Get<T>(), factory, stream).Get(instance);

		public static void Serialize<T>(this ISerializer @this, TextWriter writer, T instance)
			=> Serialize(@this, WriterFactory.Default, writer, instance);

		static void Serialize<T>(this ISerializer @this, IWriterFactory factory, TextWriter writer, T instance)
			=> @this.Get<T>()
			        .Serialize(factory.Get(writer), instance);
	}
}