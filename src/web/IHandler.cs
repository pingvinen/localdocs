using System;
using System.Web;

namespace LocalDocs.Web
{
	public interface IHandler
	{
		void HandleRequest(HttpContext context);
	}
}