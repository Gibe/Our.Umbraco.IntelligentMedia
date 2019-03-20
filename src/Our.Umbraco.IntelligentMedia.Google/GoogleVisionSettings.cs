using System.Configuration;

namespace Our.Umbraco.IntelligentMedia.Google
{
	public class GoogleVisionSettings
	{
		public bool IsConfigured => !string.IsNullOrEmpty(ApiKey);
		public string ApiKey => ConfigurationManager.AppSettings["IntelligentMedia:Google:ApiKey"];
	}
}
