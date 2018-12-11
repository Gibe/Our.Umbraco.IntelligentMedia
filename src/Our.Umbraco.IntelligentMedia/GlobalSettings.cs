using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.IntelligentMedia
{
	public class GlobalSettings
	{
		public bool OverwriteName => Convert.ToBoolean(ConfigurationManager.AppSettings["IntelligentMedia:OverwriteName"] ?? "false");
	}
}
