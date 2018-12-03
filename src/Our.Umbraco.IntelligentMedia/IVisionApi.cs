using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia
{
	public interface IVisionApi
	{
		Task<IVisionResponse> MakeRequest(IMedia media);
	}
}
