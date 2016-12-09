﻿// MIT License
// 
// Copyright (c) 2016 Wojciech Nagórski
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
using ExtendedXmlSerialization.Services;

namespace ExtendedXmlSerialization.Cache
{
	sealed class Setters : WeakCacheBase<MemberInfo, ObjectAccessors.PropertySetter>
	{
		public static Setters Default { get; } = new Setters();
		Setters() {}

		protected override ObjectAccessors.PropertySetter Callback(MemberInfo key)
		{
			if (key.IsWritable())
			{
				return ObjectAccessors.CreatePropertySetter(key.DeclaringType, key.Name);
			}

			var setter = TypeDefinitionCache.GetDefinition(key.GetMemberType())?.MethodAddToCollection;
			var result = setter != null ? new ObjectAccessors.PropertySetter(setter) : null;
			return result;
		}
	}
}