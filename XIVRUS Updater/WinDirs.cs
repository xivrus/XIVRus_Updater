using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XIVRUS_Updater
{
	public static class WinDirs
	{
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
			string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\XIVRUS Updater.lnk";
			IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
			shortcut.Description = "Запустить XIVRUS Updater";
			shortcut.TargetPath = String.Format("{0}/XIVRUSUpdater/XIVRUS Updater.exe", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			shortcut.Save();
		}
	}
}
