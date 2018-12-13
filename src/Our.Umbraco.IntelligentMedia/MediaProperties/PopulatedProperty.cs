using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia.MediaProperties
{
	public class PopulatedProperty : AbstractMediaProperty
	{
		public PopulatedProperty(IMedia media) : base(media, "imPopulated") { }
	}
}
