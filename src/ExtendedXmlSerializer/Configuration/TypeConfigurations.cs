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

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ExtensionModel.Reflection;

namespace ExtendedXmlSerializer.Configuration
{
	sealed class TypeConfigurations : ReferenceCacheBase<TypeInfo, ITypeConfiguration>, IEnumerable<ITypeConfiguration>
	{
		public static IParameterizedSource<IConfigurationContainer, TypeConfigurations> Defaults { get; }
			= new ReferenceCache<IConfigurationContainer, TypeConfigurations>(x => new TypeConfigurations(x));

		readonly ICollection<ITypeConfiguration> _types = new HashSet<ITypeConfiguration>();
		readonly IConfigurationContainer _configuration;
		readonly IDictionary<TypeInfo, string> _names;

		TypeConfigurations(IConfigurationContainer configuration)
			: this(configuration, configuration.With<TypeNamesExtension>().Names) {}

		TypeConfigurations(IConfigurationContainer configuration, IDictionary<TypeInfo, string> names)
		{
			_configuration = configuration;
			_names = names;
		}

		protected override ITypeConfiguration Create(TypeInfo parameter)
		{
			var result = new TypeConfiguration(_configuration, new TypeProperty<string>(_names, parameter), parameter);
			_types.Add(result);
			return result;
		}

		public IEnumerator<ITypeConfiguration> GetEnumerator() => _types.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	sealed class TypeConfigurations<T> : ReferenceCache<IConfigurationContainer, TypeConfiguration<T>>
	{
		public static TypeConfigurations<T> Default { get; } = new TypeConfigurations<T>();
		TypeConfigurations() : base(x => new TypeConfiguration<T>(x)) {}

		public TypeConfiguration<T> For(ITypeConfiguration type)
			=> type as TypeConfiguration<T> ?? Get(type.Configuration);
	}
}