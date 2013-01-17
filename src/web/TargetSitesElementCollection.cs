using System;
using System.Configuration;

namespace LocalDocs.Web
{
	public class TargetSitesElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new TargetSitesElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((TargetSitesElement)element).Name;
		}
	}
}