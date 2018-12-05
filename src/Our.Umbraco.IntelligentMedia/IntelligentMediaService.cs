using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ImageProcessor;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia
{
	public class IntelligentMediaService : IIntelligentMediaService
	{
		private static List<IVisionApi> _apis;
		private readonly IFileSystem2 _fileSystem;
		private readonly IIntelligentMediaSettings _intelligentMediaSettings;

		public IntelligentMediaService(IFileSystem2 fileSystem, IIntelligentMediaSettings settings)
		{
			_fileSystem = fileSystem;
			_intelligentMediaSettings = settings;
		}

		public async void UpdateMedia(IMedia media)
		{
			if (!string.IsNullOrEmpty(media.GetValue<string>("json")))
			{
				return;
			}

			var umbracoFileString = media.GetValue<string>("umbracoFile");
			var umbracoFile = JsonConvert.DeserializeObject<UmbracoFileData>(umbracoFileString);
			var image = GetImageAsByteArray(umbracoFile.Src);

			var visionMedia = new VisionMedia();
			foreach (var api in VisionApis())
			{
				var visionResponse = await api.MakeRequest(_intelligentMediaSettings ,image).ConfigureAwait(false);
				visionMedia = visionMedia.Merge(visionResponse);
			}

			visionMedia.UpdateUmbracoMedia(media, ApplicationContext.Current.Services.MediaService);
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

		private IEnumerable<IVisionApi> VisionApis()
		{
			if (_apis == null)
			{
				var visionApiType = typeof(IVisionApi);
				var types = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(s => s.GetTypes())
					.Where(p => visionApiType.IsAssignableFrom(p));

				_apis = types.Select(t => (IVisionApi)DependencyResolver.Current.GetService(t))
					.ToList();
			}

			return _apis;
		}
	}
}
