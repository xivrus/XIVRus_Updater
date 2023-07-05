using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace XIVRUS_Updater
{
	public class Config
	{
		[JsonProperty("version")]
		public string Version { get; set; }
	}

	public static class ConfigManager
	{
		public const string CONFIGFILE = "./Config.json";

		public static Config LoadConfig()
		{
			if (File.Exists(CONFIGFILE))
			{
				string json = File.ReadAllText(CONFIGFILE);
				try
				{
					Config config = JsonConvert.DeserializeObject<Config>(json);
					return config;
				}
				catch
				{
					return null;
				}
			}
			else
			{
				Config config = GetDefaultConfig();
				SaveConfig(config);
				return config;
			}

			
		}

		public static bool SaveConfig (Config config)
		{
			try
			{
				string json = JsonConvert.SerializeObject(config, Formatting.Indented);
				File.WriteAllText(CONFIGFILE, json);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static Config GetDefaultConfig()
		{
			Config config = new Config();
			config.Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

			return config;
		}
	}
}
