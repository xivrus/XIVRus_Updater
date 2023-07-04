using System;
using System.Collections.Generic;
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
	}
}
