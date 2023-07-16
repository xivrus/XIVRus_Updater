using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;

namespace XIVRUS_Updater.GitHub
{
	#region JSON
	//https://api.github.com/repos/xivrus/xiv_ru_weblate/releases/latest
	public partial class ReleaseJson
	{
		[JsonProperty("url")]
		public Uri Url { get; set; }

		[JsonProperty("assets_url")]
		public Uri AssetsUrl { get; set; }

		[JsonProperty("upload_url")]
		public string UploadUrl { get; set; }

		[JsonProperty("html_url")]
		public Uri HtmlUrl { get; set; }

		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("author")]
		public Author Author { get; set; }

		[JsonProperty("node_id")]
		public string NodeId { get; set; }

		[JsonProperty("tag_name")]
		public string TagName { get; set; }

		[JsonProperty("target_commitish")]
		public string TargetCommitish { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("draft")]
		public bool Draft { get; set; }

		[JsonProperty("prerelease")]
		public bool Prerelease { get; set; }

		[JsonProperty("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		[JsonProperty("published_at")]
		public DateTimeOffset PublishedAt { get; set; }

		[JsonProperty("assets")]
		public Asset[] Assets { get; set; }

		[JsonProperty("tarball_url")]
		public Uri TarballUrl { get; set; }

		[JsonProperty("zipball_url")]
		public Uri ZipballUrl { get; set; }

		[JsonProperty("body")]
		public string Body { get; set; }
	}

	public partial class Asset
	{
		[JsonProperty("url")]
		public Uri Url { get; set; }

		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("node_id")]
		public string NodeId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("label")]
		public object Label { get; set; }

		[JsonProperty("uploader")]
		public Author Uploader { get; set; }

		[JsonProperty("content_type")]
		public string ContentType { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("size")]
		public long Size { get; set; }

		[JsonProperty("download_count")]
		public long DownloadCount { get; set; }

		[JsonProperty("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTimeOffset UpdatedAt { get; set; }

		[JsonProperty("browser_download_url")]
		public Uri BrowserDownloadUrl { get; set; }
	}

	public partial class Author
	{
		[JsonProperty("login")]
		public string Login { get; set; }

		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("node_id")]
		public string NodeId { get; set; }

		[JsonProperty("avatar_url")]
		public Uri AvatarUrl { get; set; }

		[JsonProperty("gravatar_id")]
		public string GravatarId { get; set; }

		[JsonProperty("url")]
		public Uri Url { get; set; }

		[JsonProperty("html_url")]
		public Uri HtmlUrl { get; set; }

		[JsonProperty("followers_url")]
		public Uri FollowersUrl { get; set; }

		[JsonProperty("following_url")]
		public string FollowingUrl { get; set; }

		[JsonProperty("gists_url")]
		public string GistsUrl { get; set; }

		[JsonProperty("starred_url")]
		public string StarredUrl { get; set; }

		[JsonProperty("subscriptions_url")]
		public Uri SubscriptionsUrl { get; set; }

		[JsonProperty("organizations_url")]
		public Uri OrganizationsUrl { get; set; }

		[JsonProperty("repos_url")]
		public Uri ReposUrl { get; set; }

		[JsonProperty("events_url")]
		public string EventsUrl { get; set; }

		[JsonProperty("received_events_url")]
		public Uri ReceivedEventsUrl { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("site_admin")]
		public bool SiteAdmin { get; set; }
	}
	#endregion JSON

	public class Releases
	{
		public const string GITHUBLASTRELEASEURL = "https://api.github.com/repos/xivrus/xiv_ru_weblate/releases/latest";
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		public static ReleaseJson GetLastRelease()
		{
			try
			{
				HttpClient hc = new HttpClient();
				hc.DefaultRequestHeaders.Add("User-Agent", "XIVRUS");
				string json = hc.GetStringAsync(GITHUBLASTRELEASEURL).Result;
				System.Diagnostics.Trace.WriteLine(json);
				ReleaseJson rj = JsonConvert.DeserializeObject<ReleaseJson>(json);
				return rj;
			}
			catch (Exception ex)
			{
				Logger.Error(String.Format("Message {0}\nStack Trace:\n {1}", ex.Message, ex.StackTrace));
				return null;
			}

		}

		public static string GetAssetFileUrlByName(ReleaseJson releasejson, string filename)
		{
			string url = null;
			foreach (Asset asset in releasejson.Assets)
			{
				if (asset.Name.ToLower() == filename.ToLower())
				{
					url = asset.BrowserDownloadUrl.ToString();
					break;
				}
			}
			return url;
		}
	}
}

