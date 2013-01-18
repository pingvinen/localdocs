using System;
using System.IO;

namespace LocalDocs.Web
{
	public static class Helper
	{
		#region Get markdown root dit
		public static string GetMarkdownRootDir(string siteRoot, string webRoot)
		{
			if (Path.IsPathRooted(siteRoot))
			{
				return siteRoot;
			}

			string absolutePath = Path.Combine(webRoot, siteRoot);

			return absolutePath;
		}
		#endregion Get markdown root dit
	}
}