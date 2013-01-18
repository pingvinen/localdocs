using System;
using System.Web;
using System.Collections.Specialized;
using System.IO;
using System.Collections.Generic;

namespace LocalDocs.Web.Handlers
{
	/// <summary>
	/// Handles requests for assets
	/// </summary>
	public class AssetsHandler : IHandler
	{
		private static IDictionary<string, string> MimeTypes = new Dictionary<string, string>() {
			{ ".css", "text/css" },
			{ ".js", "text/javascript" },
			{ ".swf", "application/x-shockwave-flash" },
			{ ".pdf", "application/pdf" },
			{ ".gif", "image/gif" },
			{ ".png", "image/png" },
			{ ".jpg", "image/jpeg" },
			{ ".jpeg", "image/jpeg" }
		};

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
				resp.ContentType = this.GetMimeType(path);
				resp.WriteFile(path, true);
			}
			else
			{
				resp.StatusCode = 404;
			}
		}
		#endregion

		#region Get mimetype
		private string GetMimeType(string path)
		{
			string ext = Path.GetExtension(path);

			string res;

			if (!MimeTypes.TryGetValue(ext, out res))
			{
				throw new NotSupportedException(String.Format("I do not know which mime-type to use for '{0}'", ext));
			}

			return res;
		}
		#endregion Get mimetype
	}
}