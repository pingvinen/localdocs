using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LocalDocs.Web.Config;

namespace LocalDocs.Web
{
	/// <summary>
	/// Helper class that gives easy access to the "TargetSites" config values
	/// </summary>
	public class TargetSitesConfig
	{
		/// <summary>
		/// Contains all the target sites defined in config
		/// </summary>
		protected static Dictionary<string, TargetSite> mappedInstances;

		/// <summary>
		/// The ID of the default target site
		/// </summary>
		protected static string defaultTargetId = String.Empty;


		static TargetSitesConfig()
		{
			mappedInstances = new Dictionary<string,TargetSite>();

			TargetSitesConfigSection sec = (TargetSitesConfigSection)ConfigurationManager.GetSection("TargetSites");

			foreach (TargetSitesElement cur in sec.Instances)
			{
				mappedInstances.Add(cur.Id, new TargetSite() {
					Id = cur.Id,
					Name = cur.Name,
					Root = cur.Root,
					TemplateFile = String.Empty
				});

				if (cur.IsDefault)
				{
					defaultTargetId = cur.Id;
				}
			}
		}

		/// <summary>
		/// Get the target site with the given ID.
		/// </summary>
		/// <param name="targetId">
		/// The ID of the target site to get
		/// </param>
		public static TargetSite Get(string targetId)
		{
			TargetSite elm;

			if (!mappedInstances.TryGetValue(targetId, out elm))
			{
				throw new ArgumentException(String.Format("A target site with id '{0}' has not been defined in <TargetSites>", targetId));
			}

			return elm;
		}

		public static TargetSite GetDefaultSite()
		{
			if (String.IsNullOrEmpty(defaultTargetId))
			{
				defaultTargetId = mappedInstances.First().Value.Id;
			}

			return Get(defaultTargetId);
		}

		public static IList<TargetSite> GetAllSites()
		{
			return mappedInstances.Select<KeyValuePair<string, TargetSite>, TargetSite>(yy => yy.Value).ToList();
		}

		private TargetSitesConfig()
		{
		}
	}
}