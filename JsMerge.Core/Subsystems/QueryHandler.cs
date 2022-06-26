using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JsMerge.Core
{
	public class QueryHandler
	{
		/// <summary>
		/// Check if the query string is a function string
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public bool IsFunction(string query) => query.StartsWith(':');

		/// <summary>
		/// Parses a query as function
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public QueryFunction ParseFunction(string query)
		{
			// Remove the first char (the function indicator char ':')
			query = query.Substring(1, query.Length - 1);

			StringBuilder value = new StringBuilder();
			List<string> parameters = new List<string>();

			// Check all characters
			for (int i = 0; i < query.Length; i++)
			{
				char c = query[i];

				// If 'c' is a , or ( or ) we should add the current value to parameters
				if (c == '(' || c == ',' || c == ')')
				{
					parameters.Add(value.ToString());
					value.Clear();
					continue;
				}

				// If no action should be taken for the given char we should add it to our value
				value.Append(c);
			}

			// Use our first parameter as function name and remove it from parameters
			string name = parameters[0];
			parameters.RemoveAt(0);

			// Return our new function object
			return new QueryFunction(name, parameters.ToArray());
		}

		/// <summary>
		/// Execute function queries
		/// </summary>
		/// <param name="function"></param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		public QueryResult Execute(QueryFunction function)
		{
			// Get the execution method from our query function collection
			//
			if (QueryFunctionCollection.TryFind(function.name, out MethodInfo? foundMethod))
			{
				// Call our execution method
				object? result = foundMethod?.Invoke(null, function.parameters);

				// Check if our result is a query result
				if (result is QueryResult)
				{
					return (QueryResult)result;
				}
				else
				{
					// Tell the developer that the created function does not have the correct output type
					throw new InvalidCastException($"Output of function '{function.name}' is not of type QueryResult");
				}
			}
			else
			{
				// TODO: Log error message - Error: could not find a function with name '{function.name}'
				return new QueryResult(QueryResultType.String, null);
			}
		}

		/// <summary>
		/// Execute regex file queries
		/// </summary>
		/// <param name="filePath">A regex/normal path for selecting files</param>
		/// <returns></returns>
		public QueryResult Execute(string filePath)
		{
			// "/src/*"
			// "^/src/*
			// "$/src/*
			// "#/src/*
			// "@/src/*

			// :function()
			// ^regex.*
			Regex regexp = new Regex(filePath);
			// Get all js files in our work directory
			string[] files = Directory.GetFiles(Main.WorkDirectory, "*.js")
			// And select all that match our regex path pattern
				.Where(file => regexp.IsMatch(file)).ToArray();

			// Create our query result with our file list
			return new QueryResult(QueryResultType.FileList, files);
		}
	}

	internal static class QueryFunctionCollection
	{
		#region [Helper methods]
		public static bool TryFind(string name, out MethodInfo? method)
		{
			// Get our method
			method = typeof(QueryFunctionCollection).GetMethod(name);

			// Check if this method exists
			return method != null;
		}
		#endregion

		#region [Functions]
		// The following functions should use the naming convention for our query functions.
		//
		// Naming convention: 'lower camel case'
		//

		public static QueryResult get(string url)
		{
			// Make a simple get request to the given url and get its contents
			string contents = new HttpClient().GetStringAsync(url).Result;

			// Create our query result with our file contents
			return new QueryResult(QueryResultType.String, contents);
		}
		#endregion
	}
}
