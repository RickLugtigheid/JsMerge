using JsMerge.Core;

namespace JsMerge
{
	public class Program
	{
		static void Main(string[] args)
		{
			// Initialize our Main class with our current directory
			Core.Main.Initialize(Environment.CurrentDirectory);

			// Test the query system
			//
			var files = Core.Main.ExecuteQuery("/*");
			var jquery = Core.Main.ExecuteQuery(":get(https://code.jquery.com/jquery-3.3.1.slim.min.js)");

			Console.WriteLine($"{files.type}: {files.result}");
			Console.WriteLine($"{jquery.type}: {jquery.result}");
		}
	}
}