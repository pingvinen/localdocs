using System;
using System.Web;
using System.IO;
using LocalDocs.Web.SparkStuff;
using LocalDocs.Web.Handlers.MarkdownSupport;

namespace LocalDocs.Web.Handlers
{
	public abstract class PageHandlerBase : IHandler
	{
		public PageHandlerBase()
		{
		}

		#region IHandler implementation
		public void HandleRequest(HttpContext context)
		{
			HttpRequest req = context.Request;
			HttpResponse resp = context.Response;
			Session ses = Session.GetInstance();
			TargetSite target = TargetSitesConfig.Get(ses.TargetSiteId);

			string rootDir = Helper.GetTargetRootDir(target.Root, ses.WebRoot);
			string requestedPath = req.Path;

			// the default file is "index.md"
			if (requestedPath.Equals("/"))
			{
				requestedPath = "/index";
			}

			// remove the initial slash, so we can use Path.Combine
			requestedPath = requestedPath.Remove(0, 1);

			#region Template
			if (String.IsNullOrEmpty(target.TemplateFile))
			{
				Helper.SetTemplateFileOfTarget(target, rootDir, ses.WebRoot);
			}

			string templateFilePath = target.TemplateFile;
			#endregion Template

			if (!File.Exists(templateFilePath))
			{
				throw new InvalidOperationException(String.Format("Template file for '{0}' is missing. It should be at '{1}'", target.Name, templateFilePath));
			}

			ViewModel vm = this.InternalHandler(context, target, rootDir, requestedPath);
			vm.AvailableSites = Helper.GetAvailableTargets(target.Id);
			vm.Target = target;

			SparkRenderer renderer = new SparkRenderer();
			string output = renderer.Render(templateFilePath, vm);

			resp.Write(output);
		}
		#endregion IHandler implementation

		protected abstract ViewModel InternalHandler(HttpContext context, TargetSite target, string targetRootDir, string requestedPath);
	}
}