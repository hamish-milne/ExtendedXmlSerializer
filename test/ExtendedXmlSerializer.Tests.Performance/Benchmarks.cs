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

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using ExtendedXmlSerializer.ExtensionModel.Content;
using ExtendedXmlSerializer.ExtensionModel.Format.Xml;
using ExtendedXmlSerializer.Tests.Performance.Model;


namespace ExtendedXmlSerializer.Tests.Performance
{
	public class ExtendedXmlSerializerTest
	{
		readonly TestClassOtherClass _obj = new TestClassOtherClass();
		readonly string _xml;
#pragma warning disable 618

		readonly ExtendedXmlSerialization.IExtendedXmlSerializer _serializer =
			new ExtendedXmlSerialization.ExtendedXmlSerializer();

#pragma warning restore 618

		public ExtendedXmlSerializerTest()
		{
			_obj.Init();
			_xml = SerializationClassWithPrimitive();
			DeserializationClassWithPrimitive();
		}

		[Benchmark]
		public string SerializationClassWithPrimitive() => _serializer.Serialize(_obj);

		[Benchmark]
		public TestClassOtherClass DeserializationClassWithPrimitive() => _serializer.Deserialize<TestClassOtherClass>(_xml);
	}

/*
	public class AffinityColumnAttribute : ColumnConfigBaseAttribute
	{
		readonly static JobCharacteristicColumn Column = new JobCharacteristicColumn(EnvMode.AffinityCharacteristic);

		public AffinityColumnAttribute() : base(Column) {}
	}
*/

	[Config(typeof(Configuration))]
	public class ExtendedXmlSerializerV2Test
	{
		/*readonly TestClassOtherClass _obj = new TestClassOtherClass().Init();
		readonly byte[] _data;*/

		/*readonly IXmlReaderFactory _readerFactory = new XmlReaderFactory();*/

		readonly ISerialize<XmlWriter, int> _serializer = new ConfigurationContainer().EnableClassicMode()
		                                                                              .OptimizeConverters()
		                                                                              .Create()
		                                                                              .Get();

		/*public ExtendedXmlSerializerV2Test()
		{
			Write();
		}*/

		[Benchmark]
		public void Write()
		{
			using (var writer = XmlWriter.Create(new MemoryStream()))
			{
				_serializer.Serialize(writer, 6776);
			}
		}

		/*
				[Benchmark]
				public object DeserializationClassWithPrimitive()
				{
					using (var stream = new MemoryStream(_data))
					{
						using (var reader = _readerFactory.Get(stream))
						{
							return _serializer.Deserialize<TestClassOtherClass>(reader);
						}
					}
				}
		*/
	}

	//[AffinityColumn]
	sealed class Configuration : ManualConfig
	{
		/*readonly static JobCharacteristicColumn Column = new JobCharacteristicColumn(EnvMode.AffinityCharacteristic);*/

		public Configuration()
		{
			Add(Job.Default
			       //.With(new WindowsClock())
			       //.WithAffinity(new IntPtr(2))
			       .WithWarmupCount(5)
			       .WithTargetCount(10));

			//Add(ProcessorColumn.Default);
		}
	}

/*
	sealed class ProcessorColumn : TagColumn
	{
		[DllImport("kernel32.dll")]
		static extern int GetCurrentProcessorNumber();

		static string GetCurrentProcessor(string _) => GetCurrentProcessorNumber()
			.ToString();

		public static ProcessorColumn Default { get; } = new ProcessorColumn();
		ProcessorColumn() : base("Processor", GetCurrentProcessor) {}
	}
*/

/*
	sealed class EmptyExports : IConfig
	{
		readonly IConfig _config;

		public EmptyExports(IConfig config)
		{
			_config = config;
		}

		public IEnumerable<IColumnProvider> GetColumnProviders() => _config.GetColumnProviders();

		public IEnumerable<IExporter> GetExporters() => Enumerable.Empty<IExporter>();

		public IEnumerable<ILogger> GetLoggers() => _config.GetLoggers();

		public IEnumerable<IDiagnoser> GetDiagnosers() => _config.GetDiagnosers();

		public IEnumerable<IAnalyser> GetAnalysers() => _config.GetAnalysers();

		public IEnumerable<Job> GetJobs() => _config.GetJobs();

		public IEnumerable<IValidator> GetValidators() => _config.GetValidators();

		public IEnumerable<HardwareCounter> GetHardwareCounters() => _config.GetHardwareCounters();

		public IOrderProvider GetOrderProvider() => _config.GetOrderProvider();

		public ISummaryStyle GetSummaryStyle() => _config.GetSummaryStyle();

		public ConfigUnionRule UnionRule => _config.UnionRule;

		public bool KeepBenchmarkFiles => false;
	}
*/

	[Config(typeof(Configuration))]
	public class XmlSerializerTest
	{
		/*		readonly IXmlReaderFactory _readerFactory = new XmlReaderFactory();*/
		readonly XmlSerializer _serializer = new XmlSerializer(typeof(int));

		/*readonly TestClassOtherClass _obj = new TestClassOtherClass().Init();*/
		/*readonly byte[] _xml;*/

		public XmlSerializerTest()
		{
			Write();
			//_xml = Encoding.UTF8.GetBytes(SerializationClassWithPrimitive());
			//DeserializationClassWithPrimitive();
		}

		[Benchmark]
		public void Write()
		{
			using (var writer = XmlWriter.Create(new MemoryStream()))
			{
				_serializer.Serialize(writer, 6776);
			}
		}

/*
		[Benchmark]
		public object DeserializationClassWithPrimitive()
		{
			using (var stream = new MemoryStream(_xml))
			{
				using (var reader = _readerFactory.Get(stream))
				{
					var result = _serializer.Deserialize(reader);
					return result;
				}
			}
		}
*/
	}
}