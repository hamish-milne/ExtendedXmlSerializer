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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ContentModel.Content.Conversion
{
	public sealed class Optimizations : IAlteration<IConverter>, IOptimizations
	{
		readonly IGeneric<ICollection<Action>, IAlteration<IConverter>> _adapter;
		readonly ICollection<Action> _containers;

		public Optimizations() : this(new HashSet<Action>()) {}

		public Optimizations(ICollection<Action> containers) : this(containers, Adapter.Default) {}

		public Optimizations(ICollection<Action> containers,
		                     IGeneric<ICollection<Action>, IAlteration<IConverter>> adapter)
		{
			_containers = containers;
			_adapter = adapter;
		}

		public IConverter Get(IConverter parameter) => _adapter.GetFrom(parameter)
		                                                       .Invoke(_containers)
		                                                       .Get(parameter);


		public void Clear()
		{
			foreach (var container in _containers)
			{
				container.Invoke();
			}
		}

		sealed class Adapter : Generic<ICollection<Action>, IAlteration<IConverter>>
		{
			public static Adapter Default { get; } = new Adapter();
			Adapter() : base(typeof(Alteration<>)) {}

			sealed class Alteration<T> : IAlteration<IConverter>
			{
				readonly ICollection<Action> _containers;

				public Alteration(ICollection<Action> containers)
				{
					_containers = containers;
				}

				public IConverter Get(IConverter parameter)
				{
					var converter = parameter.AsValid<IConverter<T>>();
					var parse = Create<string, T>(converter.Parse);
					var format = Create<T, string>(converter.Format);
					var result = new Converter<T>(converter, parse, format);
					return result;
				}

				Func<TParameter, TResult> Create<TParameter, TResult>(Func<TParameter, TResult> source)
				{
					var dictionary = new ConcurrentDictionary<TParameter, TResult>(EqualityComparer<TParameter>.Default);
					var cache = new Cache<TParameter, TResult>(source, dictionary);
					_containers.Add(dictionary.Clear);
					var result = cache.ToDelegate();
					return result;
				}
			}
		}
	}
}