using System;
using System.IO;
using System.Windows.Controls;

namespace XIVRUS_Updater
{
	/// <summary>
	/// Логика взаимодействия для FirstStartPage.xaml
	/// </summary>
	public partial class FirstStartPage : Page
	{
		public FirstStartPage()
		{
			InitializeComponent();
			bool errors = false;
			XIVLauncher_BTN.Visibility = System.Windows.Visibility.Collapsed;
			Dalamud_BTN.Visibility = System.Windows.Visibility.Collapsed;
			Penumbra_BTN.Visibility = System.Windows.Visibility.Collapsed;
			ErrorTextBlocx.Visibility = System.Windows.Visibility.Collapsed;

			string xivLauncherPath = WinDirs.GetXIVLauncherFolder();
			string xivLauncherConfigPath = WinDirs.GetXIVLauncherConfigFolder();

			if (!File.Exists(String.Format("{0}/XIVLauncher.exe", xivLauncherPath)))
			{
				errors = true;
				XIVLauncherCheck_TB.Text = "XIV Launcher: Не Найден!";
				XIVLauncher_BTN.Visibility = System.Windows.Visibility.Visible;
			}
			if (!File.Exists(String.Format("{0}/dalamudConfig.json", xivLauncherConfigPath)))
			{
				errors = true;
				DalamudCheck_TB.Text = "Dalamud: Не Найден!";
				Dalamud_BTN.Visibility = System.Windows.Visibility.Visible;
			}
			if (!File.Exists(String.Format("{0}/pluginConfigs/Penumbra.json", xivLauncherConfigPath)))
			{
				errors = true;
				PenumbraCheck_TB.Text = "Penumbra: Не Найдена!";
				Penumbra_BTN.Visibility = System.Windows.Visibility.Visible;
			}

			if (errors)
			{
				DoneButton.IsEnabled = false;
				ErrorTextBlocx.Visibility = System.Windows.Visibility.Visible;
			}
		}
	}
}
