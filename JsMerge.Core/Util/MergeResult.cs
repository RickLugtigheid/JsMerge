using DouglasCrockford.JsMin;
using System.Text;

namespace JsMerge.Core
{
	public class MergeResult
	{
		private StringBuilder _contents = new StringBuilder();
		private string _fileName;

		public MergeConfig config;
		public MergeResult(string fileName, MergeConfig config)
		{
			this._fileName = fileName;
			this.config = config;
		}

		/// <summary>
		/// Appends the file contents of the query result to our merge
		/// </summary>
		/// <param name="queryResult"></param>
		public void Append(QueryResult queryResult)
		{
			// Preform the correct action for the given result
			//
			switch (queryResult.type)
			{
				case QueryResultType.FileList:
					foreach (string file in queryResult.value as string[])
					{
						Main.Log.Verbose("merging: " + file, 2);
						// If in debug mode add the name of the file where the following contents came from
						//
						if (config.debug)
						{
							_contents.AppendLine("<<<<<<<< " + Path.GetFileName(file));
						}

						// Read the contents of the file and append it to our merge result
						//
						using (StreamReader stream = new StreamReader(file))
						{
							_contents.AppendLine(stream.ReadToEnd());
						}
					}
					break;

				case QueryResultType.String:
					Main.Log.Verbose("merging: " + queryResult.origin, 2);
					// If in debug mode add the origin where the following contents came from
					//
					if (config.debug)
					{
						_contents.AppendLine("<<<<<<<< " + queryResult.origin);
					}

					// If the result contains a string we should add this string to our merge result
					_contents.AppendLine((string?)queryResult.value);
					break;
			}
		}

		/// <summary>
		/// Saves our merge result to file
		/// </summary>
		public void Save()
		{
			string contents = _contents.ToString();
			string fileName = _fileName;

			// Get out output directory
			//
			string dirOut = Main.WorkDirectory;
			if (config.dirOut != null && config.dirOut != string.Empty)
			{
				dirOut += '/' + config.dirOut;
			}

			// Check if our contents should be minified
			//
			if (config.minify)
			{
				contents = new JsMinifier().Minify(_contents.ToString());

				// Also remove newlines since the package doesn't do that automaticly
				contents = contents.ReplaceLineEndings(string.Empty);

				fileName += ".min";
			}

			// Create a new file with the given name
			//
			string fileOut = dirOut + '/' + fileName + ".js";
			using (StreamWriter stream = new StreamWriter(fileOut))
			{
				stream.Write(contents);
			}
			Main.Log.Verbose("[Merge]: Saved to '" + fileOut + '\'', 1);
		}
	}
}
