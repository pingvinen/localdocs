using System;
using System.Configuration;

namespace LocalDocs.Web
{
	public class TargetSitesElement : ConfigurationElement
	{
		[ConfigurationProperty("id", IsKey=true, IsRequired=true)]
		public string Id
		{
			get { return (string)base["id"]; }
			set { base["id"] = value; }
		}

		[ConfigurationProperty("name", IsRequired=true)]
		public string Name
		{
			get { return (string)base["name"]; }
			set { base["name"] = value; }
		}

		[ConfigurationProperty("root", IsRequired=true)]
		public string Root
		{
			get { return (string)base["root"]; }
			set { base["root"] = value; }
		}

		[ConfigurationProperty("isdefault", IsRequired=false, DefaultValue=false)]
		public bool IsDefault
		{
			get { return (bool)base["isdefault"]; }
			set { base["isdefault"] = value; }
		}
	}
}