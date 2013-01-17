using System;
using System.Web;
using LocalDocs.Web.Handlers;

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

			#region Web root
			if (String.IsNullOrEmpty(this.webRoot))
			{
				this.webRoot = context.Server.MapPath("/");
			}
			#endregion Web root

			IHandler handler = null;

			switch (req.Path)
			{
				case Constants.SwitchSitePath:
				{
					handler = new SwitchSiteHandler();
					break;
				}

				default:
				{
					handler = new MarkdownPageHandler();
					break;
				}
			}

			PageContext pcon = new PageContext() {
				Site = this.targetSite,
				WebRoot = this.webRoot
			};

			handler.HandleRequest(context, pcon);

			// this needs to be run after for
			// "switch site"-requests, to persist
			// the switch
			this.targetSite = pcon.Site;
		}
		#endregion Process request
	}
}