using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Our.Umbraco.IntelligentMedia.Azure
{
	public class AzureVisionApi : IVisionApi
	{
		
		public async Task<IVisionResponse> MakeRequest(IIntelligentMediaSettings settings, byte[] image)
		{
			var s = settings.Settings<AzureVisionSettings>();
			if (s.IsConfigured)
			{
				var subscriptionKey = s.SubscriptionKey;
				var region = s.Region;
				var client = new HttpClient();
				var queryString = HttpUtility.ParseQueryString(string.Empty);

				client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

				queryString["visualFeatures"] = "Tags,Description,Categories,Faces,Color";
				queryString["language"] = "en";

				var uri = $"https://{region}.api.cognitive.microsoft.com/vision/v1.0/analyze?{queryString}";

				using (var content = new ByteArrayContent(image))
				{
					content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
					return await client.PostAsync(uri, content).ContinueWith(ConvertResponse).Result;
				}
			}

			return null;
		}

		private async Task<IVisionResponse> ConvertResponse(Task<HttpResponseMessage> httpResponse)
		{
			var json = await httpResponse.Result.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<VisionResponse>(json);

			return new AzureVisionResponse(response, json);
		}
		
		public class VisionResponse
		{
			public Tag[] Tags { get; set; }
			public Category[] Categories { get; set; }
			public Description Description { get; set; }
			public Colors Color { get; set; }
			public IEnumerable<Face> Faces { get; set; }
		}

		public class Face
		{
			public int Age { get; set; }
			public string Gender { get; set; }
		}

		public class Colors
		{
			public string DominantColorForeground { get; set; }
			public string DominantColorBackground { get; set; }
		}

		public class Tag
		{
			public string Name { get; set; }
			public decimal Confidence { get; set; }
		}

		public class Category
		{
			public string Name { get; set; }
			public decimal Score { get; set; }
		}

		public class Description
		{
			public string[] Tags { get; set; }
			public Caption[] Captions { get; set; }
		}

		public class Caption
		{
			public string Text { get; set; }
			public decimal Confidence { get; set; }
		}
	}
}
