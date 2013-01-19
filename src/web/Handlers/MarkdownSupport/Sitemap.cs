using System;
using System.Collections.Generic;

namespace LocalDocs.Web.Handlers.MarkdownSupport
{
	public class Sitemap
	{
		public Sitemap()
		{
			this.Entries = new List<SitemapEntry>();
		}

		public IList<SitemapEntry> Entries { get; set; }
	}
}