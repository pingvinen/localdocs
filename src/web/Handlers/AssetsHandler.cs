using System;
using System.Web;
using System.Collections.Specialized;
using System.IO;

namespace LocalDocs.Web.Handlers
{
	/// <summary>
	/// Handles requests for assets
	/// </summary>
	public class AssetsHandler : IHandler
	{
		public AssetsHandler()
		{
		}

		#region IHandler implementation
		public void HandleRequest(HttpContext context, PageContext pageContext)
		{
			HttpRequest req = context.Request;
			HttpResponse resp = context.Response;

			string mdRoot = this.GetMarkdownRootDir(pageContext);
			string reqPath = req.Path.Remove(0, 1);

			string path = Path.Combine(mdRoot, Constants.LayoutFolderName, reqPath);

			if (File.Exists(path))
			{
				resp.WriteFile(path, true);
			}
			else
			{
				resp.StatusCode = 404;
			}
		}
		#endregion

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
	}
}