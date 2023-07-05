using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XIVRUS_Updater.GitHub;

namespace XIVRUS_Updater.XIVConfigs
{
	public partial class PenumbraModMetaJson
	{
		[JsonProperty("FileVersion")]
		public long FileVersion { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Author")]
		public string Author { get; set; }

		[JsonProperty("Description")]
		public string Description { get; set; }

		[JsonProperty("Version")]
		public string Version { get; set; }

		[JsonProperty("Website")]
		public Uri Website { get; set; }

	}

	public class PenumbraModMeta
	{
		public static PenumbraModMetaJson GetMetaByDirectory(string ModDirectory)
		{
			try
			{
				string path = String.Format("{0}/meta.json", ModDirectory);
				if (!File.Exists(path))
				{
					return null;
				}
				string json = File.ReadAllText(path);
				return JsonConvert.DeserializeObject<PenumbraModMetaJson>(json);
			}
			catch
			{
				return null;
			}
		}
	}
}
