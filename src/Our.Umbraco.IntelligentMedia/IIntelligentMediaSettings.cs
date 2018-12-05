namespace Our.Umbraco.IntelligentMedia
{
	public interface IIntelligentMediaSettings
	{
		T Settings<T>() where T : new();
	}
}
