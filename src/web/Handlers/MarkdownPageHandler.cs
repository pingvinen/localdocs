using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.IO;
using MarkdownSharp;

namespace LocalDocs.Web.Handlers
{
	public class MarkdownPageHandler : IHandler
	{
		public MarkdownPageHandler()
		{
		}

		public void HandleRequest(HttpContext context, PageContext pageContext)
		{
			HttpRequest req = context.Request;
			HttpResponse resp = context.Response;

			string rootDir = this.GetMarkdownRootDir(pageContext);
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
			if (String.IsNullOrEmpty(pageContext.Site.TemplateFile))
			{
				string tmp = Path.Combine(rootDir, Constants.LayoutFolderName);

				pageContext.Site.HasCustomLayout = Directory.Exists(tmp);

				if (pageContext.Site.HasCustomLayout)
				{
					pageContext.Site.TemplateFile = Path.Combine(tmp, Constants.MasterFileName);
				}
				else
				{
					pageContext.Site.TemplateFile = Path.Combine(pageContext.WebRoot, Constants.LayoutFolderName, Constants.MasterFileName);
				}
			}

			string templateFilePath = pageContext.Site.TemplateFile;
			#endregion Template

			#region Debug
			StringBuilder sbDebug = new StringBuilder();
			sbDebug.AppendFormat("<p>Current target site name: '{0}'</p>", pageContext.Site.Name);
			sbDebug.AppendFormat("<p>Markdown root directoy: '{0}'</p>", rootDir);
			sbDebug.AppendFormat("<p>Requested path: '{0}'</p>", requestedPath);
			sbDebug.AppendFormat("<p>Markdown file path: '{0}'</p>", mdFilePath);
			#endregion Debug

			if (!File.Exists(templateFilePath))
			{
				throw new InvalidOperationException(String.Format("Template file for '{0}' is missing. It should be at '{1}'", pageContext.Site.Name, templateFilePath));
			}

			string output = File.ReadAllText(templateFilePath);

			output = output.Replace("${target.name}", pageContext.Site.Name);
			output = output.Replace("${content}", this.ProcessMarkdown(this.GetMarkdown(mdFilePath)));
			output = output.Replace("${debug}", sbDebug.ToString());
			output = output.Replace("${siteselect}", this.MakeTargetSiteSelect(pageContext.Site.Id));

			resp.Write(output);
		}

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

		#region Helper: markdown root dir
		private string GetMarkdownRootDir(PageContext pageContext)
		{
			string fromConf = pageContext.Site.Root;

			if (Path.IsPathRooted(fromConf))
			{
				return fromConf;
			}

			string absolutePath = Path.Combine(pageContext.WebRoot, fromConf);

			return absolutePath;
		}
		#endregion Helper: markdown root dir

		#region Make site select
		private string MakeTargetSiteSelect(string currentSiteId)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("<select onchange=\"window.location='switchsite?to='+this.value;\">");

			IList<TargetSite> sites = TargetSitesConfig.GetAllSites();

			string selected;
			foreach (TargetSite cur in sites)
			{
				selected = String.Empty;

				if (cur.Id.Equals(currentSiteId))
				{
					selected = " selected=\"selected\"";
				}

				sb.AppendFormat("<option value=\"{0}\"{2}>{1}</option>", cur.Id, cur.Name, selected);
			}

			sb.Append("</select>");

			return sb.ToString();
		}
		#endregion Make site select
	}
}