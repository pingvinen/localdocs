using System;
using System.IO;
using MarkdownSharp;

namespace LocalDocs.Web
{
	public static class MarkdownHelper
	{
		#region Get markdown
		public static string GetMarkdown(string path)
		{
			if (File.Exists(path))
			{
				return File.ReadAllText(path);
			}

			return String.Format("File not found: '{0}'", path);
		}
		#endregion Get markdown

		#region Process markdown
		public static string ProcessMarkdown(string markdown)
		{
			Markdown md = new Markdown();

			return md.Transform(markdown);
		}
		#endregion Process markdown
	}
}