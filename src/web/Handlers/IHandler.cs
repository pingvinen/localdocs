using System;
using System.Web;

namespace LocalDocs.Web.Handlers
{
	public interface IHandler
	{
		void HandleRequest(HttpContext context, PageContext pageContext);
	}
}