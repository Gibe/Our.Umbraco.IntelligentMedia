using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia.MediaProperties
{
	public class PrimaryColourProperty : AbstractMediaProperty
	{
		public PrimaryColourProperty(IMedia media) : base(media, "imPrimaryColour")
		{
		}
	}
}
