using Newtonsoft.Json;
using System.Text;

namespace JsMerge.Core
{
	public static class Main
	{
		#region [Sub systems]
		/// <summary>
		/// A subsystem for handling query strings
		/// </summary>
		public static QueryHandler Query { get; private set; }
		#endregion
		public static Dictionary<string, MergeConfig>? Config { get; private set; }

		public static string WorkDirectory { get; private set; }

		/// <summary>
		/// Initializes al required resources
		/// </summary>
		/// <param name="workDirectory"></param>
		public static void Initialize(string workDirectory)
		{
			WorkDirectory = workDirectory;

			// Create our subsystem objects
			//
			Query = new QueryHandler();
			Config = new Dictionary<string, MergeConfig>();

			// Check if a config file is present
			//
			if (File.Exists(WorkDirectory + "/.jsmerge"))
			{
				// Load our config
				using (StreamReader stream = new StreamReader(WorkDirectory + "/.jsmerge"))
				{
					Config = JsonConvert.DeserializeObject<Dictionary<string, MergeConfig>>(stream.ReadToEnd());
				}
			}
		}

		/// <summary>
		/// Creates a new merge file with the given configuration
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="config"></param>
		public static MergeResult Merge(string fileName, MergeConfig config)
		{
			// Create a new merge result
			//
			MergeResult result = new MergeResult(fileName, config);

			// Read all given queries
			//
			foreach (string query in config.include)
			{
				// Execute our query
				QueryResult queryResult = ExecuteQuery(query);

				// Add the resulting contents to our merge result
				result.Append(queryResult);
			}

			return result;
		}

		/// <summary>
		/// Executes a file query string
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public static QueryResult ExecuteQuery(string query)
		{
			QueryResult result = QueryResult.Empty;
			// Check if our query is a function
			if (Query.IsFunction(query))
			{
				// Parse our query as function
				QueryFunction function = Query.ParseFunction(query);

				// Execute our function and get its result
				result = Query.Execute(function);
			}
			else
			{
				// Else just execute our query the default way
				result = Query.Execute(query);
			}
			return result;
		}
	}
}
