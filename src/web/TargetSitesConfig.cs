using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

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
		protected static Dictionary<string, TargetSitesElement> instances;

		/// <summary>
		/// The ID of the default target site
		/// </summary>
		protected static string defaultTargetId = String.Empty;


		static TargetSitesConfig()
		{
			instances = new Dictionary<string,TargetSitesElement>();

			TargetSitesConfigSection sec = (TargetSitesConfigSection)ConfigurationManager.GetSection("TargetSites");

			foreach (TargetSitesElement cur in sec.Instances)
			{
				instances.Add(cur.Id, cur);

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
		public static TargetSitesElement Get(string targetId)
		{
			TargetSitesElement elm;

			if (!instances.TryGetValue(targetId, out elm))
			{
				throw new ArgumentException(String.Format("A target site with id '{0}' has not been defined in <TargetSites>", targetId));
			}

			return elm;
		}

		public static TargetSitesElement GetDefaultSite()
		{
			if (String.IsNullOrEmpty(defaultTargetId))
			{
				defaultTargetId = instances.First().Value.Id;
			}

			return Get(defaultTargetId);
		}

		private TargetSitesConfig()
		{
		}
	}
}