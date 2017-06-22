#if NET35
using System.Collections.Generic;
namespace System.Threading.Tasks { }

namespace System.Collections.Concurrent
{

	public class ConcurrentDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		private readonly object monitor = new object();

		public TValue GetOrAdd(TKey key, TValue value)
		{
			lock (monitor)
			{
				if (!TryGetValue(key, out var ret))
					Add(key, ret = value);
				return ret;
			}
		}

		public TValue GetOrAdd(TKey key, Func<TKey, TValue> value)
		{
			lock (monitor)
			{
				if (!TryGetValue(key, out var ret))
					Add(key, ret = value(key));
				return ret;
			}
		}
	}

	internal class ConditionalWeakTable<TKey, TValue> : Dictionary<TKey, TValue>
	{
		private readonly object monitor = new object();

		public TValue GetValue(TKey key, Func<TKey, TValue> value)
		{
			lock (monitor)
			{
				if (!TryGetValue(key, out var ret))
					Add(key, ret = value(key));
				return ret;
			}
		}
	}
}

namespace System
{
	internal class Lazy<T> where T : class
	{
		private T value;
		private readonly Func<T> factory;

		public T Value => value = value ?? factory();

		public Lazy(Func<T> factory)
		{
			this.factory = factory;
		}
	}
}

#endif
