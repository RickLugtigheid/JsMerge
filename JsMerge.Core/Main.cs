using Newtonsoft.Json;

namespace JsMerge.Core
{
	public static class Main
	{
		public const string VERSION = "1.2.0";
		#region [Sub systems]
		/// <summary>
		/// A subsystem for handling query strings
		/// </summary>
		public static QueryHandler Query { get; private set; }
		/// <summary>
		/// A subsystem for handling logging
		/// </summary>
		public static LogWriter Log { get; private set; }
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
			Query	= new QueryHandler();
			Log		= new LogWriter();
			Config	= new Dictionary<string, MergeConfig>();

			// Check if a config file is present
			//
			if (File.Exists(WorkDirectory + "/.jsmerge"))
			{
				// Load our config
				using (StreamReader stream = new StreamReader(WorkDirectory + "/.jsmerge"))
				{
					Config = JsonConvert.DeserializeObject<Dictionary<string, MergeConfig>>(stream.ReadToEnd(), new JsonSerializerSettings()
					{
						Error = HandleConfigDeserializationError
					});
				}
			}
			else
			{
				Log.Error("No .jsmerge config file found in '" + workDirectory + '\'');
			}
		}

		private static void HandleConfigDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
		{
			Exception exception = e.ErrorContext.Error;

			// Check if the line we have an error on is the schema line
			bool isSchema = exception.Message.Contains("$schema");

			// If we do not have an error on the schema line we log what went wrong so the user can fix the syntax issue
			//
			if (!isSchema)
			{
				Log.Warning($"'{exception.Message}' when parsing '.jsmerge' file");
			}

			// Set the error as Handled so the parser can continue
			e.ErrorContext.Handled = true;
		}

		/// <summary>
		/// Creates a new merge file with the given configuration
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="config"></param>
		public static MergeResult Merge(string fileName, MergeConfig config)
		{
			// Check if 'include' property is given
			//
			if (config.include == null)
			{
				Log.Error($".jsmerge - {fileName}.include not set!");
				Environment.Exit(-1);
			}

			Log.Verbose("[Merge]: Started for '" + fileName + '\'', 1);
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
			Log.Verbose("[Query]: Executing '" + query + '\'', 3);

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
