using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia.MediaProperties
{
	public class BackgroundColourProperty : AbstractMediaProperty
	{
		public BackgroundColourProperty(IMedia media) : base(media, "imBackgroundColour")
		{
		}
	}
}
