using System;
using System.Configuration;

namespace LocalDocs.Web
{
	public class TargetSitesConfigSection : ConfigurationSection
	{
		[ConfigurationProperty("", IsRequired=true, IsDefaultCollection=true)]
		public TargetSitesElementCollection Instances
		{
			get { return (TargetSitesElementCollection)this[""]; }
			set { this[""] = value; }
		}
	}
}