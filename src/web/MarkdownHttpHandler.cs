using System;
using System.Web;
using System.Configuration;
using System.IO;

namespace LocalDocs.Web
{
	/// <summary>
	/// Handles all requests by looking for, and outputting, a Markdown
	/// file that matches the location in the url
	/// </summary>
	public class MarkdownHttpHandler : IHttpHandler
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LocalDocs.Web.MarkdownHttpHandler"/> class.
		/// </summary>
		public MarkdownHttpHandler()
		{
		}

		/// <summary>
		/// Get whether this handler instance can be used by
		/// multiple requests
		/// </summary>
		public bool IsReusable { get { return true; } }

		#region Process request
		public void ProcessRequest(HttpContext context)
		{
			string rootDir = this.GetMarkdownRootDir();

			HttpResponse resp = context.Response;
			resp.Write("<html>");
			resp.Write("<body>");
			resp.Write("<h1>Good neeeews everyone</h1>");
			resp.Write(String.Format("<p>Markdown root directoy: '{0}'</p>", rootDir));
			resp.Write("</body>");
			resp.Write("</html>");
		}
		#endregion Process request

		private string GetMarkdownRootDir()
		{
			string fromConf = ConfigurationManager.AppSettings["MarkdownFilesRootFolder"];

			if (Path.IsPathRooted(fromConf))
			{
				return fromConf;
			}

			string absolutePath = HttpContext.Current.Server.MapPath(fromConf);

			return absolutePath;
		}
	}
}