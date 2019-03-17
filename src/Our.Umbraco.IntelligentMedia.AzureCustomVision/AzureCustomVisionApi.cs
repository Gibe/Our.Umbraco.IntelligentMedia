using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Our.Umbraco.IntelligentMedia.AzureCustomVision
{
	public class AzureCustomVisionApi : IVisionApi
	{
		public async Task<IVisionResponse> MakeRequest(IIntelligentMediaSettings settings, byte[] image)
		{
			var s = settings.Settings<AzureCustomVisionSettings>();
			if (s.IsConfigured)
			{
				var predictionKey = s.PredictionKey;
				var region = s.Region;
				var projectId = s.ProjectId;
				var client = new HttpClient();

				client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

				var uri = $"https://{region}.api.cognitive.microsoft.com/customvision/v2.0/Prediction/{projectId}/image";

				using (var content = new ByteArrayContent(image))
				{
					content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
					return await client.PostAsync(uri, content).ContinueWith(c => ConvertResponse(c, s.MinimumProbability)).Result;
				}
			}
			return null;
		}

		private async Task<IVisionResponse> ConvertResponse(Task<HttpResponseMessage> httpResponse, decimal minimumProbability)
		{
			var json = await httpResponse.Result.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<CustomVisionApiResponse>(json);

			return new AzureCustomVisionResponse(response, minimumProbability);
		}

		public class CustomVisionApiResponse
		{
			public string Id { get; set; }
			public string Project { get; set; }
			public string Iteration { get; set; }
			public string Created { get; set; }
			public IEnumerable<Prediction> Predictions { get; set; }
		}

		public class Prediction
		{
			public decimal Probability { get; set; }
			public string TagId { get; set; }
			public string TagName { get; set; }
			public Region Region { get; set; }
		}

		public class Region
		{
			public decimal Left { get; set; }
			public decimal Top { get; set; }
			public decimal Width { get; set; }
			public decimal Height { get; set; }
		}
	}
}
