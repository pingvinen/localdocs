using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.IO;
using MarkdownSharp;
using LocalDocs.Web.SparkStuff;
using LocalDocs.Web.Handlers.MarkdownSupport;

namespace LocalDocs.Web.Handlers
{
	/// <summary>
	/// Handles requests for the sitemap
	/// </summary>
	public class SitemapPageHandler : IHandler
	{
		public SitemapPageHandler()
		{
		}

		#region IHandler implementation
		public void HandleRequest(HttpContext context)
		{
			throw new NotImplementedException();
		}
		#endregion IHandler implementation
	}
}