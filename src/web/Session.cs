using System;
using System.Web;

namespace LocalDocs.Web
{
	[Serializable]
	public class Session
	{
		/// <summary>
		/// The key in the actual session that holds the instance
		/// </summary>
		private static readonly string SessionKey = "sesid";

		#region Singleton-ish
		public static Session GetInstance()
		{
			if (!HasSession())
			{
				HttpContext.Current.Session[SessionKey] = new Session();
			}

			return (Session)HttpContext.Current.Session[SessionKey];
		}
		#endregion Singleton-ish

		public static bool HasSession()
		{
			if (HttpContext.Current == null)
			{
				throw new InvalidOperationException("There is no HttpContext");
			}

			return HttpContext.Current.Session[SessionKey] != null;
		}

		#region Constructor
		private Session()
		{
		}
		#endregion Constructor

		/// <summary>
		/// Gets or sets the ID of the current target site
		/// </summary>
		public string TargetSiteId { get; set; }

		/// <summary>
		/// Gets or sets the root of the website
		/// </summary>
		public string WebRoot { get; set; }
	}
}