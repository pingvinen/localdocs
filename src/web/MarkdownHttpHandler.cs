using System;
using System.Web;
using System.Configuration;
using System.IO;
using MarkdownSharp;

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



			HttpResponse resp = context.Response;
			resp.Write("<html>");
			resp.Write("<body>");
			resp.Write("<h1>Good neeeews everyone</h1>");
			resp.Write(String.Format("<p>Current target site name: '{0}'</p>", this.targetSite.Name));
			resp.Write(String.Format("<p>Markdown root directoy: '{0}'</p>", rootDir));
			resp.Write(String.Format("<p>Requested path: '{0}'</p>", requestedPath));
			resp.Write(String.Format("<p>Markdown file path: '{0}'</p>", mdFilePath));
			resp.Write("<div style='border: 1px solid #000'>");
			resp.Write(this.ProcessMarkdown(this.GetMarkdown(mdFilePath)));
			resp.Write("</div>");
			resp.Write("</body>");
			resp.Write("</html>");
		}
		#endregion Process request

		#region Get markdown
		private string GetMarkdown(string path)
		{
			if (File.Exists(path))
			{
				return File.ReadAllText(path);
			}

			return "File not found";
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

			string absolutePath = HttpContext.Current.Server.MapPath(fromConf);

			return absolutePath;
		}
		#endregion Helper: markdown root dir
	}
}