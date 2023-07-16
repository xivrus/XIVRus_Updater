using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Controls;

namespace XIVRUS_Updater
{
	/// <summary>
	/// Логика взаимодействия для FirstStartPage.xaml
	/// </summary>
	public partial class FirstStartPage : Page
	{
		bool checkErrors = false;
		public MainWindow mainWindow = null;

		public FirstStartPage()
		{
			InitializeComponent();
			CheckList();
		}

		void CheckList()
		{
			checkErrors = false;
			XIVLauncher_BTN.Visibility = System.Windows.Visibility.Collapsed;
			Dalamud_BTN.Visibility = System.Windows.Visibility.Collapsed;
			Penumbra_BTN.Visibility = System.Windows.Visibility.Collapsed;
			PenumbraFolder_BTN.Visibility = System.Windows.Visibility.Collapsed;
			ErrorTextBlocx.Visibility = System.Windows.Visibility.Collapsed;
			XIVLauncherCheck_TB.Text = "XIV Launcher: Установлен";
			DalamudCheck_TB.Text = "Dalamud: Установлен";
			PenumbraCheck_TB.Text = "Penumbra: Установлена";
			PenumbraFolderCheck_TB.Text = "Penumbra: Настроена";

			string xivLauncherPath = WinDirs.GetXIVLauncherFolder();
			string xivLauncherConfigPath = WinDirs.GetXIVLauncherConfigFolder();

			if (!File.Exists(String.Format("{0}/XIVLauncher.exe", xivLauncherPath)))
			{
				checkErrors = true;
				XIVLauncherCheck_TB.Text = "XIV Launcher: Не Найден!";
				XIVLauncher_BTN.Visibility = System.Windows.Visibility.Visible;
			}
			if (!File.Exists(String.Format("{0}/dalamudConfig.json", xivLauncherConfigPath)))
			{
				checkErrors = true;
				DalamudCheck_TB.Text = "Dalamud: Не Найден!";
				Dalamud_BTN.Visibility = System.Windows.Visibility.Visible;
			}
			if (!File.Exists(XIVConfigs.PenumbraConfig.GetConfigPath()))
			{
				checkErrors = true;
				PenumbraCheck_TB.Text = "Penumbra: Не Найдена!";
				PenumbraFolderCheck_TB.Text = "Penumbra: Не Найдена!";
				Penumbra_BTN.Visibility = System.Windows.Visibility.Visible;
			}
			else
			{
				XIVConfigs.PenumbraConfigJson pConfig = XIVConfigs.PenumbraConfig.LoadConfig();
				if (!Directory.Exists(pConfig.ModDirectory))
				{
					checkErrors = true;
					PenumbraFolderCheck_TB.Text = "Penumbra: Не Настроена!";
					PenumbraFolder_BTN.Visibility = System.Windows.Visibility.Visible;
				}
			}

			if (checkErrors)
			{
				DoneButton.Content = "Повторить";
				ErrorTextBlocx.Visibility = System.Windows.Visibility.Visible;
			}
			else
			{
				DoneButton.Content = "Готово";
			}
			
		}

		private void DoneButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (checkErrors)
			{
				CheckList();
				return;
			}
			WinDirs.CreateAppShortcutToDesktop();
			ConfigManager.LoadConfig();
			mainWindow.Init();
		}

		private void OpenUrl_Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Button button = (Button)sender;
			if (button == XIVLauncher_BTN || button == Dalamud_BTN)
			{
				Process.Start(new ProcessStartInfo("https://xivrus.ru/guide/install#install-xivlauncher-guide") { UseShellExecute = true });
			}
			else if (button == Penumbra_BTN || button == PenumbraFolder_BTN)
			{
				Process.Start(new ProcessStartInfo("https://xivrus.ru/guide/install#install-penumbra") { UseShellExecute = true });
			}
		}
    }
}
