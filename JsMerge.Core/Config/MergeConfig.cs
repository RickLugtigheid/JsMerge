namespace JsMerge.Core
{
	public struct MergeConfig
	{
		/// <summary>
		/// A boolean for development.
		/// <para>This will add '<<<<<<<< fileName' to the merge file every time file contents are added</para>
		/// </summary>
		public bool debug;
		/// <summary>
		/// Will minify the merge file
		/// </summary>
		public bool minify;

		/// <summary>
		/// A list of query strings of files to include
		/// </summary>
		public string[] include;

		/// <summary>
		/// The directory (with workDirectory as root) to add the merge file to
		/// </summary>
		public string dirOut;
	}
}
