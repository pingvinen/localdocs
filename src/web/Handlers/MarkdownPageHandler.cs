using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.IO;
using MarkdownSharp;
using LocalDocs.Web.SparkStuff;
using LocalDocs.Web.Handlers.MarkdownSupport;

namespace LocalDocs.Web.Handlers
{
	/// <summary>
	/// Handles requests for markdown pages
	/// </summary>
	public class MarkdownPageHandler : PageHandlerBase
	{
		public MarkdownPageHandler()
		{
		}

		#region implemented abstract members of LocalDocs.Web.Handlers.PageHandlerBase
		protected override ViewModel InternalHandler(HttpContext context, TargetSite target, string targetRootDir, string requestedPath)
		{
			ViewModel vm = new ViewModel();

			string mdFilePath = String.Format("{0}.md", Path.Combine(targetRootDir, requestedPath));

			vm.MarkdownHtml = MarkdownHelper.ProcessMarkdown(MarkdownHelper.GetMarkdown(mdFilePath));

			return vm;
		}
		#endregion
	}
}