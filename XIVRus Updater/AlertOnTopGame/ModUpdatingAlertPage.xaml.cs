using NLog;
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
using System.Windows.Threading;
using XIVRUS_Updater.XIVConfigs;

namespace XIVRUS_Updater.AlertOnTopGame
{
	/// <summary>
	/// Логика взаимодействия для ModUpdatingAlertPage.xaml
	/// </summary>
	public partial class ModUpdatingAlertPage : Page
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		MainWindow mwindow;
		public ModUpdatingAlertPage(Config config, MainWindow mainWindow)
		{
			InitializeComponent();
			OkButton.Visibility = Visibility.Collapsed;

			DownloadLastRelease(mainWindow);
		}

		void DownloadLastRelease(MainWindow mainWindow)
		{
			mwindow = mainWindow;
			DownloadProgressSP.Visibility = Visibility.Visible;
			string fileurl = GitHub.Releases.GetAssetFileUrlByName(mainWindow.lastRelease, XIVConfigs.XIVRUSMod.GITHUBASSETNAME);
			Logger.Trace(fileurl);
			System.Diagnostics.Trace.WriteLine(fileurl);
			Logger.Info("Start Download");
			if (fileurl == null)
			{
				Logger.Error(String.Format("Could not find file '{0}' in GitHub release assets", XIVConfigs.XIVRUSMod.GITHUBASSETNAME));
				MessageBox.Show(String.Format("Не удалось найти файл '{0}' в ассетах релиза GitHub", XIVConfigs.XIVRUSMod.GITHUBASSETNAME), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				DownloadProgressSP.Visibility = Visibility.Collapsed;
				Logger.Info("Program closes due to a file error. More details above");
				Environment.Exit(0);
				return;
			}

			mainWindow.IsEnabled = false;
			Downloader.DelegateDownloadComplete downloadComplete = new Downloader.DelegateDownloadComplete(DownloadComplete);
			Downloader.DownloadRelease(fileurl, XIVConfigs.XIVRUSMod.GetModPath(mainWindow.penumbraConfig.ModDirectory), XIVConfigs.XIVRUSMod.GITHUBASSETNAME, "./", downloadComplete, DownloadProgressBar, DownloadProgressText);
		}

		void DownloadComplete()
		{
			this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
			{
				Logger.Info("Download Complete");
				//Alert_text.Text = "Перевод успешно обновлён!";
				mwindow.IsEnabled = true;
				OkButton.Visibility = Visibility.Visible;
				TopTitle.Text = "XIV Rus обновлён!";
				TextBlock1.Text = "Обновление успешно загружено и установлено!";
				TextBlock2.Text = "ТРЕБУЕТСЯ ПЕРЕЗАПУСК ИГРЫ!";
				TextBlock2.FontWeight = FontWeights.Bold;
			}));
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Не забудьте перезапустить игру!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
			Logger.Info("Closing the program after updating");
			Environment.Exit(0);
		}
	}
}
