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

		[JsonProperty("autostartup_CloseAfter")]
		public bool AutoStartup_CloseAfter { get; set; } = true;
		[JsonProperty("autostartup_DownloadAuto")]
		public bool AutoStartup_DownloadAuto { get; set; } = false;
		[JsonProperty("autostartup_ShowWindow")]
		public bool AutoStartup_ShowWindow { get; set; } = true;
		[JsonProperty("autostartup_OpenChangeLog")]
		public bool AutoStartup_OpenChangeLog { get; set; } = false;

	}

	public static class ConfigManager
	{
		public const string CONFIGFILE = "Config.json";
		public static string currentconfigpath = GetConfigPath();
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		public static Config LoadConfig()
		{
			
			if (File.Exists(currentconfigpath))
			{
				string json = File.ReadAllText(currentconfigpath);
				try
				{
					Config config = JsonConvert.DeserializeObject<Config>(json);
					config = CheckConfig(config);
					return config;
				}
				catch (Exception ex)
				{
					Logger.Error(String.Format("Message {0}\nStack Trace:\n {1}", ex.Message, ex.StackTrace));
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
				File.WriteAllText(currentconfigpath, json);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Error(String.Format("Message {0}\nStack Trace:\n {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public static Config CheckConfig(Config config)
		{
			Config defaultConfig = GetDefaultConfig();
			if (config.Version == defaultConfig.Version)
			{
				return config;
			}
			Logger.Info(String.Format("Checking the config due to version mismatch. Current version: {0}, Version in config: {1}", defaultConfig.Version, config.Version));
			config.Version = defaultConfig.Version;
			SaveConfig(config);
			return config;
		}

		public static Config GetDefaultConfig()
		{
			Config config = new Config();
			config.Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
			
			//config.AutoStartup_CloseAfter = true;
			//config.AutoStartup_DownloadAuto = false;
			//config.AutoStartup_ShowWindow = true;
			//config.AutoStartup_OpenChangeLog = false;

			return config;
		}

		public static string GetConfigPath()
		{
			string localfolder = String.Format("{0}/{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XIVRusUpdater");
			if (!Directory.Exists(localfolder))
			{
				Directory.CreateDirectory(localfolder);
			}
			return String.Format("{0}/{1}", localfolder, CONFIGFILE);
		}
	}
}
