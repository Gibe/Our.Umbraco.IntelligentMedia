using System.Configuration;

namespace Our.Umbraco.IntelligentMedia.Azure
{
	public class AzureVisionSettings
	{
		public bool IsConfigured => !string.IsNullOrEmpty(SubscriptionKey) &&
		                            !string.IsNullOrEmpty(Region);

		public string SubscriptionKey => ConfigurationManager.AppSettings["IntelligentMedia:Azure:SubscriptionKey"];
		public string Region => ConfigurationManager.AppSettings["IntelligentMedia:Azure:Region"];
	}
}
