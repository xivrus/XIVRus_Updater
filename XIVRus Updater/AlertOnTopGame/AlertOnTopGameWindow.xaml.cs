using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XIVRUS_Updater.XIVConfigs;
using XIVRUS_Updater.XIVRus;

namespace XIVRUS_Updater.AlertOnTopGame
{
	/// <summary>
	/// Логика взаимодействия для AlertOnTopGameWindow.xaml
	/// </summary>
	public partial class AlertOnTopGameWindow : Window
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		ModDisabledAlertPage modDisabledAlertPage;
		ModWarningAlertPage modWarningAlertPage;
		ModUpdatingAlertPage modUpdatingAlertPage;
		public AlertOnTopGameWindow(Config config, MainWindow mainWindow, bool modJustNowDisabled)
		{
			InitializeComponent();
			modDisabledAlertPage = new ModDisabledAlertPage();
			modWarningAlertPage = new ModWarningAlertPage(this, mainWindow);
			modUpdatingAlertPage = new ModUpdatingAlertPage(config, mainWindow);
#if DEBUG
			modJustNowDisabled = true; // DEBUG ONLY
#endif

			mainWindow.Visibility = Visibility.Collapsed;

			string penumbraFolder = mainWindow.penumbraConfig.ModDirectory;
			string modpath = XIVConfigs.XIVRUSMod.GetModPath(penumbraFolder);
			string disabledmetafile = String.Format("{0}/meta.json.disabled", modpath);
			bool moddisabled = File.Exists(disabledmetafile);
			Logger.Info(String.Format("Mod Disabled: {0}", moddisabled));
			if (!XIVConfigs.XIVRUSMod.ModExist(penumbraFolder) && !moddisabled)
			{
				Logger.Error("LaunchWithGame: Mod Not Found. Closing the program.");
				Environment.Exit(0);
				return;
			}
			if (moddisabled && mainWindow.modStatusCode != 0 && !modJustNowDisabled)
			{
				Logger.Error("LaunchWithGame: Mod is disabled and its status is not 0. Closing the program.");
				Environment.Exit(0);
				return;
			}

			if (mainWindow.availableNewVersion && !modJustNowDisabled)
			{
				if (config.LaunchWithGame_DownloadAuto)
				{
					Logger.Info("New version discovered. Opening page modUpdatingAlertPage.");
					MainFrame.Content = modUpdatingAlertPage;
					modUpdatingAlertPage.DownloadLastRelease(mainWindow);
				}
				else
				{
					Logger.Info("LaunchWithGame: A new version of the mod was detected, but it was not stopped due to the LaunchWithGame_DownloadAuto parameter being disabled.");
					if (mainWindow.modStatusCode == 0)
					{
						Logger.Info("LaunchWithGame: modStatus 0 was also detected. Closing the program.");
						Environment.Exit(0);
					}
					setModAlertPagebyStatus(mainWindow.modStatusCode);
				}
			}
			else if (mainWindow.modStatusCode != 0)
			{
				setModAlertPagebyStatus(mainWindow.modStatusCode);
			}

			//MainFrame.Content = modUpdatingAlertPage;
			//MainFrame.Content = modWarningAlertPage;
			//MainFrame.Content = modDisabledAlertPage;
		}

		void setModAlertPagebyStatus(int modStatusCode)
		{
			Logger.Info(String.Format("LaunchWithGame: Mod status detected: {0}", modStatusCode));
			switch (modStatusCode)
			{
				case 1: //Warning
					Logger.Info("Opening page modWarningAlertPage.");
					MainFrame.Content = modWarningAlertPage;
					break;
				case 2: // Disabled
					Logger.Info("Opening page modDisabledAlertPage.");
					MainFrame.Content = modDisabledAlertPage;
					break;
			}
		}

		private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			// Always On Top
			var window = (Window)sender;
			window.Topmost = true;
		}
	}
}
