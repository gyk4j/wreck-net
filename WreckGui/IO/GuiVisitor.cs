
using System;
using System.Collections.Generic;
using System.IO;

using Java.NIO.File;
using log4net;
using Wreck.Entity;
using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.IO
{
	class GuiVisitor : SimpleFileVisitor
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(GuiVisitor));
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		GuiWorker progressWorker = null;
		
		private readonly List<DirectoryInfo> directories;
		private readonly Dictionary<CorrectionEnum, DateTime> suggestions;
		
		public GuiVisitor(GuiWorker progressWorker) : base()
		{
			this.progressWorker = progressWorker;
			this.directories = new List<DirectoryInfo>();
			this.suggestions = new Dictionary<CorrectionEnum, DateTime>();
		}
		
		public List<DirectoryInfo> Directories { get { return directories; } }
		public Dictionary<CorrectionEnum, DateTime> Suggestions { get { return suggestions; } }
		
		public override FileVisitResult PreVisitDirectory(DirectoryInfo dir)
		{
			
			// Stop immediately once cancelled
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			
			FileVisit visit = new FileVisit(dir);
			progressWorker.Publish(visit);
			
			STATS.Count(FileEvent.DirectoryFound);
			STATS.Count(FileEvent.FileFound);
			
			Directories.Add(dir);
			
			Suggestions.Clear();
			progressWorker.Task.PreVisitDirectory(Suggestions, dir);
			progressWorker.UpdateFileList(dir, Suggestions);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult PostVisitDirectory(DirectoryInfo dir, IOException exc)
		{
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			
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
			progressWorker.UpdateFileList(dir, Suggestions);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFile(FileInfo file)
		{
			// Stop immediately once cancelled
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			else if(file.Name.Equals(R.strings.SKIP_DESKTOP_INI) ||
			        file.Name.Equals(R.strings.LOG_FILE_NAME))
				return FileVisitResult.Continue;
			
			FileVisit visit = new FileVisit(file);
			progressWorker.Publish(visit);
			STATS.Count(FileEvent.FileFound);
			
			Suggestions.Clear();
			progressWorker.Task.VisitFile(Suggestions, file);
			progressWorker.UpdateFileList(file, Suggestions);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFileFailed(FileSystemInfo file, IOException exc)
		{
			// Stop immediately once cancelled
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			
			LOG.ErrorFormat("{0}: {1}", file.Name, exc.ToString());
			STATS.Count(FileEvent.FileError);
			
			progressWorker.Task.VisitFileFailed(Suggestions, file, exc);
			progressWorker.UpdateFileList(file, Suggestions);
			
			return FileVisitResult.Continue;
		}
	}
}
