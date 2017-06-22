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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExtendedXmlSerialization.Cache
{
    internal static class ObjectAccessors
    {
        internal delegate object ObjectActivator();

        internal delegate object PropertyGetter(object item);

        internal delegate void PropertySetter(object item, object value);
        
        internal delegate void AddItemToCollection(object item, object value);
        internal delegate void AddItemToDictionary(object item, object key, object value);

        internal static ObjectActivator CreateObjectActivator(Type type, bool isPrimitive)
        {
            var typeInfo = type.GetTypeInfo();
            //if isClass or struct but not abstract, enum or primitive
            if (!isPrimitive && (typeInfo.IsClass || typeInfo.IsValueType) && !typeInfo.IsAbstract && !typeInfo.IsEnum && !typeInfo.IsPrimitive)
            {
                if (typeInfo.IsClass)
                {
                    //class must have constructor
                    var constructor = type.GetConstructor(Type.EmptyTypes);
                    if (constructor == null)
                        return null;
                }

                var newExp = Expression.Convert(Expression.New(type), typeof(object));

                var lambda = Expression.Lambda<ObjectActivator>(newExp);

                return lambda.Compile();
            }
                
            return null;
        }
        
        internal static PropertyGetter CreatePropertyGetter(Type type, string propertyName)
        {
            // Object (type object) from witch the data are retrieved
            ParameterExpression itemObject = Expression.Parameter(typeof(object), "item");

            // Object casted to specific type using the operator "as".
            UnaryExpression itemCasted = Expression.Convert(itemObject, type);

            // Property from casted object
            MemberExpression property = Expression.PropertyOrField(itemCasted, propertyName);

            // Because we use this function also for value type we need to add conversion to object
            Expression conversion = Expression.Convert(property, typeof(object));

            LambdaExpression lambda = Expression.Lambda(typeof(PropertyGetter), conversion, itemObject);

            PropertyGetter compiled = (PropertyGetter)lambda.Compile();
            return compiled;
        }

        internal static AddItemToDictionary CreateMethodAddToDictionary(Type type)
        {
            // Object (type object) from witch the data are retrieved
            ParameterExpression itemObject = Expression.Parameter(typeof(object), "item");

            // Object casted to specific type using the operator "as".
            UnaryExpression itemCasted = Expression.Convert(itemObject, type);

            var arguments = type.GetGenericArguments();
            List<ParameterExpression> objParams = new List<ParameterExpression>();
            //Add object as first param
            objParams.Add(itemObject);
            List<Expression> castedParams = new List<Expression>();
            foreach (var argument in arguments)
            {
                ParameterExpression value = Expression.Parameter(typeof(object), "value");
                objParams.Add(value);
                castedParams.Add(Expression.Convert(value, argument));
            }

            MethodInfo method = type.GetMethod("Add");

            Expression conversion = Expression.Call(itemCasted, method, castedParams);

            LambdaExpression lambda = Expression.Lambda(typeof(AddItemToDictionary), conversion, objParams);

            AddItemToDictionary compiled = (AddItemToDictionary)lambda.Compile();
            return compiled;
        }

        internal static AddItemToCollection CreateMethodAddCollection(Type type, Type elementType)
        {
            // Object (type object) from witch the data are retrieved
            ParameterExpression itemObject = Expression.Parameter(typeof(object), "item");

            // Object casted to specific type using the operator "as".
            UnaryExpression itemCasted = Expression.Convert(itemObject, type);

            var parameterType = elementType ?? type.GetGenericArguments()[0];

            ParameterExpression value = Expression.Parameter(typeof(object), "value");

            Expression castedParam = Expression.Convert(value, parameterType);

            MethodInfo method = AddMethodLocator.Default.Locate(type, parameterType);
            
            Expression conversion = Expression.Call(itemCasted, method, castedParam);

            LambdaExpression lambda = Expression.Lambda(typeof(AddItemToCollection), conversion, itemObject, value);

            AddItemToCollection compiled = (AddItemToCollection)lambda.Compile();
            return compiled;
        }

        internal static PropertySetter CreatePropertySetter(Type type, string propertyName)
        {
            // Object (type object) from witch the data are retrieved
            ParameterExpression itemObject = Expression.Parameter(typeof(object), "item");

            // Object casted to specific type using the operator "as".
            Expression itemCasted =
#if !NET35
				type.GetTypeInfo().IsValueType
				? Expression.Unbox(itemObject, type)
                : 
#endif
				Expression.Convert(itemObject, type);

			// Property from casted object
			MemberExpression property = Expression.PropertyOrField(itemCasted, propertyName);

            // Secound parameter - value to set
            ParameterExpression value = Expression.Parameter(typeof(object), "value");

            // Because we use this function also for value type we need to add conversion to object
            Expression paramCasted = Expression.Convert(value, property.Type);

#if NET35
	        Expression assign = Expression.Call(itemCasted, "set_" + propertyName, new[] {property.Type}, paramCasted);
#else
			// Assign value to property
			BinaryExpression assign = Expression.Assign(property, paramCasted);
#endif

            LambdaExpression lambda = Expression.Lambda(typeof(PropertySetter), assign, itemObject, value);

            PropertySetter compiled = (PropertySetter)lambda.Compile();
            return compiled;
        }
    }
}
