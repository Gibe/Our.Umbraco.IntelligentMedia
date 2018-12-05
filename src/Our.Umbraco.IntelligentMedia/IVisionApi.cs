using System.Threading.Tasks;

namespace Our.Umbraco.IntelligentMedia
{
	public interface IVisionApi
	{
		Task<IVisionResponse> MakeRequest(IIntelligentMediaSettings settings, byte[] image);
	}
}
