using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XIVRUS_Updater.XIVConfigs
{
	public class XIVRUSMod
	{
		public const string MODFOLDERNAME = "XIV Rus";

		public static bool ModExist(string PenumbraFolder)
		{
			string path = GetModPath(PenumbraFolder);

			return Directory.Exists(path) && File.Exists(String.Format("{0}/meta.json", path));
		}

		public static string GetModPath(string PenumbraFolder)
		{
			return String.Format("{0}/{1}", PenumbraFolder, MODFOLDERNAME);
		}

	}
}
