using System;
using System.Web;
using System.Configuration;
using System.IO;
using MarkdownSharp;
using System.Text;

namespace LocalDocs.Web
{
	/// <summary>
	/// Handles all requests by looking for, and outputting, a Markdown
	/// file that matches the location in the url
	/// </summary>
	public class MarkdownHttpHandler : IHttpHandler
	{
		private TargetSitesElement targetSite;

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

			string rootDir = this.GetMarkdownRootDir();
			string requestedPath = req.Path;

			// the default file is "index.md"
			if (requestedPath.Equals("/"))
			{
				requestedPath = "/index";
			}

			// remove the initial slash, so we can use Path.Combine
			requestedPath = requestedPath.Remove(0, 1);

			string mdFilePath = String.Format("{0}.md", Path.Combine(rootDir, requestedPath));
			string templateFilePath = Path.Combine(rootDir, "master.html");

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

			HttpResponse resp = context.Response;

			string output = File.ReadAllText(templateFilePath);

			output = output.Replace("${target.name}", this.targetSite.Name);
			output = output.Replace("${content}", this.ProcessMarkdown(this.GetMarkdown(mdFilePath)));
			output = output.Replace("${debug}", sbDebug.ToString());

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

			string webroot = HttpContext.Current.Server.MapPath("/");

			string absolutePath = Path.Combine(webroot, fromConf);

			return absolutePath;
		}
		#endregion Helper: markdown root dir
	}
}