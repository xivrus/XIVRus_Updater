using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XIVRUS_Updater.XIVConfigs.XIVLauncher
{
	public static class LauncherConfigManager
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		const string CONFIGV3FILENAME = "launcherConfigV3.json";

		public static LauncherConfigV3 LoadConfigV3()
		{
			string configpath = GetConfigV3Path();

			if (!File.Exists(configpath))
			{
				Logger.Error(String.Format("XIVLauncher config not found at path: {0}", configpath));
				return null;
			}

			string configstr = File.ReadAllText(configpath);
			LauncherConfigV3 launcherConfig = JsonConvert.DeserializeObject<LauncherConfigV3>(configstr);
			
			launcherConfig.AddonListData = JsonConvert.DeserializeObject<List<LauncherConfigV3Addon>>(launcherConfig.AddonList);
			return launcherConfig;
		}

		public static bool SaveConfigV3(LauncherConfigV3 launcherConfig)
		{
			string configpath = GetConfigV3Path();
			if (!File.Exists(configpath))
			{
				Logger.Error(String.Format("XIVLauncher config not found at path: {0}", configpath));
				return false;
			}
			try
			{
				launcherConfig.AddonList = JsonConvert.SerializeObject(launcherConfig.AddonListData, Formatting.None);
				string json = JsonConvert.SerializeObject(launcherConfig, Formatting.Indented);
				File.WriteAllText(configpath, json);
				Logger.Info(String.Format("XIVLauncher config was successfully saved to the path {0}", configpath));
				return true;
			}
			catch (Exception ex)
			{
				Logger.Error(String.Format("Message {0}\nStack Trace:\n {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		static string GetConfigV3Path()
		{
			string launcherPath = WinDirs.GetXIVLauncherConfigFolder();
			string configpath = String.Format("{0}/{1}", launcherPath, CONFIGV3FILENAME);
			return configpath;
		}

		public static bool ExistThisAppInAutoLaunch(LauncherConfigV3 launcherConfig)
		{
			List<LauncherConfigV3Addon> addonLists = launcherConfig.AddonListData;
			string apppath = WinDirs.GetAppInstallPath();
			
			foreach (LauncherConfigV3Addon addon in addonLists)
			{
				if (addon.Addon.Path.ToLower() == apppath.ToLower())
				{
					return true;
				}
			}
			return false;
		}

		public static bool AddThisAppInAutoLaunch(LauncherConfigV3 launcherConfig)
		{
			string apppath = WinDirs.GetAppInstallPath();

			LauncherConfigV3Addon addon = new LauncherConfigV3Addon()
			{
				IsEnabled = true,
				Addon = new AddonData()
				{
					Path = apppath,
					CommandLine = "-xivautolaunch",
					RunAsAdmin = false,
					RunOnClose = false,
					KillAfterClose = false,
					Name = "Launch XIVRus Updater" //Launch EXE : XIVRus Updater
				}
			};
			launcherConfig.AddonListData.Add(addon);
			bool saved = SaveConfigV3(launcherConfig);
			return saved;
		}
	}
}
