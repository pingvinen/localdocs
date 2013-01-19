using System;

namespace LocalDocs.Web
{
	public class TargetSite
	{
		public TargetSite()
		{
			this.HasCustomLayout = false;
			this.HasLoadedConfig = false;
		}

		public string Id { get; set; }
		public string Name { get; set; }
		public string Root { get; set; }
		public string TemplateFile { get; set; }
		public bool HasCustomLayout { get; set; }
		public TargetConfig Config { get; set; }
		public bool HasLoadedConfig { get; set; }
	}
}