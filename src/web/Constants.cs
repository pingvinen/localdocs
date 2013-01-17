using System;

namespace LocalDocs.Web
{
	/// <summary>
	/// Holds various constants used throughout the system
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// The name of the folder that contains layout specific files
		/// </summary>
		public const string LayoutFolderName = "__layout";

		/// <summary>
		/// The name of the master file.
		/// </summary>
		public const string MasterFileName = "master.html";

		/// <summary>
		/// The request path of "switch site"-requests
		/// </summary>
		public const string SwitchSitePath = "/switchsite";

		/// <summary>
		/// The first part of the request path for assets
		/// </summary>
		public const string AssetsPath = "/assets/";
	}
}