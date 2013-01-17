using System;

namespace LocalDocs.Web
{
	public class PageContext
	{
		public PageContext()
		{
		}

		public TargetSite Site { get; set; }
		public string WebRoot { get; set; }
	}
}