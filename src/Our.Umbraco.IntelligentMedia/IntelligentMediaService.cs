using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Our.Umbraco.IntelligentMedia
{
	public class IntelligentMediaService : IIntelligentMediaService
	{
		private static List<IVisionApi> _apis;

		public IEnumerable<IVisionApi> VisionApis()
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
