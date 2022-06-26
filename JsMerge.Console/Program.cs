using JsMerge.Core;

namespace JsMerge
{
	public class Program
	{
		static void Main(string[] args)
		{
			// Initialize our Main class with our current directory
			Core.Main.Initialize(
				GetWorkdir(args.Length > 1 ? args[0] : null)
			);

			// Test the query system
			//
			var files = Core.Main.ExecuteQuery("/*");
			var jquery = Core.Main.ExecuteQuery(":get(https://code.jquery.com/jquery-3.3.1.slim.min.js)");

			Console.WriteLine($"{files.type}: {files.result}");
			Console.WriteLine($"{jquery.type}: {jquery.result}");
		}

		/// <summary>
		/// Converts the given to a work directory path
		/// </summary>
		/// <param name="argument"></param>
		/// <returns>Work directory</returns>
		static string GetWorkdir(string? argument)
		{
			string result = Environment.CurrentDirectory;

			// Check if an argument is given
			//
			if (argument == null || argument == string.Empty)
			{
				return result;
			}

			// Check if the given argument is an addition to the current work directory
			//
			if (argument.StartsWith('.'))
			{
				return result + argument;
			}

			// Return the argument as full path (if a full path is given)
			return argument;
		}
	}
}