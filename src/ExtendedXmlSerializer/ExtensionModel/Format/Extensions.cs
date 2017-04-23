using System;
using System.IO;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ContentModel.Content.Composite.Members;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Specifications;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ExtensionModel.Format
{
	public static class Defaults
	{
		public static Func<Stream> Stream { get; } = Activators.Default.New<MemoryStream>;
	}

	public static class Extensions
	{
		public static IMemberConfiguration Attribute<T, TMember>(
			this MemberConfiguration<T, TMember> @this, Func<TMember, bool> when)
		{
			@this.Configuration.With<MemberFormatExtension>().Specifications[@this.Get()] =
				new AttributeSpecification(new DelegatedSpecification<TMember>(when).Adapt());
			return @this.Attribute();
		}

		public static IMemberConfiguration Attribute(this IMemberConfiguration @this)
		{
			@this.Configuration.With<MemberFormatExtension>().Registered.Add(@this.Get());
			return @this;
		}

		public static IMemberConfiguration Content(this IMemberConfiguration @this)
		{
			@this.Configuration.With<MemberFormatExtension>().Registered.Remove(@this.Get());
			return @this;
		}

	}
}
