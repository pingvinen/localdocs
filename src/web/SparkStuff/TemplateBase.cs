using System;
using Spark;
using System.Web.Mvc;

namespace LocalDocs.Web.SparkStuff
{
	public abstract class TemplateBase<TViewModel> : AbstractSparkView
	{
		public TemplateBase()
		{
		}

		public TViewModel Model { get; set; }
	}
}