using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XIVRUS_Updater
{
	/// <summary>
	/// Логика взаимодействия для SettingsPage.xaml
	/// </summary>
	public partial class SettingsPage : Page
	{
		public MainWindow mainWindow = null;
		Config config = null;
		public SettingsPage()
		{
			InitializeComponent();
			AppVersionTB.Text = String.Format("Version: {0}", Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);
		}

		public void Init()
		{
			config = mainWindow.config;
			AddAutoStartupCB.IsChecked = WinDirs.CheckWindowsStartUp();
			CloseAfterAutoStartupCB.IsChecked = config.AutoStartup_CloseAfter;
			DownloadAutoStartupCB.IsChecked = config.AutoStartup_DownloadAuto;
			ShowWindowAutoStartupCB.IsChecked = config.AutoStartup_ShowWindow;
			OpenChangeLogAutoStartupCB.IsChecked = config.AutoStartup_OpenChangeLog;

			DownloadAutoLaunchWithGameCB.IsChecked = config.LaunchWithGame_DownloadAuto;

		}

		private void CancelSettingsButton_Click(object sender, RoutedEventArgs e)
		{
			mainWindow.LoadCofig();
			mainWindow.SettingsPageFrame.Visibility = Visibility.Collapsed;
        }

		private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
		{
			mainWindow.config = config;
			ConfigManager.SaveConfig(config);
			mainWindow.SettingsPageFrame.Visibility = Visibility.Collapsed;
		}

		private void AddAutoStartupCB_Click(object sender, RoutedEventArgs e)
		{
			if ((bool)AddAutoStartupCB.IsChecked)
			{
				WinDirs.WindowsStartUp(true);
			}
			else
			{
				WinDirs.WindowsStartUp(false);
			}
        }

		private void CloseAfterAutoStartupCB_Click(object sender, RoutedEventArgs e)
		{
			config.AutoStartup_CloseAfter = (bool)CloseAfterAutoStartupCB.IsChecked;
		}

		private void DownloadAutoStartupCB_Click(object sender, RoutedEventArgs e)
		{
			config.AutoStartup_DownloadAuto = (bool)DownloadAutoStartupCB.IsChecked;
		}

		private void ShowWindowAutoStartupCB_Click(object sender, RoutedEventArgs e)
		{
			config.AutoStartup_ShowWindow = (bool)ShowWindowAutoStartupCB.IsChecked;
		}

		private void OpenChangeLogAutoStartupCB_Click(object sender, RoutedEventArgs e)
		{
			config.AutoStartup_OpenChangeLog = (bool)OpenChangeLogAutoStartupCB.IsChecked;
		}

		private void AddAutoLaunchWithGameCB_Click(object sender, RoutedEventArgs e)
		{
			if ((bool)AddAutoLaunchWithGameCB.IsChecked)
			{
				//enable
			}
			else
			{
				MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отключить запуск программы при старте игры?\n\nXIVRus Updater не влияет на производительность игры. Программа закроется автоматически, если не найдёт новой версии или статуса перевода.\n\nНастоятельно не рекомендуется отключать этот параметр!", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Yes)
				{
					//disable
				}
				else
				{
					AddAutoLaunchWithGameCB.IsChecked = true;

				}
			}
        }

		private void DownloadAutoLaunchWithGameCB_Click(object sender, RoutedEventArgs e)
		{
			config.LaunchWithGame_DownloadAuto = (bool)DownloadAutoLaunchWithGameCB.IsChecked;
		}
	}
}
