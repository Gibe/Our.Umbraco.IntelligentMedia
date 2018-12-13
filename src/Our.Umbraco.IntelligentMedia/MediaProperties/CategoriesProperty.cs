using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia.MediaProperties
{
	public class CategoriesProperty : AbstractMediaProperty
	{
		public CategoriesProperty(IMedia media) : base(media, "imCategories") { }
	}
}
