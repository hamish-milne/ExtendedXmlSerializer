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

using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Format;

namespace ExtendedXmlSerializer
{
	public interface ISerializer<in TRead, in TWrite> : IDeserialize<TRead>, ISerialize<TWrite> {}

	class Serializer<TRead, TWrite> : IDeserialize<TRead>, ISerialize<TWrite>
	{
		readonly IDeserialize<TRead> _read;
		readonly ISerialize<TWrite> _write;

		public Serializer(IDeserialize<TRead> read, ISerialize<TWrite> write)
		{
			_read = read;
			_write = write;
		}

		public TInstance Deserialize<TInstance>(TRead reader) => _read.Deserialize<TInstance>(reader);

		public void Serialize<TInstance>(TWrite parameter, TInstance instance) => _write.Serialize(parameter, instance);
	}

	sealed class Serialize<T> : ISerialize<T>
	{
		readonly IFormatWriters<T> _writers;
		readonly ISerializerStore _store;

		public Serialize(IFormatWriters<T> writers, ISerializerStore store)
		{
			_writers = writers;
			_store = store;
		}

		void ISerialize<T>.Serialize<TInstance>(T parameter, TInstance instance)
			=> _store.Get<TInstance>()
			         .Write(_writers.Get(parameter), instance);
	}
}