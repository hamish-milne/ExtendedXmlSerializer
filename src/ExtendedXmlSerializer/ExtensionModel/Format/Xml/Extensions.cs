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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ExtensionModel.Format.Xml.Classic;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ExtensionModel.Format.Xml
{
	public static class Extensions
	{
		public static XElement Member(this XElement @this, string name)
			=> @this.Element(XName.Get(name, @this.Name.NamespaceName));

		public static TypeConfiguration<T> CustomSerializer<T>(this TypeConfiguration<T> @this,
		                                                       Action<System.Xml.XmlWriter, T> serializer,
		                                                       Func<XElement, T> deserialize)
			=> @this.CustomSerializer(new ExtendedXmlCustomSerializer<T>(deserialize, serializer));

		public static TypeConfiguration<T> CustomSerializer<T>(this TypeConfiguration<T> @this,
		                                                       IExtendedXmlCustomSerializer<T> serializer)
		{
			@this.Configuration.With<CustomXmlExtension>().Assign(@this.Get(), new Adapter<T>(serializer));
			return @this;
		}

		public static TypeConfiguration<T> AddMigration<T>(this TypeConfiguration<T> @this,
		                                                   ICommand<XElement> migration)
			=> @this.AddMigration(migration.Execute);

		public static TypeConfiguration<T> AddMigration<T>(this TypeConfiguration<T> @this,
		                                                   Action<XElement> migration)
			=> @this.AddMigration(migration.Yield());

		public static TypeConfiguration<T> AddMigration<T>(this TypeConfiguration<T> @this,
		                                                   IEnumerable<Action<XElement>> migrations)
		{
			@this.Configuration.With<MigrationsExtension>().Add(@this.Get(), migrations.Fixed());
			return @this;
		}

		public static IExtendedXmlSerializer Create(this IConfigurationContainer @this)
			=> @this.AsValid<ConfigurationContainer>().Create();

		public static IConfigurationContainer UseAutoFormatting(this IConfigurationContainer @this)
			=> @this.Extend(AutoMemberFormatExtension.Default);

		public static IConfigurationContainer UseAutoFormatting(this IConfigurationContainer @this, int maxTextLength)
			=> @this.Extend(new AutoMemberFormatExtension(maxTextLength));

		public static IConfigurationContainer EnableClassicMode(this IConfigurationContainer @this)
			=> @this./*Emit(EmitBehaviors.Classic).*/Extend(ClassicExtension.Default);

		public static IConfigurationContainer UseOptimizedNamespaces(this IConfigurationContainer @this)
			=> @this.Extend(OptimizedNamespaceExtension.Default);


		readonly static Func<Stream> New = Activators.Default.New<MemoryStream>;

		readonly static IXmlWriterFactory WriterFactory
			= new XmlWriterFactory(CloseSettings.Default.Get(Defaults.WriterSettings));

		readonly static XmlReaderSettings CloseRead = CloseSettings.Default.Get(Defaults.ReaderSettings);

		public static IExtendedXmlSerializer Create<T>(this T @this, Func<T, IConfigurationContainer> configure)
			where T : IConfigurationContainer => configure(@this).Create();


		public static string Serialize<T>(this IExtendedXmlSerializer @this, T instance)
			=> Serialize(@this, WriterFactory, New, instance);

		public static string Serialize<T>(this IExtendedXmlSerializer @this, XmlWriterSettings settings, T instance)
			=> Serialize(@this, new XmlWriterFactory(CloseSettings.Default.Get(settings)), New, instance);

		public static string Serialize<T>(this IExtendedXmlSerializer @this, Stream stream, T instance)
			=> Serialize(@this, XmlWriterFactory.Default, stream.Self, instance);

		public static string Serialize<T>(this IExtendedXmlSerializer @this, XmlWriterSettings settings, Stream stream,
		                                  T instance)
			=> Serialize(@this, new XmlWriterFactory(settings), stream.Self, instance);

		static string Serialize<T>(this IExtendedXmlSerializer @this, IXmlWriterFactory factory, Func<Stream> stream,
		                           T instance)
			=> new InstanceFormatter<T>(@this, factory, stream).Get(instance);

		public static void Serialize<T>(this IExtendedXmlSerializer @this, TextWriter writer, T instance)
			=> Serialize(@this, XmlWriterFactory.Default, writer, instance);

		public static void Serialize<T>(this IExtendedXmlSerializer @this, XmlWriterSettings settings, TextWriter writer,
		                                T instance)
			=> Serialize(@this, new XmlWriterFactory(settings), writer, instance);

		static void Serialize<T>(this IExtendedXmlSerializer @this, IXmlWriterFactory factory, TextWriter writer, T instance)
			=> @this.Serialize(factory.Get(writer), instance);

		public static XmlParserContext Context(this XmlNameTable @this)
			=> XmlParserContexts.Default.Get(@this ?? new NameTable());


		public static T Deserialize<T>(this IExtendedXmlSerializer @this, string data)
			=> Deserialize<T>(@this, CloseRead, new MemoryStream(Encoding.UTF8.GetBytes(data)));

		public static T Deserialize<T>(this IExtendedXmlSerializer @this, Stream stream)
			=> Deserialize<T>(@this, Defaults.ReaderSettings, stream);

		public static T Deserialize<T>(this IExtendedXmlSerializer @this, XmlReaderSettings settings, Stream stream)
			=> Deserialize<T>(@this, new XmlReaderFactory(settings, settings.NameTable.Context()), stream);

		static T Deserialize<T>(this IExtendedXmlSerializer @this, IXmlReaderFactory factory, Stream stream)
		{
			return @this.Deserialize<T>((string) null /*factory.Get(stream)*/);
		}

		public static T Deserialize<T>(this IExtendedXmlSerializer @this, TextReader reader)
			=> Deserialize<T>(@this, Defaults.ReaderSettings, reader);

		public static T Deserialize<T>(this IExtendedXmlSerializer @this, XmlReaderSettings settings, TextReader reader)
			=> Deserialize<T>(@this, new XmlReaderFactory(settings, settings.NameTable.Context()), reader);

		static T Deserialize<T>(this IExtendedXmlSerializer @this, IXmlReaderFactory factory, TextReader reader)
			=> @this.Deserialize<T>(/*factory.Get(reader)*/StreamReader.Null);


		/*public static T Deserializer<T>(this IExtendedXmlSerializer<T> @this, string data)
					=> Deserializer(@this, CloseRead, new MemoryStream(Encoding.UTF8.GetBytes(data)));

				public static T Deserializer<T>(this IExtendedXmlSerializer<T> @this, Stream stream)
					=> Deserializer(@this, Defaults.ReaderSettings, stream);

				public static T Deserializer<T>(this IExtendedXmlSerializer<T> @this, XmlReaderSettings settings, Stream stream)
					=> Deserializer(@this, new XmlReaderFactory(settings, settings.NameTable.Context()), stream);

				static T Deserializer<T>(this IExtendedXmlSerializer<T> @this, IXmlReaderFactory factory, Stream stream)
					=> @this.Deserialize(factory.Get(stream));

				public static T Deserializer<T>(this IExtendedXmlSerializer<T> @this, TextReader reader)
					=> Deserializer(@this, Defaults.ReaderSettings, reader);

				public static T Deserializer<T>(this IExtendedXmlSerializer<T> @this, XmlReaderSettings settings, TextReader reader)
					=> Deserializer(@this, new XmlReaderFactory(settings, settings.NameTable.Context()), reader);

				static T Deserializer<T>(this IExtendedXmlSerializer<T> @this, IXmlReaderFactory factory, TextReader reader)
					=> @this.Deserialize(factory.Get(reader));*/


		sealed class CloseSettings : IAlteration<XmlWriterSettings>, IAlteration<XmlReaderSettings>
		{
			public static CloseSettings Default { get; } = new CloseSettings();
			CloseSettings() {}

			public XmlWriterSettings Get(XmlWriterSettings parameter)
			{
				var result = parameter.Clone();
				result.CloseOutput = true;
				return result;
			}

			public XmlReaderSettings Get(XmlReaderSettings parameter)
			{
				var result = parameter.Clone();
				result.CloseInput = true;
				return result;
			}
		}
	}
}