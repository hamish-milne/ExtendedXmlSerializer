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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExtendedXmlSerialization.Cache
{
    public interface IElementTypeLocator
    {
        Type Locate( Type type );
    }

    public class ElementTypeLocator : ConcurrentDictionary<Type, Type>, IElementTypeLocator
    {
        readonly static Func<Type, Type> PerformLocationDelegate = PerformLocation;
        readonly static TypeInfo ArrayInfo = typeof(Array).GetTypeInfo();
        public static ElementTypeLocator Default { get; } = new ElementTypeLocator();
        ElementTypeLocator() {}

        public Type Locate( Type type )
        {
            return GetOrAdd( type, PerformLocationDelegate );
        }

        // Attribution: http://stackoverflow.com/a/17713382/3602057
        static Type PerformLocation( Type type )
        {
             // Type is Array
            // short-circuit if you expect lots of arrays 
            if ( ArrayInfo.IsAssignableFrom( type ) )
                return type.GetElementType();

            // type is IEnumerable<T>;
            if ( type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) )
                return type.GetGenericArguments()[0];

            // type implements/extends IEnumerable<T>;
            var result = type.GetInterfaces()
                             .Where( t => t.GetTypeInfo().IsGenericType &&
                                          t.GetGenericTypeDefinition() == typeof(IEnumerable<>) )
                             .Select( t => t.GetGenericArguments()[0] ).FirstOrDefault();
            
            return result;
        }
    }
}
