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

using System.Reflection;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Content.Conversion;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.Core.Specifications;
using ExtendedXmlSerializer.ExtensionModel.Content.Members;

namespace ExtendedXmlSerializer.ExtensionModel.Content
{
	public static class Extensions
	{
		public static IServiceRepository Decorate<T>(this IServiceRepository @this, ISpecification<TypeInfo> specification)
			where T : IElement
			=> new ElementDecoration<T>(specification).Get(@this);



		public static IConfigurationContainer Emit(this IConfigurationContainer @this, IEmitBehavior behavior) => behavior.Get(@this);

		public static IMemberConfiguration Ignore(this IMemberConfiguration @this)
		{
			@this.Configuration.With<AllowedMembersExtension>().Blacklist.Add(@this.Get());
			return @this;
		}

		public static IMemberConfiguration Include(this IMemberConfiguration @this)
		{
			@this.Configuration.With<AllowedMembersExtension>().Whitelist.Add(@this.Get());
			return @this;
		}

		public static IConfigurationContainer OnlyConfiguredProperties(this IConfigurationContainer @this)
		{
			foreach (var type in TypeConfigurations.Defaults.Get(@this))
			{
				type.OnlyConfiguredProperties();
			}
			return @this;
		}

		public static ITypeConfiguration OnlyConfiguredProperties(this ITypeConfiguration @this)
		{
			foreach (var member in @this)
			{
				member.Include();
			}
			return @this;
		}

		public static IConfigurationContainer Alter(this IConfigurationContainer @this, IAlteration<IConverter> alteration)
		{
			@this.With<ConverterAlterationsExtension>().Alterations.Add(alteration);
			return @this;
		}

		public static IConfigurationContainer OptimizeConverters(this IConfigurationContainer @this)
			=> OptimizeConverters(@this, new Optimizations());

		public static IConfigurationContainer OptimizeConverters(this IConfigurationContainer @this, Optimizations optimizations)
			=> @this.Alter(optimizations);
	}
}