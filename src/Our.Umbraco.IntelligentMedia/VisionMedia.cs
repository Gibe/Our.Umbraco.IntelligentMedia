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
				NumberOfFaces = Math.Max(NumberOfFaces??0, response.NumberOfFaces??0),
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

			mediaItem.SetValue("imTags",
				String.Join(",", Tags
					.OrderByDescending(t => t.Confidence)
					.Select(t => t.Tag)
					.Distinct()));
			mediaItem.SetValue("imDescription", 
				Descriptions
					.OrderByDescending(d => d.Confidence)
					.First().Tag);
			mediaItem.SetValue("imCategories",
				String.Join(",", Categories
					.OrderByDescending(t => t.Confidence)
					.Select(t => t.Tag.Replace("_", " ").TrimEnd())));
			mediaItem.SetValue("imNumberOfFaces", NumberOfFaces);
			mediaItem.SetValue("imPrimaryColour", PrimaryColour);
			mediaItem.SetValue("imBackgroundColour", BackgroundColour);
			mediaItem.SetValue("imPopulated", true);
			mediaService.Save(mediaItem);
		}
	}
}
