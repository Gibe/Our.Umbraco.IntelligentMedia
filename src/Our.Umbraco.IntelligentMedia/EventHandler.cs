using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.IO;
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
			var service = new IntelligentMediaService((IFileSystem2)FileSystemProviderManager.Current.GetUnderlyingFileSystemProvider("media"), new IntelligentMediaSettings());
			foreach (var media in e.SavedEntities)
			{
				service.UpdateMedia(media);
			}
		}
	}
}
