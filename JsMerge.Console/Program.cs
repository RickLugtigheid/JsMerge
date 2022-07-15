using JsMerge.Core;

namespace JsMerge
{
	public class Program
	{
		static void Main(string[] args)
		{
			Options options = new Options(args);

			// Initialize our Main class with our current directory
			Core.Main.Initialize(
				GetWorkdir(options)
			);
			Core.Main.Log.verbosity = options.Verbosity;

			// Check if a config is present
			//
			if (Core.Main.Config == null)
			{
				Console.WriteLine($"No .jsmerge config file found in {Core.Main.WorkDirectory}");
				return;
			}

			// Check if watchdog is requested
			//
			if (options.Watchdog)
			{
				// Add watchers foreach config item
				//
				foreach (KeyValuePair<string, MergeConfig> configItem in Core.Main.Config)
				{
					// Add a watcher foreach file/directory incuded by this config item
					//
					foreach (string path in configItem.Value.include)
					{
						string fullPath = Core.Main.WorkDirectory + path;

						FileWatcher watcher = null;
						if (File.Exists(fullPath))
						{
							watcher = FileWatcher.WatchFile(fullPath);
						}
						else if (fullPath.EndsWith('*'))
						{
							fullPath = fullPath.Substring(0, fullPath.Length - 1);
							if (Directory.Exists(fullPath))
							{
								watcher = FileWatcher.WatchDir(fullPath);
							}
						}

						if (watcher != null)
						{
							Console.WriteLine("Added event Changed for '" + path + "'");
							watcher.OnChange += (sender, e) =>
							{
								MergeResult result = Core.Main.Merge(configItem.Key, configItem.Value);
								result.Save();
							};
						}
					}
				}

				// Wait for the user to stop the watcher
				Console.WriteLine("Type 'quit' or 'exit' to stop.");
				string input;
				while ((input = Console.ReadLine()) != null)
				{
					if (input == "quit" || input == "exit")
					{
						break; // Stop our program
					}
				}
			}
			// Else just trigger a merge
			else
			{
				// Create a new merge file for each config item
				//
				foreach (KeyValuePair<string, MergeConfig> configItem in Core.Main.Config)
				{
					MergeResult result = Core.Main.Merge(configItem.Key, configItem.Value);
					result.Save();
				}
			}
		}

		/// <summary>
		/// Converts the given to a work directory path
		/// </summary>
		/// <param name="argument"></param>
		/// <returns>Work directory</returns>
		static string GetWorkdir(Options options)
		{
			// Set our result to the current work directory
			string result = Environment.CurrentDirectory;

			// Check if an argument is given
			//
			if (options.WorkDirectory == null || options.WorkDirectory == string.Empty)
			{
				return result;
			}

			// Check if the given argument is an addition to the current work directory
			//
			if (options.WorkDirectory.StartsWith('.'))
			{
				return result + '/' + options.WorkDirectory;
			}

			// Return the argument as full path (if a full path is given)
			return options.WorkDirectory;
		}
	}
}