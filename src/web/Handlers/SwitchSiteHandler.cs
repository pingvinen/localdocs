using System;
using System.Web;
using System.Collections.Specialized;

namespace LocalDocs.Web.Handlers
{
	/// <summary>
	/// Handles "switch site"-requests
	/// </summary>
	public class SwitchSiteHandler : IHandler
	{
		public SwitchSiteHandler()
		{
		}

		#region IHandler implementation
		public void HandleRequest(HttpContext context, PageContext pageContext)
		{
			HttpRequest req = context.Request;
			HttpResponse resp = context.Response;

			NameValueCollection nvc = req.QueryString;
			if (String.IsNullOrEmpty(nvc["to"]))
			{
				resp.Redirect(req.UrlReferrer.AbsoluteUri, false);
			}

			try
			{
				pageContext.Site = TargetSitesConfig.Get(nvc["to"]);
				resp.Redirect("/", false);
			}

			catch (Exception)
			{
				resp.Redirect(req.UrlReferrer.AbsoluteUri, false);
			}
		}
		#endregion
	}
}