using System.Collections.Generic;

namespace Our.Umbraco.IntelligentMedia
{
	public interface IIntelligentMediaService
	{
		IEnumerable<IVisionApi> VisionApis();
	}
}
