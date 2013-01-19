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
		protected override string InternalHandler(HttpContext context, TargetSite target, string targetRootDir, string requestedPath, ViewModel vm)
		{
			string mdFilePath = String.Format("{0}.md", Path.Combine(targetRootDir, requestedPath));

			vm.MarkdownHtml = MarkdownHelper.ProcessMarkdown(MarkdownHelper.GetMarkdown(mdFilePath));

			SparkRenderer renderer = new SparkRenderer();
			string output = renderer.Render(target.TemplateFile, vm);

			return output;
		}
		#endregion
	}
}