using System;

namespace LocalDocs.Web.Handlers.MarkdownSupport
{
	public class ViewModel
	{
		public ViewModel()
		{
		}

		public TargetSite Target { get; set; }
		public string MarkdownHtml { get; set; }
	}
}

