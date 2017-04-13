using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer
{
	public interface ISerialize<in T>
	{
		void Serialize<TInstance>(T parameter, TInstance instance);
	}

	sealed class Serializer<T> : ISerialize<T>
	{
		readonly IFormatWriters<T> _writers;
		readonly ISerializers _serializers;

		public Serializer(IFormatWriters<T> writers, ISerializers serializers)
		{
			_writers = writers;
			_serializers = serializers;
		}

		public void Serialize<TInstance>(T parameter, TInstance instance)
		{
			var serializer = _serializers.Get(Support<T>.Key);

			var writer = _writers.Get(parameter);
			serializer.Write(writer, instance);
		}
	}
}