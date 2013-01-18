using System;

namespace LocalDocs.Web.Handlers.MarkdownSupport
{
	public class SiteSwitchEntry
	{
		public SiteSwitchEntry()
		{
		}

		public string Url { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
	}
}