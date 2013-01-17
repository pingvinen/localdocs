using System;
using System.Web;

namespace LocalDocs.Web
{
	/// <summary>
	/// Handles all requests by looking for, and outputting, a Markdown
	/// file that matches the location in the url
	/// </summary>
	public class LocalDocsHttpHandler : IHttpHandler
	{
		private TargetSite targetSite;

		private string webRoot;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalDocs.Web.MarkdownHttpHandler"/> class.
		/// </summary>
		public LocalDocsHttpHandler()
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
			#region Web root
			if (String.IsNullOrEmpty(this.webRoot))
			{
				this.webRoot = context.Server.MapPath("/");
			}
			#endregion Web root

			IHandler handler = HandlerFactory.GetInstance(context);
		
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