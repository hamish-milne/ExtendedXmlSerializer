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

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Content.Composite.Members;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Specifications;

namespace ExtendedXmlSerializer.ExtensionModel.Content.Members
{
	public sealed class AllowedMembersExtension : ISerializerExtension
	{
		readonly static Collection<MemberInfo> DefaultBlacklist =
			new Collection<MemberInfo>
			{
				typeof(IDictionary<,>).GetRuntimeProperty(nameof(IDictionary.Keys)),
				typeof(IDictionary<,>).GetRuntimeProperty(nameof(IDictionary.Values))
			};

		public AllowedMembersExtension(IMetadataSpecification specification)
			: this(specification, new HashSet<MemberInfo>(DefaultBlacklist), new HashSet<MemberInfo>()) {}

		public AllowedMembersExtension(IMetadataSpecification specification, ICollection<MemberInfo> blacklist,
		                               ICollection<MemberInfo> whitelist)
		{
			Specification = specification;
			Blacklist = blacklist;
			Whitelist = whitelist;
		}

		public IMetadataSpecification Specification { get; }
		public ICollection<MemberInfo> Blacklist { get; }
		public ICollection<MemberInfo> Whitelist { get; }

		public IServiceRepository Get(IServiceRepository parameter)
		{
			var policy = Whitelist.Any()
				? (ISpecification<MemberInfo>) new WhitelistMemberPolicy(Whitelist.ToArray())
				: new BlacklistMemberPolicy(Blacklist.ToArray());

			return parameter
				.RegisterInstance(policy.And<PropertyInfo>(Specification))
				.RegisterInstance(policy.And<FieldInfo>(Specification))
				.Register<IMetadataSpecification, MetadataSpecification>();
		}

		void ICommand<IServices>.Execute(IServices parameter) {}
	}
}