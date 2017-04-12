namespace ExtendedXmlSerializer
{
	public interface ISerialize<in TWrite>
	{
		void Serialize<T>(TWrite writer, T instance);
	}
}