using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public static string WorkDirectory { get; private set; }

		public static void Initialize(string workDirectory)
		{
			WorkDirectory = workDirectory;

			// Create our subsystem objects
			//
			Query = new QueryHandler();
		}

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
