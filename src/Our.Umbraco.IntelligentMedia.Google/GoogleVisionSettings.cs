using System.Configuration;

namespace Our.Umbraco.IntelligentMedia.Google
{
	public class GoogleVisionSettings
	{
		public string ApiKey => ConfigurationManager.AppSettings["IntelligentMedia:Google:ApiKey"];
	}
}
