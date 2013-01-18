using System;
using System.Web;

namespace LocalDocs.Web
{
	/// <summary>
	/// Handles all requests by looking for, and outputting, a Markdown
	/// file that matches the location in the url
	/// </summary>
	public class LocalDocsHttpHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LocalDocs.Web.MarkdownHttpHandler"/> class.
		/// </summary>
		public LocalDocsHttpHandler()
		{
		}

		/// <summary>
		/// Get whether this handler instance can be used by
		/// multiple requests
		/// </summary>
		public bool IsReusable { get { return false; } }

		#region Process request
		public void ProcessRequest(HttpContext context)
		{
			if (!Session.HasSession())
			{
				this.PrepareSession(context);
			}

			IHandler handler = HandlerFactory.GetInstance(context);
		
			handler.HandleRequest(context);
		}
		#endregion Process request

		#region Prepare session
		private void PrepareSession(HttpContext context)
		{
			Session ses = Session.GetInstance();
			ses.WebRoot = context.Server.MapPath("/");
			ses.TargetSiteId = TargetSitesConfig.GetDefaultSite().Id;
		}
		#endregion Prepare session
	}
}