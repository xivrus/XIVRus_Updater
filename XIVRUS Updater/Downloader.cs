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
		public static void DownloadRelease(string fileUrl, string outputfolder, string fileName, string tempfolder = "./", DelegateDownloadComplete downloadComplete = null, ProgressBar progressBar = null, TextBlock statusTextBlock = null)
		{
			var task = new Task(() =>
			{
				WebClient client = new WebClient();
				string downloadpath = String.Format("{0}/{1}", tempfolder, fileName);
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
		}

		static double BytesToKilobytes(long bytes)
		{
			return bytes / 1024d;
		}
	}
}
