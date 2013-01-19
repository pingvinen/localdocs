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
	public class SitemapPageHandler : PageHandlerBase
	{
		public SitemapPageHandler()
		{
		}

		#region implemented abstract members of LocalDocs.Web.Handlers.PageHandlerBase
		protected override string InternalHandler(HttpContext context, TargetSite target, string targetRootDir, string requestedPath, ViewModel vm)
		{
			string sitemapPath;
			if (target.HasCustomLayout)
			{
				sitemapPath = Path.Combine(targetRootDir, Constants.LayoutFolderName, Constants.SitemapFileName);
			}
			else
			{
				sitemapPath = Path.Combine(Session.GetInstance().WebRoot, Constants.LayoutFolderName, Constants.SitemapFileName);
			}

			if (!File.Exists(sitemapPath))
			{
				throw new InvalidOperationException(String.Format("Sitemap template file for '{0}' is missing. It should be at '{1}'", target.Name, sitemapPath));
			}

			vm.Sitemap = this.GetSitemap(targetRootDir);

			SparkRenderer renderer = new SparkRenderer();
			string sitemapOutput = renderer.Render(sitemapPath, vm);

			vm.MarkdownHtml = sitemapOutput;
			string output = renderer.Render(target.TemplateFile, vm);

			return output;
		}
		#endregion

		private Sitemap GetSitemap(string targetRootDir)
		{
			return new Sitemap();
		}
	}
}