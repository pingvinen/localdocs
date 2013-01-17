using System;
using System.Web;
using LocalDocs.Web.Handlers;

namespace LocalDocs.Web
{
	public static class HandlerFactory
	{
		public static IHandler GetInstance(HttpContext context)
		{
			HttpRequest req = context.Request;

			if (req.Path.Equals(Constants.SwitchSitePath))
			{
				return new SwitchSiteHandler();
			}

			if (req.Path.StartsWith(Constants.AssetsPath))
			{
				return new AssetsHandler();
			}

			return new MarkdownPageHandler();
		}
	}
}