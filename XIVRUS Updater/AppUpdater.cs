using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel;


namespace XIVRUS_Updater
{
	public static class AppUpdater
	{
		public static void UpdateApp()
		{
			using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/FAR747/XIVRUS_Updater").Result)
			{
				if (mgr.IsInstalledApp)
				{
					ReleaseEntry newversion = mgr.UpdateApp().Result;
					if (newversion != null)
					{
						UpdateManager.RestartApp();
					}
				}
			}
		}
	}
}
