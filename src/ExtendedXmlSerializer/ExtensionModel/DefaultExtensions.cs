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

using System.Collections.Generic;
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Members;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ExtensionModel.Content;
using ExtendedXmlSerializer.ExtensionModel.Content.Members;
using ExtendedXmlSerializer.ExtensionModel.Format;
using ExtendedXmlSerializer.ExtensionModel.References;
using ExtendedXmlSerializer.ExtensionModel.Types;

namespace ExtendedXmlSerializer.ExtensionModel
{
	public sealed class DefaultExtensions : ItemsBase<ISerializerExtension>
	{
		readonly IMetadataSpecification _metadata;
		readonly IFormatExtension _format;
		readonly ISerializationExtension _serialization;
		readonly ContentModel.Reflection.INames _defaultTypeNames;
		readonly INames _defaultMemberNames;
		readonly IParameterizedSource<MemberInfo, int> _defaultMemberOrder;

		public DefaultExtensions(IMetadataSpecification metadata, IFormatExtension format,
		                         ISerializationExtension serialization,
		                         ContentModel.Reflection.INames defaultTypeNames, INames defaultMemberNames,
		                         IParameterizedSource<MemberInfo, int> defaultMemberOrder)
		{
			_metadata = metadata;
			_format = format;
			_serialization = serialization;
			_defaultTypeNames = defaultTypeNames;
			_defaultMemberNames = defaultMemberNames;
			_defaultMemberOrder = defaultMemberOrder;
		}

		public override IEnumerator<ISerializerExtension> GetEnumerator()
		{
			yield return DefaultReferencesExtension.Default;
			yield return ContentModelExtension.Default;
			yield return TypeModelExtension.Default;
			yield return _format;
			yield return new ConverterRegistryExtension();
			yield return MemberModelExtension.Default;
			yield return new TypeNamesExtension(_defaultTypeNames);
			yield return new MemberPropertiesExtension(_defaultMemberNames, _defaultMemberOrder);
			yield return new AllowedMembersExtension(_metadata);
			yield return new AllowedMemberValuesExtension();
			yield return new MemberFormatExtension();
			yield return _serialization;
		}
	}
}