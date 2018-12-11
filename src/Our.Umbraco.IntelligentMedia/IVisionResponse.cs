using System.Collections.Generic;

namespace Our.Umbraco.IntelligentMedia
{
	public interface IVisionResponse
	{
		List<ProbableTag> Tags { get; }
		List<ProbableTag> Categories { get;}
		List<ProbableTag> Description { get; }
		int? NumberOfFaces { get; }
		string PrimaryColour { get; }
		string BackgroundColour { get; }
	}
}
