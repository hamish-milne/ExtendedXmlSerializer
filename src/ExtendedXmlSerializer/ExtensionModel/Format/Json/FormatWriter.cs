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

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Conversion;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.ContentModel.Members;
using ExtendedXmlSerializer.ContentModel.Properties;
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.Core;
using Newtonsoft.Json;

namespace ExtendedXmlSerializer.ExtensionModel.Format.Json
{
	sealed class FormatWriter : Dictionary<string, string>, IFormatWriter
	{
		readonly static Delimiter DefaultSeparator = DefaultClrDelimiters.Default.Separator;

		readonly IAliases _aliases;
		readonly IIdentifierFormatter _formatter;
		readonly IIdentityStore _store;
		readonly ITypePartResolver _parts;

		readonly JsonTextWriter _writer;
		readonly Delimiter _separator;
		readonly Func<TypeParts, TypeParts> _selector;

		public FormatWriter(IAliases aliases, IIdentifierFormatter formatter, IIdentityStore store, ITypePartResolver parts,
		                    Writing<JsonTextWriter> parameter)
			: this(aliases, formatter, store, parts, parameter.Writer, parameter.Instance, DefaultSeparator) {}

		public FormatWriter(IAliases aliases, IIdentifierFormatter formatter, IIdentityStore store, ITypePartResolver parts,
		                    JsonTextWriter writer, object instance,
		                    Delimiter separator)
		{
			_aliases = aliases;
			_formatter = formatter;
			_store = store;
			_parts = parts;
			_writer = writer;
			_separator = separator;
			Instance = instance;
			_selector = Get;
		}

		public object Instance { get; }
		public object Get() => _writer;

		public void Start(IIdentity identity)
		{
			/*var identifier = identity.Identifier.NullIfEmpty();
			if (identifier != null)
			{

				/*_writer.WriteStartElement(identity.Name, identifier);#1#
			}
			else
			{
				_writer.WriteStartElement(identity.Name);
			}*/
			_writer.WriteStartObject();
			_writer.WritePropertyName("$type");
			_writer.WriteValue(IdentityFormatter.Default.Get(identity));
		}

		public void EndCurrent() => _writer.WriteEnd();

		public void Content(string content) => _writer.WriteValue(content);

		public void Content(IIdentity property, string content)
		{
			/*var identifier = property.Identifier.NullIfEmpty();
			var prefix = identifier != null ? Prefix(identifier) : null;
			var name = property.Name;

			if (!string.IsNullOrEmpty(prefix))
			{
				_writer.WriteAttributeString(prefix, name, identifier, content);
			}
			else
			{
				_writer.WriteAttributeString(name, content);
			}*/
		}

		string Prefix(string parameter) => Lookup(parameter) ?? CreatePrefix(parameter);

		string Lookup(string parameter)
		{
			/*var lookup = _writer.LookupPrefix(parameter ?? string.Empty);
			if (parameter != null && lookup == string.Empty && !ContainsKey(string.Empty))
			{
				Add(string.Empty, parameter);
			}

			var result = parameter == this.Get(string.Empty) ? string.Empty : lookup;
			return result;*/
			return null;
		}

		string CreatePrefix(string identifier)
		{
			if (!ContainsKey(identifier))
			{
				var result = _aliases.Get(identifier) ?? Create(identifier);
				// _writer.WriteAttributeString(result, Xmlns, identifier);
				return result;
			}
			return null;
		}

		string Create(string parameter)
		{
			var result = _formatter.Get(Count + 1);
			Add(result, parameter);
			return result;
		}

		public string Get(MemberInfo parameter)
			=> parameter is TypeInfo
				? GetType((TypeInfo) parameter)
				: string.Concat(GetType(parameter.GetReflectedType()), _separator, parameter.Name);

		string GetType(TypeInfo parameter)
		{
			var typeParts = _parts.Get(parameter);
			var qualified = Get(typeParts);
			var result = TypePartsFormatter.Default.Get(qualified);
			return result;
		}

		TypeParts Get(TypeParts parameter)
		{
			var arguments = parameter.GetArguments();
			var result = new TypeParts(parameter.Name, Prefix(parameter.Identifier),
			                           arguments.HasValue
				                           ? arguments.Value.Select(_selector).ToImmutableArray
				                           : (Func<ImmutableArray<TypeParts>>) null);
			return result;
		}

		public IIdentity Get(string name, string identifier) => _store.Get(name, Prefix(identifier));

		public void Dispose()
		{
			_writer.To<IDisposable>().Dispose();
			Clear();
		}
	}
}