using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace XIVRUS_Updater
{
	public class Downloader
	{
		public delegate void DelegateDownloadComplete();
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		public static void DownloadRelease(string fileUrl, string outputfolder, string fileName, string tempfolder = "./", DelegateDownloadComplete downloadComplete = null, ProgressBar progressBar = null, TextBlock statusTextBlock = null)
		{
			var task = new Task(() =>
			{
				WebClient client = new WebClient();
				string downloadpath = String.Format("{0}/{1}", tempfolder, fileName);
				Logger.Info(String.Format("Download Release url:{0} output:{1} filename: {2}", fileUrl, outputfolder, fileName));
				client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((object sender, DownloadProgressChangedEventArgs e) =>
				{
					double bytesIn = double.Parse(e.BytesReceived.ToString());
					double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
					double percentage = bytesIn / totalBytes * 100;
					if (progressBar != null)
					{
						progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
						{
							progressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
						}));
					}
					if (statusTextBlock != null)
					{
						statusTextBlock.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
						{
							statusTextBlock.Text = "Загружено " + ((int)BytesToKilobytes(e.BytesReceived)).ToString() + "kB из " + ((int)BytesToKilobytes(e.TotalBytesToReceive)).ToString() + "kB";
						}));
					}
				});
				client.DownloadFileCompleted += new AsyncCompletedEventHandler((object sender, AsyncCompletedEventArgs e) =>
				{
					Logger.Info("Download File Completed");
					if (progressBar != null)
					{
						progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
						{
							progressBar.IsIndeterminate = true;
						}));
					}
					if (statusTextBlock != null)
					{
						statusTextBlock.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
						{
							statusTextBlock.Text = "Распаковка";
						}));
					}

					UnZipArchive(downloadpath, outputfolder);
					Logger.Info(String.Format("Delete temp file: {0}", downloadpath));
					File.Delete(downloadpath);

					if (downloadComplete != null)
					{
						downloadComplete.Invoke();
					}

					if (progressBar != null)
					{
						progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
						{
							progressBar.IsIndeterminate = false;
							progressBar.Value = 100;
						}));
					}
					if (statusTextBlock != null)
					{
						statusTextBlock.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
						{
							statusTextBlock.Text = "Готово";
						}));
					}
					Logger.Info("Complete Download Release");
				});
				client.DownloadFileAsync(new Uri(fileUrl), downloadpath);
			});
			if (progressBar != null)
			{
				progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
				{
					progressBar.IsIndeterminate = false;
				}));
			}
			task.Start();
		}

		static void UnZipArchive(string archive, string outputfolder, bool clearfolder = true)
		{
			Logger.Info(String.Format("Start Extract Archive {0} to {1} clear folder: {2}", archive, outputfolder, clearfolder));
			if (!Directory.Exists(outputfolder))
			{
				Directory.CreateDirectory(outputfolder);
				clearfolder = false;
			}
			if (clearfolder)
			{
				DirectoryInfo di = new DirectoryInfo(outputfolder);

				foreach (FileInfo file in di.GetFiles())
				{
					file.Delete();
				}
				foreach (DirectoryInfo dir in di.GetDirectories())
				{
					dir.Delete(true);
				}
			}

			ZipFile.ExtractToDirectory(archive, outputfolder);
			Logger.Info("Extract Archive Done");
		}

		static double BytesToKilobytes(long bytes)
		{
			return bytes / 1024d;
		}
	}
}
