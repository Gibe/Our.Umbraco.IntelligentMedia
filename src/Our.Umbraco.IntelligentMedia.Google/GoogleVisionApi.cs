using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Umbraco.Core.IO;
using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia.Google
{
  public class GoogleVisionApi : IVisionApi
	{
		private readonly string _apiKey;
		private readonly IFileSystem2 _fileSystem;

		public GoogleVisionApi(IIntelligentMediaSettings settings, IFileSystem2 fileSystem)
		{
			_fileSystem = fileSystem;
			_apiKey = settings.Settings<GoogleVisionSettings>().ApiKey;
		}
		
		public async Task<IVisionResponse> MakeRequest(IMedia media)
		{
			var client = new HttpClient();
			var uri = $"https://vision.googleapis.com/v1/images:annotate?key={_apiKey}";

			var umbracoFileString = media.GetValue<string>("umbracoFile");
			var umbracoFile = JsonConvert.DeserializeObject<UmbracoFileData>(umbracoFileString);
			var imageData = Convert.ToBase64String(GetImageAsByteArray(umbracoFile.Src));

			var request = new AnnotateImageRequests
			{
				Requests = new List<AnnotateImageRequest>
				{
					new AnnotateImageRequest
					{
						Image = new ImageRequest {Content = imageData},
						Features = new List<FeatureRequest>
						{
							new FeatureRequest { Type = "LABEL_DETECTION" },
							new FeatureRequest { Type = "FACE_DETECTION" }
						}
					}
				}
			};
			var content = new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			}), Encoding.UTF8, "application/json");
			return await client.PostAsync(uri, content).ContinueWith(ConvertResponse).Result;
		}

		private async Task<IVisionResponse> ConvertResponse(Task<HttpResponseMessage> httpResponse)
		{
			var json = await httpResponse.Result.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<AnnotateResponse>(json);
			return new GoogleVisionResponse(response, json);
		}

		private byte[] GetImageAsByteArray(string imageFilePath)
		{
			var fileStream = _fileSystem.OpenFile(imageFilePath);

			using (var outStream = new MemoryStream())
			{
				using (var imageFactory = new ImageFactory())
				{
					imageFactory.Load(fileStream)
						.Resize(new Size(1000, 1000))
						.Save(outStream);

				}
				outStream.Position = 0;
				return new BinaryReader(outStream).ReadBytes((int)outStream.Length);
			}
		}
	}

	public class AnnotateResponse
	{
		public AnnotateLabelResponse[] Responses { get; set; }
	}

	public class AnnotateLabelResponse
	{
		public LabelAnnotationResponse[] LabelAnnotations { get; set; }
		public FaceAnnotationResponse[] FaceAnnotations { get; set; }
	}

	public class LabelAnnotationResponse
	{
		public string Mid { get; set; }
		public string Description { get; set; }
		public decimal Score { get; set; }
	}

	public class FaceAnnotationResponse
	{
		public BoundingPolyResponse BoundingPoly { get; set; }
		public BoundingPolyResponse FdBoundingPoly { get; set; }
		public LandmarkResponse[] Landmarks { get; set; }

	}

	public class BoundingPolyResponse
	{
		public VerticeResponse[] Vertices { get; set; }
	}

	public class VerticeResponse
	{
		public int X { get; set; }
		public int Y { get; set; }
	}

	public class LandmarkResponse
	{
		public string Type { get; set; }
		public PositionResponse Position { get; set; }
	}

	public class PositionResponse
	{
		public decimal X { get; set; }
		public decimal Y { get; set; }
		public decimal Z { get; set; }
	}

	public class AnnotateImageRequests
	{
		public IEnumerable<AnnotateImageRequest> Requests { get; set; }
	}

	public class AnnotateImageRequest
	{
		public ImageRequest Image { get; set; }
		public IEnumerable<FeatureRequest> Features { get; set; }
	}

	public class ImageRequest
	{
		public string Content { get; set; }
	}

	public class FeatureRequest
	{
		public string Type { get; set; }
		public int? MaxResults { get; set; }
	}
}
