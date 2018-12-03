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
			Json = "";
		}

		private string Name => Descriptions.OrderByDescending(d => d.Confidence).First().Tag;
		private List<ProbableTag> Tags { get; set; }
		private List<ProbableTag> Categories { get; set; }
		private List<ProbableTag> Descriptions { get; set; }
		private int? NumberOfFaces { get; set; }
		private string PrimaryColour { get; set; }
		private string BackgroundColour { get; set; }
		private string Json { get; set; }
		

		public VisionMedia Merge(IVisionResponse response)
		{
			var tags = Tags;
			tags.AddRange(response.Tags);
			
			var categories = Categories;
			categories.AddRange(response.Categories);

			var descriptions = Descriptions;
			descriptions.AddRange(response.Description);

			var json = Json + "\r\n" + response.Json;
			
			return new VisionMedia
			{
				Tags = tags,
				Categories = categories,
				Descriptions = descriptions,
				NumberOfFaces = Math.Max(NumberOfFaces??0, response.NumberOfFaces??0),
				Json = json
			};
		}

		public void UpdateUmbracoMedia(IMedia mediaItem, IMediaService mediaService)
		{

			mediaItem.Name = Name;
			mediaItem.SetValue("tags",
				String.Join(",", Tags
					.Select(t => t.Tag)
					.Distinct()));
			mediaItem.SetValue("description", 
				Descriptions
					.OrderByDescending(d => d.Confidence)
					.First().Tag);
			mediaItem.SetValue("categories",
				String.Join(",", Categories
					.Select(t => t.Tag.Replace("_", " ").TrimEnd())));
			mediaItem.SetValue("numberOfFaces", NumberOfFaces);
			mediaItem.SetValue("primaryColour", PrimaryColour);
			mediaItem.SetValue("backgroundColour", BackgroundColour);
			mediaItem.SetValue("json", Json);
			mediaService.Save(mediaItem);
		}
	}
}
