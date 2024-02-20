using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace XIVRUS_Updater.XIVConfigs.XIVLauncher
{
	public partial class LauncherConfigV3
	{
		[JsonProperty("AcceptLanguage")]
		public string AcceptLanguage { get; set; }

		[JsonProperty("LauncherLanguage")]
		public string LauncherLanguage { get; set; }

		[JsonProperty("AddonList")]
		public string AddonList { get; set; }

		[JsonProperty("PatchAcquisitionMethod")]
		public string PatchAcquisitionMethod { get; set; }

		[JsonProperty("InGameAddonLoadMethod")]
		public string InGameAddonLoadMethod { get; set; }

		[JsonProperty("EncryptArguments")]
		public bool EncryptArguments { get; set; }

		[JsonProperty("AskBeforePatchInstall")]
		public bool AskBeforePatchInstall { get; set; }

		[JsonProperty("DpiAwareness")]
		public string DpiAwareness { get; set; }

		[JsonProperty("TreatNonZeroExitCodeAsFailure")]
		public bool TreatNonZeroExitCodeAsFailure { get; set; }

		[JsonProperty("ExitLauncherAfterGameExit")]
		public bool ExitLauncherAfterGameExit { get; set; }

		[JsonProperty("IsFt")]
		public bool IsFt { get; set; }

		[JsonProperty("AutoStartSteam")]
		public bool AutoStartSteam { get; set; }

		[JsonProperty("ForceNorthAmerica")]
		public bool ForceNorthAmerica { get; set; }

		[JsonProperty("VersionUpgradeLevel")]
		public long VersionUpgradeLevel { get; set; }

		[JsonProperty("GamePath")]
		public string GamePath { get; set; }

		[JsonProperty("IsDx11")]
		public bool IsDx11 { get; set; }

		[JsonProperty("Language")]
		public string Language { get; set; }

		[JsonProperty("InGameAddonEnabled")]
		public bool InGameAddonEnabled { get; set; }

		[JsonProperty("DalamudRolloutBucket")]
		public string DalamudRolloutBucket { get; set; }

		[JsonProperty("KeepPatches")]
		public bool KeepPatches { get; set; }

		[JsonProperty("DalamudInjectionDelayMs")]
		public long DalamudInjectionDelayMs { get; set; }

		[JsonProperty("OtpServerEnabled")]
		public bool OtpServerEnabled { get; set; }

		[JsonProperty("AdditionalLaunchArgs")]
		public string AdditionalLaunchArgs { get; set; }

		[JsonProperty("SpeedLimitBytes")]
		public long SpeedLimitBytes { get; set; }

		[JsonProperty("HasShownAutoLaunchDisclaimer")]
		public bool HasShownAutoLaunchDisclaimer { get; set; }

		[JsonProperty("HasComplainedAboutAdmin")]
		public bool HasComplainedAboutAdmin { get; set; }

		[JsonProperty("CurrentAccountId")]
		public string CurrentAccountId { get; set; }

		[JsonProperty("AutologinEnabled")]
		public bool AutologinEnabled { get; set; }

		[JsonProperty("MainWindowPlacement")]
		public string MainWindowPlacement { get; set; }

		[JsonProperty("PatchPath")]
		public string PatchPath { get; set; }

		[JsonIgnore]
		public List<LauncherConfigV3Addon> AddonListData { get; set; }
	}

	public partial class LauncherConfigV3Addon
	{
		[JsonProperty("IsEnabled")]
		public bool IsEnabled { get; set; }

		[JsonProperty("Addon")]
		public AddonData Addon { get; set; }
	}

	public partial class AddonData
	{
		[JsonProperty("Path")]
		public string Path { get; set; }

		[JsonProperty("CommandLine")]
		public string CommandLine { get; set; }

		[JsonProperty("RunAsAdmin")]
		public bool RunAsAdmin { get; set; }

		[JsonProperty("RunOnClose")]
		public bool RunOnClose { get; set; }

		[JsonProperty("KillAfterClose")]
		public bool KillAfterClose { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }
	}
}
