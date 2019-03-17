using System;
using System.Configuration;

namespace Our.Umbraco.IntelligentMedia.AzureCustomVision
{
	public class AzureCustomVisionSettings
	{
		public bool IsConfigured => !string.IsNullOrEmpty(ProjectId) && 
		                            !string.IsNullOrEmpty(PredictionKey) &&
		                            !string.IsNullOrEmpty(Region);
		public string ProjectId => ConfigurationManager.AppSettings["IntelligentMedia:AzureCustomVision:ProjectId"];
		public string PredictionKey => ConfigurationManager.AppSettings["IntelligentMedia:AzureCustomVision:PredictionKey"];
		public string Region => ConfigurationManager.AppSettings["IntelligentMedia:AzureCustomVision:Region"];
		public decimal MinimumProbability => decimal.Parse(ConfigurationManager.AppSettings["IntelligentMedia:AzureCustomVision:MinimumProbability"]);
	}
}
