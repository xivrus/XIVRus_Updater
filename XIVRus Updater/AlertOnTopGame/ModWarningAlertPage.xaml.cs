using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XIVRUS_Updater.AlertOnTopGame
{
	/// <summary>
	/// Логика взаимодействия для ModWarningAlertPage.xaml
	/// </summary>
	public partial class ModWarningAlertPage : Page
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		AlertOnTopGameWindow alertOnTopGameWindow;
		MainWindow mainWindow;
		public ModWarningAlertPage(AlertOnTopGameWindow alertwindow, MainWindow main)
		{
			InitializeComponent();
			alertOnTopGameWindow = alertwindow;
			mainWindow = main;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			Logger.Info("User chose to continue");
			Environment.Exit(0);
		}

		private void DisableModButton_Click(object sender, RoutedEventArgs e)
		{
			bool dis = mainWindow.DisableMod();
			if (dis)
			{
				Logger.Info("Mod has been successfully disabled. Exit");
				MessageBox.Show("Мод успешно отключён!\n\nПЕРЕЗАПУСТИТЕ ИГРУ!", "Отключение мода", MessageBoxButton.OK, MessageBoxImage.Information);
				DisableModButton.IsEnabled = false;
				Environment.Exit(0);
			}
			else
			{
				MessageBox.Show("Не удалось отключить мод", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void OpenXIVRusUpdaterButton_Click(object sender, RoutedEventArgs e)
		{
			Logger.Info("Open XIVRus Updater. Hide this window");
			alertOnTopGameWindow.Close();
			mainWindow.ShowWindow();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			WinDirs.OpenDiscordURL();
		}
	}
}
