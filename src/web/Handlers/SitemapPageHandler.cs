using System;
using System.Web;
using LocalDocs.Web.Handlers.MarkdownSupport;
using System.IO;
using LocalDocs.Web.SparkStuff;
using System.Collections.Generic;

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

			vm.Sitemap = this.BuildSitemap(targetRootDir);

			SparkRenderer renderer = new SparkRenderer();
			string sitemapOutput = renderer.Render(sitemapPath, vm);

			vm.MarkdownHtml = sitemapOutput;
			string output = renderer.Render(target.TemplateFile, vm);

			return output;
		}
		#endregion

		#region Build sitemap
		private Sitemap BuildSitemap(string targetRootDir)
		{
			Sitemap res = new Sitemap();

			Queue<string> q = new Queue<string>();

			q.Enqueue(targetRootDir);

			string cur;
			string webpath;
			SitemapEntry entry;
			while (q.Count > 0)
			{
				cur = q.Dequeue();

				//
				// get all the subdirectories
				//
				foreach (string s in Directory.GetDirectories(cur))
				{
					if (s.EndsWith(Constants.LayoutFolderName))
					{
						continue;
					}

					q.Enqueue(s);
				}

				//
				// create the request path for the current dir
				//
				webpath = cur.Remove(0, targetRootDir.Length-1);
				if (!webpath.EndsWith("/"))
				{
					webpath = String.Concat(webpath, "/");
				}

				//
				// get all the markdown files
				//
				foreach (string s in Directory.GetFiles(cur, "*.md"))
				{
					entry = new SitemapEntry();
					entry.Namespace = webpath;
					entry.Name = Path.GetFileNameWithoutExtension(s);
					entry.Url = String.Concat(webpath, entry.Name);

					res.Entries.Add(entry);
				}
			}

			return res;
		}
		#endregion Build sitemap
	}
}