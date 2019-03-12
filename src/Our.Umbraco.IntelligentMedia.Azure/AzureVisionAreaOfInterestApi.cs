using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Our.Umbraco.IntelligentMedia.Azure
{
	public class AzureVisionAreaOfInterestApi : IVisionApi
	{
		
		public async Task<IVisionResponse> MakeRequest(IIntelligentMediaSettings settings, byte[] image)
		{
			var s = settings.Settings<AzureVisionSettings>();
			var subscriptionKey = s.SubscriptionKey;
			var region = s.Region;
			var client = new HttpClient();

			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

			var uri = $"https://{region}.api.cognitive.microsoft.com/vision/v2.0/areaOfInterest";
			
			using (var content = new ByteArrayContent(image))
			{
				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
				return await client.PostAsync(uri, content).ContinueWith(ConvertResponse).Result;
			}
		}

		private async Task<IVisionResponse> ConvertResponse(Task<HttpResponseMessage> httpResponse)
		{
			var json = await httpResponse.Result.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<VisionAreaOfInterestResponse>(json);

			return new AzureVisionAreaOfInterestResponse(response.ToFocalPoint());
		}
		
		public class VisionAreaOfInterestResponse
		{
			public AreaOfInterest AreaOfInterest { get; set; }
            public MetaData MetaData { get; set; }
            public FocalPoint ToFocalPoint()
            {
                var xfp = (AreaOfInterest.X + (decimal)AreaOfInterest.W / 2) / (decimal)MetaData.Width;
                var yfp = (AreaOfInterest.Y + (decimal)AreaOfInterest.H / 2) / (decimal)MetaData.Height;
                return new FocalPoint {X = xfp, Y = yfp};
            }               
		}

        public class MetaData
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public class AreaOfInterest
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int W { get; set; }
            public int H { get; set; }
        }

	}
}
