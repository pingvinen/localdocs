using System;
using System.Web;
using System.IO;
using MarkdownSharp;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace LocalDocs.Web
{
	/// <summary>
	/// Handles all requests by looking for, and outputting, a Markdown
	/// file that matches the location in the url
	/// </summary>
	public class MarkdownHttpHandler : IHttpHandler
	{
		private TargetSite targetSite;

		private string webRoot;

		private readonly string LayoutFolderName = "__layout";

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalDocs.Web.MarkdownHttpHandler"/> class.
		/// </summary>
		public MarkdownHttpHandler()
		{
			this.targetSite = TargetSitesConfig.GetDefaultSite();
		}

		/// <summary>
		/// Get whether this handler instance can be used by
		/// multiple requests
		/// </summary>
		public bool IsReusable { get { return true; } }

		#region Process request
		public void ProcessRequest(HttpContext context)
		{
			HttpRequest req = context.Request;
			HttpResponse resp = context.Response;

			#region Web root
			if (String.IsNullOrEmpty(this.webRoot))
			{
				this.webRoot = context.Server.MapPath("/");
			}
			#endregion Web root

			string rootDir = this.GetMarkdownRootDir();
			string requestedPath = req.Path;

			// the default file is "index.md"
			if (requestedPath.Equals("/"))
			{
				requestedPath = "/index";
			}

			#region Switch site
			if (requestedPath.Equals("/switchsite"))
			{
				NameValueCollection nvc = req.QueryString;
				if (String.IsNullOrEmpty(nvc["to"]))
				{
					resp.Redirect(req.UrlReferrer.AbsoluteUri, true);
				}

				try
				{
					this.targetSite = TargetSitesConfig.Get(nvc["to"]);
					resp.Redirect("/", true);
				}

				catch (Exception)
				{
					resp.Redirect(req.UrlReferrer.AbsoluteUri, true);
				}
			}
			#endregion Switch site

			// remove the initial slash, so we can use Path.Combine
			requestedPath = requestedPath.Remove(0, 1);

			string mdFilePath = String.Format("{0}.md", Path.Combine(rootDir, requestedPath));

			#region Template
			if (String.IsNullOrEmpty(this.targetSite.TemplateFile))
			{
				string tmp = Path.Combine(rootDir, this.LayoutFolderName, "master.html");
				if (File.Exists(tmp))
				{
					this.targetSite.TemplateFile = tmp;
				}
				else
				{
					this.targetSite.TemplateFile = Path.Combine(this.webRoot, this.LayoutFolderName, "master.html");
				}
			}

			string templateFilePath = this.targetSite.TemplateFile;
			#endregion Template

			#region Debug
			StringBuilder sbDebug = new StringBuilder();
			sbDebug.AppendFormat("<p>Current target site name: '{0}'</p>", this.targetSite.Name);
			sbDebug.AppendFormat("<p>Markdown root directoy: '{0}'</p>", rootDir);
			sbDebug.AppendFormat("<p>Requested path: '{0}'</p>", requestedPath);
			sbDebug.AppendFormat("<p>Markdown file path: '{0}'</p>", mdFilePath);
			#endregion Debug

			if (!File.Exists(templateFilePath))
			{
				throw new InvalidOperationException(String.Format("Template file for '{0}' is missing. It should be at '{1}'", this.targetSite.Name, templateFilePath));
			}

			string output = File.ReadAllText(templateFilePath);

			output = output.Replace("${target.name}", this.targetSite.Name);
			output = output.Replace("${content}", this.ProcessMarkdown(this.GetMarkdown(mdFilePath)));
			output = output.Replace("${debug}", sbDebug.ToString());
			output = output.Replace("${siteselect}", this.MakeTargetSiteSelect());

			resp.Write(output);
		}
		#endregion Process request

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
		private string GetMarkdownRootDir()
		{
			string fromConf = this.targetSite.Root;

			if (Path.IsPathRooted(fromConf))
			{
				return fromConf;
			}

			string absolutePath = Path.Combine(this.webRoot, fromConf);

			return absolutePath;
		}
		#endregion Helper: markdown root dir

		#region Make site select
		private string MakeTargetSiteSelect()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("<select onchange=\"window.location='switchsite?to='+this.value;\">");

			IList<TargetSite> sites = TargetSitesConfig.GetAllSites();

			string selected;
			foreach (TargetSite cur in sites)
			{
				selected = String.Empty;

				if (cur.Id.Equals(this.targetSite.Id))
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