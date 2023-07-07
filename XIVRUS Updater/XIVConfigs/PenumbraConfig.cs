using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XIVRUS_Updater.XIVConfigs
{
	public partial class PenumbraConfigJson
	{
		[JsonProperty("DebugSeparateWindow")]
		public bool DebugSeparateWindow { get; set; }

		[JsonProperty("Version")]
		public long Version { get; set; }

		[JsonProperty("LastSeenVersion")]
		public long LastSeenVersion { get; set; }

		[JsonProperty("ChangeLogDisplayType")]
		public long ChangeLogDisplayType { get; set; }

		[JsonProperty("EnableMods")]
		public bool EnableMods { get; set; }

		[JsonProperty("ModDirectory")]
		public string ModDirectory { get; set; }

		[JsonProperty("ExportDirectory")]
		public string ExportDirectory { get; set; }

		[JsonProperty("HideUiInGPose")]
		public bool HideUiInGPose { get; set; }

		[JsonProperty("HideUiInCutscenes")]
		public bool HideUiInCutscenes { get; set; }

		[JsonProperty("HideUiWhenUiHidden")]
		public bool HideUiWhenUiHidden { get; set; }

		[JsonProperty("UseCharacterCollectionInMainWindow")]
		public bool UseCharacterCollectionInMainWindow { get; set; }

		[JsonProperty("UseCharacterCollectionsInCards")]
		public bool UseCharacterCollectionsInCards { get; set; }

		[JsonProperty("UseCharacterCollectionInInspect")]
		public bool UseCharacterCollectionInInspect { get; set; }

		[JsonProperty("UseCharacterCollectionInTryOn")]
		public bool UseCharacterCollectionInTryOn { get; set; }

		[JsonProperty("UseOwnerNameForCharacterCollection")]
		public bool UseOwnerNameForCharacterCollection { get; set; }

		[JsonProperty("UseNoModsInInspect")]
		public bool UseNoModsInInspect { get; set; }

		[JsonProperty("HideRedrawBar")]
		public bool HideRedrawBar { get; set; }

		[JsonProperty("OptionGroupCollapsibleMin")]
		public long OptionGroupCollapsibleMin { get; set; }

		[JsonProperty("DebugMode")]
		public bool DebugMode { get; set; }

		[JsonProperty("TutorialStep")]
		public long TutorialStep { get; set; }

		[JsonProperty("EnableResourceLogging")]
		public bool EnableResourceLogging { get; set; }

		[JsonProperty("ResourceLoggingFilter")]
		public string ResourceLoggingFilter { get; set; }

		[JsonProperty("EnableResourceWatcher")]
		public bool EnableResourceWatcher { get; set; }

		[JsonProperty("OnlyAddMatchingResources")]
		public bool OnlyAddMatchingResources { get; set; }

		[JsonProperty("MaxResourceWatcherRecords")]
		public long MaxResourceWatcherRecords { get; set; }

		[JsonProperty("ResourceWatcherResourceTypes")]
		public long ResourceWatcherResourceTypes { get; set; }

		[JsonProperty("ResourceWatcherResourceCategories")]
		public long ResourceWatcherResourceCategories { get; set; }

		[JsonProperty("ResourceWatcherRecordTypes")]
		public long ResourceWatcherRecordTypes { get; set; }

		[JsonProperty("ScaleModSelector")]
		public bool ScaleModSelector { get; set; }

		[JsonProperty("ModSelectorAbsoluteSize")]
		public long ModSelectorAbsoluteSize { get; set; }

		[JsonProperty("ModSelectorScaledSize")]
		public long ModSelectorScaledSize { get; set; }

		[JsonProperty("OpenFoldersByDefault")]
		public bool OpenFoldersByDefault { get; set; }

		[JsonProperty("SingleGroupRadioMax")]
		public long SingleGroupRadioMax { get; set; }

		[JsonProperty("DefaultImportFolder")]
		public string DefaultImportFolder { get; set; }

		[JsonProperty("QuickMoveFolder1")]
		public string QuickMoveFolder1 { get; set; }

		[JsonProperty("QuickMoveFolder2")]
		public string QuickMoveFolder2 { get; set; }

		[JsonProperty("QuickMoveFolder3")]
		public string QuickMoveFolder3 { get; set; }


		[JsonProperty("CollectionPanel")]
		public long CollectionPanel { get; set; }

		[JsonProperty("SelectedTab")]
		public long SelectedTab { get; set; }

		[JsonProperty("PrintSuccessfulCommandsToChat")]
		public bool PrintSuccessfulCommandsToChat { get; set; }

		[JsonProperty("FixMainWindow")]
		public bool FixMainWindow { get; set; }

		[JsonProperty("AutoDeduplicateOnImport")]
		public bool AutoDeduplicateOnImport { get; set; }

		[JsonProperty("EnableHttpApi")]
		public bool EnableHttpApi { get; set; }

		[JsonProperty("DefaultModImportPath")]
		public string DefaultModImportPath { get; set; }

		[JsonProperty("AlwaysOpenDefaultImport")]
		public bool AlwaysOpenDefaultImport { get; set; }

		[JsonProperty("KeepDefaultMetaChanges")]
		public bool KeepDefaultMetaChanges { get; set; }

		[JsonProperty("DefaultModAuthor")]
		public string DefaultModAuthor { get; set; }

		[JsonProperty("Colors")]
		public Dictionary<string, long> Colors { get; set; }

		[JsonProperty("SortMode")]
		public string SortMode { get; set; }
	}

	public static class PenumbraConfig
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		public static PenumbraConfigJson LoadConfig()
		{
			string configPath = GetConfigPath();
			if (!File.Exists(configPath))
			{
				return null;
			}
			try
			{
				string json = File.ReadAllText(configPath);
				return JsonConvert.DeserializeObject<PenumbraConfigJson>(json);
			} 
			catch(Exception ex)
			{
				Logger.Error(String.Format("Message {0}\nStack Trace:\n {1}", ex.Message, ex.StackTrace));
				return null;
			}
			
		}

		public static string GetConfigPath()
		{
			return String.Format("{0}/pluginConfigs/Penumbra.json", WinDirs.GetXIVLauncherConfigFolder());
		}
	}
}
