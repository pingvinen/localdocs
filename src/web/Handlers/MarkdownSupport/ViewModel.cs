using System;
using System.Collections.Generic;

namespace LocalDocs.Web.Handlers.MarkdownSupport
{
	public class ViewModel
	{
		public ViewModel()
		{
			this.AvailableSites = new List<SiteSwitchEntry>();
		}

		public TargetSite Target { get; set; }
		public string MarkdownHtml { get; set; }
		public IList<SiteSwitchEntry> AvailableSites { get; set; }
		public Sitemap Sitemap { get; set; }
	}
}

