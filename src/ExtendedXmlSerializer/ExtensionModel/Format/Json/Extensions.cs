﻿// MIT License
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
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ExtensionModel.Format.Json
{
	public static class Extensions
	{
		readonly static Func<Stream> New = Activators.Default.New<MemoryStream>;

		public static void Serialize(this ISerializer @this, object instance)
			=> Serialize(@this, WriterFactory.Default, New, instance);

		public static string Serialize(this ISerializer @this, Stream stream, object instance)
			=> Serialize(@this, WriterFactory.Default, stream.Self, instance);

		static string Serialize(this ISerializer @this, IWriterFactory factory, Func<Stream> stream, object instance)
			=> new InstanceFormatter(@this, factory, stream).Get(instance);

		public static void Serialize(this ISerializer @this, TextWriter writer, object instance)
			=> Serialize(@this, WriterFactory.Default, writer, instance);

		static void Serialize(this ISerializer @this, IWriterFactory factory, TextWriter writer, object instance)
			=> @this.Serialize(factory.Get(writer), instance);
	}
}