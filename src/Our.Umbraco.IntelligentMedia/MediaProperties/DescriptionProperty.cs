using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia.MediaProperties
{
	public class DescriptionProperty : AbstractMediaProperty
	{
		public DescriptionProperty(IMedia media) : base(media, "imDescription") {	}
	}
}
