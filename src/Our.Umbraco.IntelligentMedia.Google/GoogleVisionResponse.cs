using System.Collections.Generic;
using System.Linq;

namespace Our.Umbraco.IntelligentMedia.Google
{
	public class GoogleVisionResponse : IVisionResponse
	{
		public GoogleVisionResponse(AnnotateResponse response, string json)
		{
			Tags = response.Responses.First().LabelAnnotations
				.Select(t => new ProbableTag {Confidence = t.Score, Tag = t.Description}).ToList();
			NumberOfFaces = response.Responses.First().FaceAnnotations != null ? response.Responses.First().FaceAnnotations.Length : 0;
			Description = new List<ProbableTag>();
			BackgroundColour = null;
			PrimaryColour = null;
			Categories = new List<ProbableTag>();
		}

		public List<ProbableTag> Tags { get; }
		public List<ProbableTag> Categories { get; }
		public List<ProbableTag> Description { get; }
		public int? NumberOfFaces { get; }
		public string PrimaryColour { get; }
		public string BackgroundColour { get; }
        public FocalPoint FocalPoint => null;
    }
}
