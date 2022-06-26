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

			// Check if a config is present
			//
			if (Core.Main.Config == null)
			{
				Console.WriteLine($"No .jsmerge config file found in {Core.Main.WorkDirectory}");
				return;
			}

			// Create a new merge file for each config item
			//
			foreach (KeyValuePair<string, MergeConfig> configItem in Core.Main.Config)
			{
				MergeResult result = Core.Main.Merge(configItem.Key, configItem.Value);
				result.Save();
			}
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