using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Our.Umbraco.IntelligentMedia.Google
{
  public class GoogleVisionApi : IVisionApi
	{
		public async Task<IVisionResponse> MakeRequest(IIntelligentMediaSettings settings, byte[] image)
		{
			var apiKey = settings.Settings<GoogleVisionSettings>().ApiKey;
			var client = new HttpClient();
			var uri = $"https://vision.googleapis.com/v1/images:annotate?key={apiKey}";

			var imageData = Convert.ToBase64String(image);

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
