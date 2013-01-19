using System;
using System.IO;
using System.Collections.Generic;
using LocalDocs.Web.Handlers.MarkdownSupport;
using ServiceStack.Text;

namespace LocalDocs.Web
{
	public static class Helper
	{
		#region Get target root dit
		public static string GetTargetRootDir(string siteRoot, string webRoot)
		{
			if (Path.IsPathRooted(siteRoot))
			{
				return siteRoot;
			}

			string absolutePath = Path.Combine(webRoot, siteRoot);

			return absolutePath;
		}
		#endregion Get target root dit

		#region Set templatefile of target
		public static void SetTemplateFileOfTarget(TargetSite target, string rootDir, string webRoot)
		{
			string tmp = Path.Combine(rootDir, Constants.LayoutFolderName);

			target.HasCustomLayout = Directory.Exists(tmp);

			if (target.HasCustomLayout)
			{
				target.TemplateFile = Path.Combine(tmp, Constants.MasterFileName);
			}
			else
			{
				target.TemplateFile = Path.Combine(webRoot, Constants.LayoutFolderName, Constants.MasterFileName);
			}
		}
		#endregion Set templatefile of target

		#region Get available targets
		public static IList<SiteSwitchEntry> GetAvailableTargets(string currentSiteId)
		{
			List<SiteSwitchEntry> res = new List<SiteSwitchEntry>();

			IList<TargetSite> sites = TargetSitesConfig.GetAllSites();

			foreach (TargetSite cur in sites)
			{
				res.Add(new SiteSwitchEntry() {
					Name = cur.Name,
					Url = String.Format("/switchsite?to={0}", cur.Id),
					IsActive = cur.Id.Equals(currentSiteId)
				});
			}

			return res;
		}
		#endregion Get available targets

		#region Load config
		public static void LoadConfig(TargetSite target, string webroot)
		{
			string configFile = Path.Combine(Helper.GetTargetRootDir(target.Root, webroot), Constants.ConfigFileName);

			if (!File.Exists(configFile))
			{
				target.HasLoadedConfig = true;
				target.Config = new TargetConfig();
				return;
			}

			target.Config = File.ReadAllText(configFile).FromJson<TargetConfig>();
		}
		#endregion Load config
	}
}