using System.Collections.Generic;
using System.Linq;

namespace Our.Umbraco.IntelligentMedia.Azure
{
	public class AzureVisionResponse : IVisionResponse
	{
		public AzureVisionResponse(AzureVisionApi.VisionResponse response, string json)
		{
			Tags = response.Tags.Select(t => new ProbableTag {Confidence = t.Confidence, Tag = t.Name}).ToList();
			Description = response.Description.Captions.OrderByDescending(c => c.Confidence)
				.Select(t => new ProbableTag {Confidence = t.Confidence, Tag = t.Text}).ToList();
			if (response.Categories != null)
			{
				Categories = response.Categories
					.Select(t => new ProbableTag {Confidence = t.Score, Tag = t.Name.Replace("_", " ")}).ToList();
			}

			NumberOfFaces = response.Faces.Count();
			PrimaryColour = response.Color.DominantColorForeground;
			BackgroundColour = response.Color.DominantColorBackground;
		}

		public List<ProbableTag> Tags { get; }
		public List<ProbableTag> Categories { get; }
		public List<ProbableTag> Description { get; }
		public int? NumberOfFaces { get; }
		public string PrimaryColour { get; }
		public string BackgroundColour { get; }
	}
}
