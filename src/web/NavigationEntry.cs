using System;
using System.Collections.Generic;

namespace LocalDocs.Web
{
	public class NavigationEntry
	{
		public string Label { get; set; }
		public string Path { get; set; }
		public IList<NavigationEntry> Items { get; set; }
	}
}