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

			string rootDir = Helper.GetMarkdownRootDir(target.Root, ses.WebRoot);
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
				string tmp = Path.Combine(rootDir, Constants.LayoutFolderName);

				target.HasCustomLayout = Directory.Exists(tmp);

				if (target.HasCustomLayout)
				{
					target.TemplateFile = Path.Combine(tmp, Constants.MasterFileName);
				}
				else
				{
					target.TemplateFile = Path.Combine(ses.WebRoot, Constants.LayoutFolderName, Constants.MasterFileName);
				}
			}

			string templateFilePath = target.TemplateFile;
			#endregion Template

			#region Debug
			StringBuilder sbDebug = new StringBuilder();
			sbDebug.AppendFormat("<p>Current target site name: '{0}'</p>", target.Name);
			sbDebug.AppendFormat("<p>Markdown root directoy: '{0}'</p>", rootDir);
			sbDebug.AppendFormat("<p>Requested path: '{0}'</p>", requestedPath);
			sbDebug.AppendFormat("<p>Markdown file path: '{0}'</p>", mdFilePath);
			#endregion Debug

			if (!File.Exists(templateFilePath))
			{
				throw new InvalidOperationException(String.Format("Template file for '{0}' is missing. It should be at '{1}'", target.Name, templateFilePath));
			}

			SparkRenderer renderer = new SparkRenderer();
			string output = renderer.Render(templateFilePath, new ViewModel {
				Target = target,
				MarkdownHtml = this.ProcessMarkdown(this.GetMarkdown(mdFilePath)),
				AvailableSites = this.MakeTargetSiteSelect(target.Id)
			});


			/*

			string output = File.ReadAllText(templateFilePath);

			output = output.Replace("${target.name}", target.Name);
			output = output.Replace("${content}", this.ProcessMarkdown(this.GetMarkdown(mdFilePath)));
			output = output.Replace("${debug}", sbDebug.ToString());
			output = output.Replace("${siteselect}", this.MakeTargetSiteSelect(target.Id));
			*/

			resp.Write(output);
		}
		#endregion IHandler implementation

		#region Get markdown
		private string GetMarkdown(string path)
		{
			if (File.Exists(path))
			{
				return File.ReadAllText(path);
			}

			return String.Format("File not found: '{0}'", path);
		}
		#endregion Get markdown

		#region Process markdown
		private string ProcessMarkdown(string markdown)
		{
			Markdown md = new Markdown();

			return md.Transform(markdown);
		}
		#endregion Process markdown

		#region Make site select
		private IList<SiteSwitchEntry> MakeTargetSiteSelect(string currentSiteId)
		{
			List<SiteSwitchEntry> res = new List<SiteSwitchEntry>();

			IList<TargetSite> sites = TargetSitesConfig.GetAllSites();

			foreach (TargetSite cur in sites)
			{
				res.Add(new SiteSwitchEntry() {
					Name = cur.Name,
					Url = String.Format("/switchsite?to={0}", cur.Id),
					IsActive = cur.Id.Equals(currentSiteId)
				});
			}

			return res;
		}
		#endregion Make site select
	}
}