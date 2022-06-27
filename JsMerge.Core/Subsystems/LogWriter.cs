using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsMerge.Core
{
	public class LogWriter
	{
		public int verbosity = 0;

		/// <summary>
		/// Logs a verbose message
		/// </summary>
		/// <param name="message">Message to log</param>
		/// <param name="requiredLevel">Required verbosity level for the message to be logged</param>
		public void Verbose(string message, int requiredLevel)
		{
			if (verbosity >= requiredLevel)
			{
				Console.WriteLine(message);
			}
		}

		/// <summary>
		/// Logs a message as an error
		/// </summary>
		/// <param name="message">Message to log</param>
		public void Error(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Error: " + message);
			Console.ResetColor();
		}
	}
}
