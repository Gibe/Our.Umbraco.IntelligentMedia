using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Our.Umbraco.IntelligentMedia
{
	public class MediaEvents : ApplicationEventHandler
	{
		protected override void ApplicationStarted(
			UmbracoApplicationBase umbracoApplication,
			ApplicationContext applicationContext)
		{
			MediaService.Saved += MediaServiceSaved;
		}

		private async void MediaServiceSaved(IMediaService sender, SaveEventArgs<IMedia> e)
		{
			var service = new IntelligentMediaService();
			foreach (var mediaItem in e.SavedEntities)
			{
				if (!string.IsNullOrEmpty(mediaItem.GetValue<string>("json")))
				{
					continue;
				}

				var visionMedia = new VisionMedia();
				foreach (var api in service.VisionApis())
				{
					var visionResponse = await api.MakeRequest(mediaItem).ConfigureAwait(false);
					visionMedia = visionMedia.Merge(visionResponse);
				}

				visionMedia.UpdateUmbracoMedia(mediaItem, ApplicationContext.Current.Services.MediaService);
			}
		}
	}
}
