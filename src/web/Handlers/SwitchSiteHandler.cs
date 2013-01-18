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
		public void HandleRequest(HttpContext context)
		{
			HttpRequest req = context.Request;
			HttpResponse resp = context.Response;
			Session ses = Session.GetInstance();

			NameValueCollection nvc = req.QueryString;
			if (String.IsNullOrEmpty(nvc["to"]))
			{
				resp.Redirect(req.UrlReferrer.AbsoluteUri, false);
			}

			try
			{
				ses.TargetSiteId = TargetSitesConfig.Get(nvc["to"]).Id;
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