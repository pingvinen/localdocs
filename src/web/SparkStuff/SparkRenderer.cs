using System;
using Spark;
using Spark.FileSystem;
using System.IO;
using LocalDocs.Web.Handlers.MarkdownSupport;

namespace LocalDocs.Web.SparkStuff
{
	public class SparkRenderer
	{
		private ISparkViewEngine engine;

		public SparkRenderer()
		{
			SparkSettings settings = new SparkSettings();
			settings.SetPageBaseType(typeof(MarkdownPageTemplate));
			settings.DefaultLanguage = LanguageType.CSharp;
			settings.Debug = true;

			settings.AddNamespace("System");

			this.engine = new SparkViewEngine(settings);
		}

		public string Render(string templateFile, ViewModel viewmodel)
		{
			string fName = Path.GetFileName(templateFile);
			string fDir = Path.GetDirectoryName(templateFile);

			var descriptor = new SparkViewDescriptor();
			descriptor.AddTemplate(fName);
			descriptor.TargetNamespace = "templates";

			this.engine.ViewFolder = new FileSystemViewFolder(fDir);
		
			var view = (MarkdownPageTemplate)this.engine.CreateInstance(descriptor);

			StringWriter writer = new StringWriter();

			try
			{
				view.Model = viewmodel;
				view.RenderView(writer);
				return writer.ToString();
			}
			finally
			{
				this.engine.ReleaseInstance(view);
			}
		}
	}
}