using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = System.IO.File;

namespace XIVRUS_Updater
{
	public static class WinDirs
	{
		const string STARTUPSHORTCUT = "XIVRusUpdater.lnk";
		const string DISCORDURL = "https://discord.gg/e6z2VY8fNj";
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		public static string GetXIVLauncherFolder()
		{
			return String.Format("{0}/XIVLauncher", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
		}

		public static string GetXIVLauncherConfigFolder()
		{
			return String.Format("{0}/XIVLauncher", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
		}

		public static void CreateAppShortcutToDesktop()
		{
			object shDesktop = (object)"Desktop";
			WshShell shell = new WshShell();
			string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\XIVRus Updater.lnk";
			IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
			shortcut.Description = "Запустить XIVRus Updater";
			shortcut.TargetPath = String.Format("{0}/XIVRusUpdater/XIVRus Updater.exe", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			shortcut.Save();
		}

		public static void WindowsStartUp(bool add = true)
		{
			string startupfolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
			string shortcutAddress = String.Format("{0}/{1}", startupfolder, STARTUPSHORTCUT);
			if (add)
			{
				Logger.Info(String.Format("Add WindowsStartup: {0}", shortcutAddress));	
				WshShell shell = new WshShell();
				IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
				shortcut.Description = "Запустить XIVRus Updater";
				shortcut.TargetPath = String.Format("{0}/XIVRusUpdater/XIVRus Updater.exe", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
				shortcut.Arguments = "-autolaunch";
				shortcut.Save();
			}
			else
			{
				if (File.Exists(shortcutAddress))
				{
					Logger.Info(String.Format("Delete WindowsStartup: {0}", shortcutAddress));
					File.Delete(shortcutAddress);
				}
			}
		}

		public static bool CheckWindowsStartUp()
		{
			string startupfolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
			string shortcutAddress = String.Format("{0}/{1}", startupfolder, STARTUPSHORTCUT);
			return File.Exists(shortcutAddress);
		}

		public static string GetAppInstallPath()
		{
			return String.Format("{0}\\XIVRusUpdater\\XIVRus Updater.exe", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
		}

		public static void OpenDiscordURL()
		{
			Process.Start(new ProcessStartInfo(DISCORDURL) { UseShellExecute = true });
		}
	}
}
