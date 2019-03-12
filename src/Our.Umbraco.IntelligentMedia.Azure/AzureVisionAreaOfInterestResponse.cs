using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.IntelligentMedia.Azure
{
    public class AzureVisionAreaOfInterestResponse : IVisionResponse
    {
        public AzureVisionAreaOfInterestResponse(FocalPoint focalPoint)
        {
            FocalPoint = focalPoint;
        }

        public List<ProbableTag> Tags => new List<ProbableTag>();
        public List<ProbableTag> Categories => new List<ProbableTag>();
        public List<ProbableTag> Description => new List<ProbableTag>();
        public int? NumberOfFaces => null;
        public string PrimaryColour => null;
        public string BackgroundColour => null;
        public FocalPoint FocalPoint { get; }
    }
}
