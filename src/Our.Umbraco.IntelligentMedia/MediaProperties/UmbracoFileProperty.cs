using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia.MediaProperties
{
	public class UmbracoFileProperty : AbstractMediaProperty
	{
		public UmbracoFileProperty(IMedia media) : base(media, "umbracoFile") { }
	}
}
