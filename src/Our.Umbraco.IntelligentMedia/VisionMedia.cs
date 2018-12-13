using Our.Umbraco.IntelligentMedia.MediaProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Our.Umbraco.IntelligentMedia
{
	public class VisionMedia
	{
		public VisionMedia()
		{
			Tags = new List<ProbableTag>();
			Categories = new List<ProbableTag>();
			Descriptions = new List<ProbableTag>();
		}

		private string Name => Descriptions.OrderByDescending(d => d.Confidence).First().Tag;
		private List<ProbableTag> Tags { get; set; }
		private List<ProbableTag> Categories { get; set; }
		private List<ProbableTag> Descriptions { get; set; }
		private int? NumberOfFaces { get; set; }
		private string PrimaryColour { get; set; }
		private string BackgroundColour { get; set; }
		
		public VisionMedia Merge(IVisionResponse response)
		{
			var tags = Tags;
			tags.AddRange(response.Tags);
			
			var categories = Categories;
			categories.AddRange(response.Categories);

			var descriptions = Descriptions;
			descriptions.AddRange(response.Description);
			
			return new VisionMedia
			{
				Tags = tags,
				Categories = categories,
				Descriptions = descriptions,
				NumberOfFaces = Math.Max(NumberOfFaces ?? 0, response.NumberOfFaces ?? 0),
				PrimaryColour = PrimaryColour ?? response.PrimaryColour,
				BackgroundColour = BackgroundColour ?? response.BackgroundColour
			};
		}

		public void UpdateUmbracoMedia(IMedia mediaItem, IMediaService mediaService, IIntelligentMediaSettings settings)
		{

			if (settings.Settings<GlobalSettings>().OverwriteName)
			{
				mediaItem.Name = Name;
			}

			new TagsProperty(mediaItem).SetValueIfExists(string.Join(",", Tags.OrderByDescending(t => t.Confidence).Select(t => t.Tag).Distinct()));
			new DescriptionProperty(mediaItem).SetValueIfExists(Descriptions.OrderByDescending(d => d.Confidence).First().Tag);
			new CategoriesProperty(mediaItem).SetValueIfExists(string.Join(",", Categories.OrderByDescending(t => t.Confidence).Select(t => t.Tag.Replace("_", " ").TrimEnd())));
			new NumberOfFacesProperty(mediaItem).SetValueIfExists(NumberOfFaces);
			new PrimaryColourProperty(mediaItem).SetValueIfExists(PrimaryColour);
			new BackgroundColourProperty(mediaItem).SetValueIfExists(BackgroundColour);
			new PopulatedProperty(mediaItem).SetValueIfExists(true);
			mediaService.Save(mediaItem);
		}
	}
}
