using System.Collections.Generic;
using System.Linq;

namespace Our.Umbraco.IntelligentMedia.AzureCustomVision
{
	public class AzureCustomVisionResponse : IVisionResponse
	{
		public AzureCustomVisionResponse(AzureCustomVisionApi.CustomVisionApiResponse response, decimal minimumProbability)
		{
			Tags = response.Predictions.Where(t => t.Probability >= minimumProbability / 100).Select(t => new ProbableTag { Confidence = t.Probability, Tag = t.TagName }).ToList();
			Description = new List<ProbableTag>();
			Categories = new List<ProbableTag>();
		}

		public List<ProbableTag> Tags { get; }
		public List<ProbableTag> Categories { get; }
		public List<ProbableTag> Description { get; }
		public int? NumberOfFaces { get; }
		public string PrimaryColour { get; }
		public string BackgroundColour { get; }
	}
}
