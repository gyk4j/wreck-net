
using System;
using System.Collections.Generic;
using System.IO;

using Java.NIO.File;
using log4net;
using Wreck.IO;
using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.IO
{
	/// <summary>
	/// Description of CliVisitor.
	/// </summary>
	public class CliVisitor : SimpleFileVisitor
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(CliVisitor));
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		CliWorker progressWorker = null;
		
		private readonly List<DirectoryInfo> directories;
		private readonly Dictionary<CorrectionEnum, DateTime> suggestions;
		
		public CliVisitor(CliWorker progressWorker) : base()
		{
			this.progressWorker = progressWorker;
			this.directories = new List<DirectoryInfo>();
			this.suggestions = new Dictionary<CorrectionEnum, DateTime>();
		}
		
		public List<DirectoryInfo> Directories { get { return directories; } }
		public Dictionary<CorrectionEnum, DateTime> Suggestions { get { return suggestions; } }
	
		public override FileVisitResult PreVisitDirectory(DirectoryInfo dir)
		{
			STATS.Count(FileEvent.DirectoryFound);
			STATS.Count(FileEvent.FileFound);
			
			Directories.Add(dir);
			
			Suggestions.Clear();
			progressWorker.Task.PreVisitDirectory(Suggestions, dir);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult PostVisitDirectory(DirectoryInfo dir, IOException exc)
		{			
			if(exc != null)
			{
				LOG.Error(exc.ToString());
				return FileVisitResult.Continue;
			}
			
			bool ok = Directories.Remove(dir);
			
			if(!ok)
				LOG.WarnFormat("{0}: attrs = null", dir.FullName);
			
			Suggestions.Clear();
			progressWorker.Task.PostVisitDirectory(Suggestions, dir);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFile(FileInfo file)
		{
			if(file.Name.Equals(R.strings.SKIP_DESKTOP_INI) ||
			        file.Name.Equals(R.strings.LOG_FILE_NAME))
				return FileVisitResult.Continue;
			
			STATS.Count(FileEvent.FileFound);
			
			Suggestions.Clear();
			progressWorker.Task.VisitFile(Suggestions, file);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFileFailed(FileSystemInfo file, IOException exc)
		{			
			LOG.ErrorFormat("{0}: {1}", file.Name, exc.ToString());
			STATS.Count(FileEvent.FileError);
			
			progressWorker.Task.VisitFileFailed(Suggestions, file, exc);
			
			return FileVisitResult.Continue;
		}
	}
}
