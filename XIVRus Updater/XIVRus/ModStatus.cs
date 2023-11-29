using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XIVRUS_Updater.GitHub;

namespace XIVRUS_Updater.XIVRus
{
	public partial class ModStatusJson
	{
		//[JsonProperty("status_values")]
		//public string StatusValues { get; set; }

		[JsonProperty("status")]
		public int Status { get; set; }
	}

	public class ModStatus
	{
		public const string MODSTATUSURL = "https://xivrus.ru/status.json";
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		public static ModStatusJson GetModStatus(string statusurl = MODSTATUSURL)
		{
			try
			{
				Logger.Info(String.Format("Load mod status from {0}", statusurl));
				HttpClient hc = new HttpClient();
				hc.DefaultRequestHeaders.Add("User-Agent", "XIVRUS");
				string json = hc.GetStringAsync(statusurl).Result;
				System.Diagnostics.Trace.WriteLine(json);
				ModStatusJson modStatusJson = JsonConvert.DeserializeObject<ModStatusJson>(json);
				Logger.Info(String.Format("Mod Status Code {0}", modStatusJson.Status));
				return modStatusJson;
			}
			catch (Exception ex)
			{
				Logger.Error(String.Format("Message {0}\nStack Trace:\n {1}", ex.Message, ex.StackTrace));
				return null;
			}
		}
		public static ModStatusJson GetModStatusFromFile(string statusfile)
		{
			try
			{
				Logger.Info(String.Format("Load mod status from file {0}", statusfile));
				string json = File.ReadAllText(statusfile);
				System.Diagnostics.Trace.WriteLine(json);
				ModStatusJson modStatusJson = JsonConvert.DeserializeObject<ModStatusJson>(json);
				Logger.Info(String.Format("(DEBUG FILE) Mod Status Code {0}", modStatusJson.Status));
				return modStatusJson;
			}
			catch (Exception ex)
			{
				Logger.Error(String.Format("Message {0}\nStack Trace:\n {1}", ex.Message, ex.StackTrace));
				return null;
			}
		}
	}
}
