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
using System.Windows.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace XIVRUS_Updater
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public Config config = null;
		XIVConfigs.PenumbraConfigJson penumbraConfig = null;
		GitHub.ReleaseJson lastRelease = null;
		string currentRusInstall = "0.0";
		bool availableNewVersion = false;
		bool isAutoLaunch = false; // Windows Auto Launch
		bool isXIVAutoLaunch = false; // XIV Launcher Auto-Launch
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		public MainWindow()
		{
			Logger.Info(String.Format("Starting app v{0}", Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion));
			Squirrel.SquirrelAwareApp.HandleEvents();
			var currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) =>
			{
				Exception ex = e.ExceptionObject as Exception;
				Logger.Fatal(String.Format("Unhandled exception occurred:\nMessage {0}\nStack Trace:\n {1}", ex.Message, ex.StackTrace));
			};
			InitializeComponent();
			SettingsPageFrame.Visibility = Visibility.Collapsed;
			DownloadProgressSP.Visibility = Visibility.Collapsed;
			Init();
		}

		public void Init()
		{
			FirstStartPageFrame.Visibility = Visibility.Collapsed;
			SetLoadGridVisibility(true);
			string[] args = Environment.GetCommandLineArgs();
			Logger.Info(String.Format("Launch args: {0}", string.Join(" | ", args)));
			if (!File.Exists(ConfigManager.GetConfigPath()))
			{
				SetLoadGridVisibility(false);
				FirstStartPageFrame.Visibility = Visibility.Visible;
				Logger.Info("Config not found");
				return;
			}
			LoadCofig();
			penumbraConfig = XIVConfigs.PenumbraConfig.LoadConfig();
			LoadCurrentVersion();
			ArgsScenarios(args);
			var task = new Task(() =>
			{
				CheckAppUpdate();
				LoadReleaseInfo();
				CheckVersion();
				SetLoadGridVisibility(false);
				if (isAutoLaunch)
				{
					this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
					{
						AutoLaunchScenario();
					}));
				}
				if (isXIVAutoLaunch)
				{
					this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
					{
						
					}));
				}
				ShowAlertOnTopGameWindow();

			});
			task.Start();
			

		}

		void ArgsScenarios(string[] args)
		{
			foreach (string arg in args)
			{
				if (arg.ToLower() == "-autolaunch")
				{
					isAutoLaunch = true;
					this.WindowState = WindowState.Minimized;
				}
				else if (arg.ToLower() == "-isxivautolaunch")
				{
					isXIVAutoLaunch = true;
					this.WindowState = WindowState.Minimized;
				}
			}
		}

		public void ShowAlertOnTopGameWindow()
		{
			this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
			{
				AlertOnTopGame.AlertOnTopGameWindow wind = new AlertOnTopGame.AlertOnTopGameWindow(config, this);
				wind.Show();
			}));
		}

		public void LoadCofig()
		{
			config = ConfigManager.LoadConfig();
		}

		void AutoLaunchScenario()
		{
			Logger.Info("Started with autolaunch flag");
			
			if (availableNewVersion)
			{
				Logger.Info("AutoLaunch: New version found! Performing actions according to the config");
				if (config.AutoStartup_DownloadAuto)
				{
					this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
					{
						DownloadLastRelease();
					}));
				}
				if (config.AutoStartup_OpenChangeLog)
				{
					Process.Start(new ProcessStartInfo("https://xivrus.ru/download#changelog") { UseShellExecute = true });
				}
				if (config.AutoStartup_ShowWindow)
				{
					this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
					{
						this.WindowState = WindowState.Normal;
					}));
				}
			}
			else if (config.AutoStartup_CloseAfter)
			{
				Logger.Info("AutoLaunch: New version not found. Closing the program due to the CloseAfter parameter");
				Environment.Exit(0);
			}
		}

		void DownloadLastRelease()
		{
			DownloadProgressSP.Visibility = Visibility.Visible;
			string fileurl = GitHub.Releases.GetAssetFileUrlByName(lastRelease, XIVConfigs.XIVRUSMod.GITHUBASSETNAME);
			Logger.Trace(fileurl);
			System.Diagnostics.Trace.WriteLine(fileurl);
			Logger.Info("Start Download");
			if (fileurl == null)
			{
				Logger.Error(String.Format("Could not find file '{0}' in GitHub release assets", XIVConfigs.XIVRUSMod.GITHUBASSETNAME));
				ShowError(String.Format("Не удалось найти файл '{0}' в ассетах релиза GitHub", XIVConfigs.XIVRUSMod.GITHUBASSETNAME), closeapp: false);
				DownloadProgressSP.Visibility = Visibility.Collapsed;
				return;
			}

			DownloadButton.IsEnabled = false;
			Downloader.DelegateDownloadComplete downloadComplete = new Downloader.DelegateDownloadComplete(DownloadComplete);
			Downloader.DownloadRelease(fileurl, XIVConfigs.XIVRUSMod.GetModPath(penumbraConfig.ModDirectory), XIVConfigs.XIVRUSMod.GITHUBASSETNAME, "./", downloadComplete, DownloadProgressBar, DownloadProgressText);
		}

		void CheckAppUpdate()
		{
			try
			{
				Logger.Info("Check Updates");
				AppUpdater.UpdateApp();
			}
			catch (Exception ex)
			{
				Logger.Error(String.Format("Message {0}\nStack Trace:\n {1}", ex.Message, ex.StackTrace));
				ShowError("Произошла ошибка при проверке обновления приложения. Подробнее в run.log", closeapp: false);
			}
		}

		void LoadCurrentVersion()
		{
			if (penumbraConfig == null)
			{
				CurrentVersion_text.Text = "Ошибка конфига Penumbra";
				ShowError("Ошибка конфига Penumbra", "Ошибка Penumbra", false);
				Logger.Error("Penumbra Config error");
				return;
			}
			string penumbraFolder = penumbraConfig.ModDirectory;
			if (!Directory.Exists(penumbraFolder))
			{
				CurrentVersion_text.Text = "Папка Penumbra не найдена";
				ShowError("Папка Penumbra не найдена", "Ошибка Penumbra", false);
				Logger.Error("Penumbra Folder Not Found");
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
				Logger.Error("XIVRUS/meta error");
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
				Logger.Error("Failed to get latest GitHub release information");
				ShowError("Не удалось получить информацию о последнем релизе");
				return;
			}
			this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
			{
				ServerVersion_text.Text = String.Format("Актуальная версия: {0}", lastRelease.TagName);
			}));
		}

		void CheckVersion()
		{
			string currenversion = String.Format("v{0}", currentRusInstall.Replace("-release", ""));
			this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
			{
				ServerVersion_text.Text = String.Format("Актуальная версия: {0}", lastRelease.TagName);
				if (currenversion == lastRelease.TagName)
				{
					availableNewVersion = false;
					Alert_text.Text = "Установлена актуальная версия";
				}
				else
				{
					availableNewVersion = true;
					Alert_text.Text = "Доступна новая версия!";
				}
			}));
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

		
		void DownloadComplete()
		{
			this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
			{
				Logger.Info("Download Complete");
				DownloadButton.IsEnabled = true;
				LoadCurrentVersion();
				if (isAutoLaunch && config.AutoStartup_CloseAfter)
				{
					Logger.Info("AutoLaunch: New version Downloaded! Closing the program due to the CloseAfter parameter");
					Environment.Exit(0);
				}
			}));
		}

		void SetLoadGridVisibility(bool visibility)
		{
			LoadingGrid.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
			{
				if (visibility)
				{
					LoadingGrid.Visibility = Visibility.Visible;
				}
				else
				{
					LoadingGrid.Visibility = Visibility.Collapsed;
				}
			}));
		}

		private void DownloadButton_Click(object sender, RoutedEventArgs e)
		{
			DownloadLastRelease();
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
			e.Handled = true;
		}

		private void SettingsPageFrame_LoadCompleted(object sender, NavigationEventArgs e)
		{
			//SettingsPage sp = (SettingsPage)SettingsPageFrame.Content;
			//sp.mainWindow = this;
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			SettingsPage sp = (SettingsPage)SettingsPageFrame.Content;
			sp.mainWindow = this;
			sp.Init();
			SettingsPageFrame.Visibility = Visibility.Visible;
		}


	}
}
