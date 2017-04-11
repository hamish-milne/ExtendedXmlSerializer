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
using ExtendedXmlSerializer.ExtensionModel.Content;
using ExtendedXmlSerializer.ExtensionModel.Format.Xml;
using ExtendedXmlSerializer.ExtensionModel.Types;
using ExtendedXmlSerializer.Tests.Support;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ExtensionModel.Content.Members
{
	public class AllowedMembersExtensionTests
	{
		[Fact]
		public void Ignore()
		{
			var serializer = new ConfigurationContainer().Type<Subject>().Member(x => x.Property2).Ignore().Configuration;
			var support = new SerializationSupport(serializer);
			var instance = new Subject {Property1 = "Hello World!", Property2 = 1000, Property3 = DateTime.Now};
			var actual = support.Cycle(instance);
			Assert.NotEqual(instance.Property2, actual.Property2);
		}

		[Fact]
		public void Include()
		{
			var serializer = new ConfigurationContainer().Type<Subject>().Member(x => x.Property2).Include().Configuration;
			var support = new SerializationSupport(serializer);
			var instance = new Subject {Property1 = "Hello World!", Property2 = 1000, Property3 = DateTime.Now};
			var actual = support.Cycle(instance);
			Assert.NotEqual(instance.Property1, actual.Property1);
			Assert.Equal(instance.Property2, actual.Property2);
			Assert.NotEqual(instance.Property3, actual.Property3);
		}

		[Fact]
		public void TypedOnlyConfiguredProperties()
		{
			var serializer =
				new ConfigurationContainer().Type<Subject>()
				                            .Member(x => x.Property2)
				                            .Owner.Member(x => x.Property1)
				                            .Owner.OnlyConfiguredProperties()
				                            .Configuration;
			var support = new SerializationSupport(serializer);
			var instance = new Subject {Property1 = "Hello World!", Property2 = 1000, Property3 = DateTime.Now};
			var actual = support.Cycle(instance);
			Assert.Equal(instance.Property1, actual.Property1);
			Assert.Equal(instance.Property2, actual.Property2);
			Assert.NotEqual(instance.Property3, actual.Property3);
		}

		[Fact]
		public void GlobalOnlyConfiguredProperties()
		{
			var serializer =
				new ConfigurationContainer().Type<Subject>()
				                            .Member(x => x.Property2)
				                            .Owner.Member(x => x.Property3)
				                            .Owner.Configuration.OnlyConfiguredProperties();
			var support = new SerializationSupport(serializer);
			var instance = new Subject {Property1 = "Hello World!", Property2 = 1000, Property3 = DateTime.Now};
			var actual = support.Cycle(instance);
			Assert.NotEqual(instance.Property1, actual.Property1);
			Assert.Equal(instance.Property2, actual.Property2);
			Assert.Equal(instance.Property3, actual.Property3);
		}

		class Subject
		{
			public string Property1 { get; set; }

			public int Property2 { get; set; }

			public DateTime Property3 { get; set; }
		}
	}
}