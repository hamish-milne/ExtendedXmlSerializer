using System.Collections.Generic;

#if NET35
namespace System.Reflection
{
	internal static class Extensions
	{
		public static TypeInfo GetTypeInfo(this Type type)
		{
			return new TypeInfo(type);
		}
	}

	internal struct TypeInfo
	{
		private readonly Type type;

		public TypeInfo(Type type)
		{
			this.type = type;
		}

		public bool IsAssignableFrom(Type other)
		{
			return type.IsAssignableFrom(other);
		}

		public bool IsGenericType => type.IsGenericType;
		public bool IsArray => type.IsArray;
		public bool IsEnum => type.IsEnum;
		public bool IsValueType => type.IsValueType;
		public bool IsClass => type.IsClass;
		public bool IsPrimitive => type.IsPrimitive;
		public bool IsAbstract => type.IsAbstract;
		public bool IsInterface => type.IsInterface;

		public Type[] ImplementedInterfaces => type.GetInterfaces();

		public T GetCustomAttribute<T>() where T : Attribute
		{
			return (T)Attribute.GetCustomAttribute(type, typeof(T));
		}
	}
}
#endif
