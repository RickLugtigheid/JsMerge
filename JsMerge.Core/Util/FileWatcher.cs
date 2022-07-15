using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsMerge.Core
{
    /// <summary>
    /// A FileSystemWatcher wraper class that executes its events once instead of twice
    /// </summary>
	public class FileWatcher
	{
        /// <summary>
        /// Event that is called when a file is changed
        /// </summary>
		public event EventHandler<FileSystemEventArgs> OnChange;
		private FileSystemWatcher _watcher;
        private bool _firstEventCalled = false;

        /// <summary>
        /// Create a new watcher
        /// </summary>
        /// <param name="watcher"></param>
        /// <param name="includeSubdirectories"></param>
		public FileWatcher(FileSystemWatcher watcher, bool includeSubdirectories)
		{
            // Add event listners
            //
            watcher.Changed += new FileSystemEventHandler(ChangeHandler);
            watcher.Created += new FileSystemEventHandler(ChangeHandler);
            watcher.Deleted += new FileSystemEventHandler(ChangeHandler);

            // Begin watching
            //
            watcher.IncludeSubdirectories   = includeSubdirectories;
            watcher.EnableRaisingEvents     = true;

            _watcher = watcher;
        }

        /// <summary>
        /// Watches a directory for changes
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="watchPattern"></param>
        public static FileWatcher WatchDir(string path, string watchPattern = "*.js")
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Filter files when calling event
            watcher.Filter = watchPattern;
            return new FileWatcher(watcher, true);
        }

        /// <summary>
        /// Watches a file for changes
        /// </summary>
        /// <param name="file">File path of the file to watch</param>
        public static FileWatcher WatchFile(string file)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(file);

            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Filter files when calling event
            watcher.Filter = Path.GetFileName(file);
            return new FileWatcher(watcher, false);
        }

        /// <summary>
        /// Handle any changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeHandler(object sender, FileSystemEventArgs e)
		{
            if (!_firstEventCalled)
			{
                Main.Log.Verbose("\n[Watcher]: File " + e.FullPath + " " + e.ChangeType, 1);
                OnChange?.Invoke(sender, e);
                _firstEventCalled = true;
            }
            else
			{
                _firstEventCalled = false;
            }
		}
	}
}
