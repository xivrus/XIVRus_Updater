using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Config config = null;
		XIVConfigs.PenumbraConfigJson penumbraConfig = null;
		GitHub.ReleaseJson lastRelease = null;
		string currentRusInstall = "0.0";
		public MainWindow()
		{
			InitializeComponent();
			DownloadProgressSP.Visibility = Visibility.Collapsed;
			Init();
		}

		public void Init()
		{
			FirstStartPageFrame.Visibility = Visibility.Collapsed;
			if (!File.Exists(ConfigManager.CONFIGFILE))
			{
				FirstStartPageFrame.Visibility = Visibility.Visible;
				return;
			}
			config = ConfigManager.LoadConfig();
			penumbraConfig = XIVConfigs.PenumbraConfig.LoadConfig();
			LoadCurrentVersion();
			LoadReleaseInfo();
			CheckVersion();

		}

		void LoadCurrentVersion()
		{
			if (penumbraConfig == null)
			{
				CurrentVersion_text.Text = "Ошибка конфига Penumbra";
				ShowError("Ошибка конфига Penumbra", "Ошибка Penumbra", false);
				return;
			}
			string penumbraFolder = penumbraConfig.ModDirectory;
			if (!Directory.Exists(penumbraFolder))
			{
				CurrentVersion_text.Text = "Папка Penumbra не найдена";
				ShowError("Папка Penumbra не найдена", "Ошибка Penumbra", false);
				return;
			}
			if (!XIVConfigs.XIVRUSMod.ModExist(penumbraFolder))
			{
				CurrentVersion_text.Text = "Текущая версия: Мод отсутствует";
				return;
			}
			XIVConfigs.PenumbraModMetaJson modMeta = XIVConfigs.PenumbraModMeta.GetMetaByDirectory(XIVConfigs.XIVRUSMod.GetModPath(penumbraFolder));
			if (modMeta == null)
			{
				CurrentVersion_text.Text = "Ошибка XIVRUS/meta.json";
				ShowError("Файл метаданных мода XIVRus повреждён", "Ошибка XIVRUS/meta.json", false);
				return;
			}
			CurrentVersion_text.Text = String.Format("Текущая версия: v{0}", modMeta.Version.Replace("-release", ""));
			currentRusInstall = modMeta.Version;
			CurrentVersion_text.ToolTip = modMeta.Version;
		}

		void LoadReleaseInfo()
		{
			lastRelease = GitHub.Releases.GetLastRelease();
			if (lastRelease == null)
			{
				ShowError("Не удалось получить информацию о последнем релизе");
				return;
			}
			ServerVersion_text.Text = String.Format("Актуальная версия: {0}", lastRelease.TagName);
		}

		void CheckVersion()
		{
			string currenversion = String.Format("v{0}", currentRusInstall.Replace("-release", ""));
			if (currenversion == lastRelease.TagName)
			{
				Alert_text.Text = "Установлена актуальная версия";
			}
			else
			{
				Alert_text.Text = "Доступна новая версия!";
			}
		}

		private void FirstStartPageFrame_LoadCompleted(object sender, NavigationEventArgs e)
		{
			FirstStartPage fsp = (FirstStartPage)FirstStartPageFrame.Content;
			fsp.mainWindow = this;
		}

		void ShowError(string text, string title = "Ошибка", bool closeapp = true)
		{
			MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
			if (closeapp)
			{
				Environment.Exit(0);
			}
		}

		private void DownloadButton_Click(object sender, RoutedEventArgs e)
		{
			DownloadProgressSP.Visibility = Visibility.Visible;
			string fileurl = GitHub.Releases.GetAssetFileUrlByName(lastRelease, XIVConfigs.XIVRUSMod.GITHUBASSETNAME);
			System.Diagnostics.Trace.WriteLine(fileurl);

			if (fileurl == null)
			{
				ShowError(String.Format("Не удалось найти файл '{0}' в ассетах релиза GitHub", XIVConfigs.XIVRUSMod.GITHUBASSETNAME), closeapp: false);
				DownloadProgressSP.Visibility = Visibility.Collapsed;
				return;
			}

			DownloadButton.IsEnabled = false;
			Downloader.DownloadRelease(fileurl, XIVConfigs.XIVRUSMod.GetModPath(penumbraConfig.ModDirectory), XIVConfigs.XIVRUSMod.GITHUBASSETNAME, "./", DownloadProgressBar, DownloadProgressText, DownloadButton);
		}
	}
}
