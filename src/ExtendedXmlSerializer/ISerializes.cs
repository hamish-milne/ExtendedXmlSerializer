using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Format;

namespace ExtendedXmlSerializer
{
	public interface IDeserializes<in TReader>
	{
		IDeserialize<TReader, TInstance> Get<TInstance>();
	}

	sealed class Deserializes<TReader> : IDeserializes<TReader>
	{
		readonly IFormatReaders<TReader> _readers;
		readonly ISerializerStore _store;

		public Deserializes(IFormatReaders<TReader> readers, ISerializerStore store)
		{
			_readers = readers;
			_store = store;
		}

		public IDeserialize<TReader, TInstance> Get<TInstance>()
			=> new Deserialize<TReader, TInstance>(_readers, _store.Get<TInstance>());
	}

	public interface ISerializer<in TRead, in TWrite, TInstance>
		: IDeserialize<TRead, TInstance>, ISerialize<TWrite, TInstance> {}

	sealed class Serializer<TRead, TWrite, TInstance> : ISerializer<TRead, TWrite, TInstance>
	{
		readonly IDeserialize<TRead, TInstance> _deserialize;
		readonly ISerialize<TWrite, TInstance> _serialize;

		public Serializer(IDeserialize<TRead, TInstance> deserialize, ISerialize<TWrite, TInstance> serialize)
		{
			_deserialize = deserialize;
			_serialize = serialize;
		}

		public TInstance Deserialize(TRead reader) => _deserialize.Deserialize(reader);

		public void Serialize(TWrite writer, TInstance instance) => _serialize.Serialize(writer, instance);
	}

	public interface ISerializes<in TWriter>
	{
		ISerialize<TWriter, TInstance> Get<TInstance>();
	}

	sealed class Serializes<TWriter> : ISerializes<TWriter>
	{
		readonly IFormatWriters<TWriter> _writers;
		readonly ISerializerStore _store;

		public Serializes(IFormatWriters<TWriter> writers, ISerializerStore store)
		{
			_writers = writers;
			_store = store;
		}

		public ISerialize<TWriter, TInstance> Get<TInstance>()
			=> new Serialize<TWriter, TInstance>(_writers, _store.Get<TInstance>());
	}


	public interface ISerializers<in TRead, in TWrite>
	{
		ISerializer<TRead, TWrite, TInstance> Get<TInstance>();
	}
	sealed class Serializers<TReader, TWriter> : ISerializers<TReader, TWriter>
	{
		readonly IDeserializes<TReader> _deserializes;
		readonly ISerializes<TWriter> _serializes;

		public Serializers(IDeserializes<TReader> deserializes, ISerializes<TWriter> serializes)
		{
			_deserializes = deserializes;
			_serializes = serializes;
		}

		public ISerializer<TReader, TWriter, TInstance> Get<TInstance>()
			=> new Serializer<TReader, TWriter, TInstance>(_deserializes.Get<TInstance>(), _serializes.Get<TInstance>());
	}
}