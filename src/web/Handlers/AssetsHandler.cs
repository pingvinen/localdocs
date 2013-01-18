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
		public void HandleRequest(HttpContext context)
		{
			HttpRequest req = context.Request;
			HttpResponse resp = context.Response;
			Session ses = Session.GetInstance();
			TargetSite target = TargetSitesConfig.Get(ses.TargetSiteId);

			string mdRoot = ses.WebRoot;
			if (target.HasCustomLayout)
			{
				mdRoot = Helper.GetMarkdownRootDir(target.Root, ses.WebRoot);
			}

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
	}
}