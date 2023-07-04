using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XIVRUS_Updater
{
	public class Config
	{
		[JsonProperty("version")]
		public int Version { get; set; }
	}

	public static class ConfigManager
	{
		public const string CONFIGFILE = "./Config.json";
	}
}
