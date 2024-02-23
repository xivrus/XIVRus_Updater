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
	/// Логика взаимодействия для ModDisabledAlertPage.xaml
	/// </summary>
	public partial class ModDisabledAlertPage : Page
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		AlertOnTopGameWindow alertOnTopGameWindow;
		MainWindow mainWindow;
		public ModDisabledAlertPage(AlertOnTopGameWindow alertwindow, MainWindow main)
		{
			InitializeComponent();
			alertOnTopGameWindow = alertwindow;
			mainWindow = main;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			Logger.Info("User chose to continue");
			MessageBox.Show("ПЕРЕЗАПУСТИТЕ ИГРУ!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
			Environment.Exit(0);
		}

		private void OpenXIVRusUpdaterButton_Click(object sender, RoutedEventArgs e)
		{
			Logger.Info("Open XIVRus Updater. Hide this window");
			alertOnTopGameWindow.Close();
			mainWindow.ShowWindow();
		}

		private void OpenDiscordButton_Click(object sender, RoutedEventArgs e)
		{
			WinDirs.OpenDiscordURL();
		}
	}
}
