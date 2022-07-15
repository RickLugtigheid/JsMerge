using JsMerge.Core;
using Mono.Options;

namespace JsMerge
{
	public class Options
	{
		private readonly OptionSet _options;
		/// <summary>
		/// The given verbosity level
		/// </summary>
		public int Verbosity { get; private set; }
		/// <summary>
		/// The given work directory
		/// </summary>
		public string? WorkDirectory { get; private set; }
		/// <summary>
		/// If watchdog is requested
		/// </summary>
		public bool Watchdog { get; private set; }

		public Options(string[] args)
		{
			_options = new OptionSet
			{
				{ "h|help", "Shows a help message before exit", HandleHelp },
				{ "V|version", "Shows the JsMerge version", HandleVersion },
				{ "d|directory", "Sets the work directory used by the program", (dir) => { if (dir != null) WorkDirectory = dir; } },
				{ "w|watch", "Watches the included directories for changes and executes a merge on change", (watch) => Watchdog = watch != null },
				{ "v", "Increase debug message verbosity", HandleVerbosity }
			};

			List<string> extra;
			try
			{
				// parse the command line
				extra = _options.Parse(args);
			}
			catch (OptionException e)
			{
				// output some error message
				Console.Write("JsMerge: ");
				Console.WriteLine(e.Message);
				Console.WriteLine("Try `JsMerge --help' for more information.");
				return;
			}
		}

		private void HandleHelp(string h)
		{
			// Check if the help option is given
			//
			if (h != null)
			{
				Console.WriteLine("Usage: jsMerge\n");
				foreach (Option opt in _options)
				{
					Console.WriteLine("\t{0}\t{1}", opt.Prototype, opt.Description);
				}
				Environment.Exit(0);
			}
		}
		private void HandleVersion(string v)
		{
			// Check if the version option is given
			//
			if (v != null)
			{
				Console.WriteLine("v" + Main.VERSION);
				Environment.Exit(0);
			}
		}
		private void HandleVerbosity(string v)
		{
			// Check if the verbosity option is given
			//
			if (v != null)
			{
				Verbosity++;
			}
		}
	}
}
