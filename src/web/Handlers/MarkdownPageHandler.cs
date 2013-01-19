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
	public class MarkdownPageHandler : IHandler
	{
		public MarkdownPageHandler()
		{
		}

		#region IHandler implementation
		public void HandleRequest(HttpContext context)
		{
			HttpRequest req = context.Request;
			HttpResponse resp = context.Response;
			Session ses = Session.GetInstance();
			TargetSite target = TargetSitesConfig.Get(ses.TargetSiteId);

			string rootDir = Helper.GetTargetRootDir(target.Root, ses.WebRoot);
			string requestedPath = req.Path;

			// the default file is "index.md"
			if (requestedPath.Equals("/"))
			{
				requestedPath = "/index";
			}

			// remove the initial slash, so we can use Path.Combine
			requestedPath = requestedPath.Remove(0, 1);

			string mdFilePath = String.Format("{0}.md", Path.Combine(rootDir, requestedPath));

			#region Template
			if (String.IsNullOrEmpty(target.TemplateFile))
			{
				Helper.SetTemplateFileOfTarget(target, rootDir, ses.WebRoot);
			}

			string templateFilePath = target.TemplateFile;
			#endregion Template

			if (!File.Exists(templateFilePath))
			{
				throw new InvalidOperationException(String.Format("Template file for '{0}' is missing. It should be at '{1}'", target.Name, templateFilePath));
			}

			SparkRenderer renderer = new SparkRenderer();
			string output = renderer.Render(templateFilePath, new ViewModel {
				Target = target,
				MarkdownHtml = MarkdownHelper.ProcessMarkdown(MarkdownHelper.GetMarkdown(mdFilePath)),
				AvailableSites = Helper.GetAvailableTargets(target.Id)
			});

			resp.Write(output);
		}
		#endregion IHandler implementation
	}
}