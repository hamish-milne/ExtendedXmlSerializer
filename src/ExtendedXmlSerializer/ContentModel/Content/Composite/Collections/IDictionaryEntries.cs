/*namespace ExtendedXmlSerializer.ContentModel.Content.Composite.Collections
{
	public interface IDictionaryEntries : IParameterizedSource<TypeInfo, ISerializer<DictionaryEntry>> {}

	sealed class DictionaryEntries : IDictionaryEntries
	{
		readonly static TypeInfo Type = Support<DictionaryEntry>.Key;

		readonly static PropertyInfo
			Key = Type.GetProperty(nameof(DictionaryEntry.Key)),
			Value = Type.GetProperty(nameof(DictionaryEntry.Value));

		readonly static DictionaryPairTypesLocator Pairs = DictionaryPairTypesLocator.Default;

		readonly IInnerContents<IDictionary, DictionaryEntry> _contents;
		readonly IMembers _members;
		readonly IMemberSerializers _serializers;
		readonly IWriter<DictionaryEntry> _element;
		readonly IDictionaryPairTypesLocator _locator;
		readonly IInnerContentActivator<DictionaryEntry> _activator;

		[UsedImplicitly]
		public DictionaryEntries(IInnerContents<IDictionary, DictionaryEntry> contents, IIdentities identities, IMembers members,
		                         IMemberSerializers serializers)
			: this(
				contents, serializers, members, new ElementOption(identities).Get(Type).AsValid<IWriter<DictionaryEntry>>(), Pairs,
				contents.Get(Type).AsValid<IInnerContentActivator<DictionaryEntry>>()) {}

		public DictionaryEntries(IInnerContents<IDictionary, DictionaryEntry> contents, IMemberSerializers serializers, IMembers members,
		                         IWriter<DictionaryEntry> element, IDictionaryPairTypesLocator locator,
		                         IInnerContentActivator<DictionaryEntry> activator)
		{
			_contents = contents;
			_members = members;
			_serializers = serializers;
			_element = element;
			_locator = locator;
			_activator = activator;
		}

		IMemberSerializer Create(PropertyInfo metadata, TypeInfo classification)
			=> _serializers.Get(_members.Get(new MemberDescriptor(metadata, classification)));

		public ISerializer<DictionaryEntry> Get(TypeInfo parameter)
		{
			var pair = _locator.Get(parameter);
			var members = new[] {Create(Key, pair.KeyType), Create(Value, pair.ValueType)}.ToImmutableArray();
			var member = new MemberSerialization(new FixedRuntimeMemberList(members), members);

			var reader = new InnerContentReader<DictionaryEntry>(
				_activator,
				new InnerContentCommand<DictionaryEntry>(member, _contents, _contents),
				_contents);

			var result = new Serializer<DictionaryEntry>(
				reader,
				new Enclosure<DictionaryEntry>(_element, new MemberListWriter<DictionaryEntry>(member)));
			return result;
		}
	}

	sealed class DictionaryContentOption : DelegatedContentOption
	{
		public DictionaryContentOption(IActivatingTypeSpecification specification, IDictionaryContents contents)
			: base(specification, contents.Get) {}
	}

	public interface IDictionaryContents : ISerializerSource {}

	sealed class DictionaryContents : IDictionaryContents
	{
		readonly IMemberSerializations _serializations;
		readonly IDictionaryEnumerators _enumerators;
		readonly IDictionaryEntries _entries;
		readonly IInnerContents<IDictionary, DictionaryEntry> _instance;

		public DictionaryContents(IMemberSerializations serializations, IDictionaryEnumerators enumerators,
		                          IDictionaryEntries entries, IInnerContents<IDictionary, DictionaryEntry> instance)
		{
			_serializations = serializations;
			_enumerators = enumerators;
			_entries = entries;
			_instance = instance;
		}

		public ISerializer Get(TypeInfo parameter)
		{
			var members = _serializations.Get(parameter);
			var entry = _entries.Get(parameter);

			var handler = new CompositeCollectionInnerContentCommand<IDictionary, DictionaryEntry>(
				_instance,
				new InnerContentCommand<IDictionary>(members, _instance, _instance),
				new CollectionInnerContentCommand<IDictionary, DictionaryEntry>(entry, _instance)
				);

			var reader = new InnerContentReader<IDictionary>(_instance.Get(parameter), handler, _instance);
			var writer = new CompositeCollectionWriter<IDictionary>(
				new MemberListWriter<IDictionary>(members),
				new EnumerableWriter<DictionaryEntry>(_enumerators, entry));
			var result = new Serializer<IDictionary>(reader, writer).Adapt();
			return result;
		}
	}
}*/