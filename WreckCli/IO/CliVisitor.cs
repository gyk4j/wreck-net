
using System;
using System.Collections.Generic;
using System.IO;

using Java.NIO.File;
using log4net;
using Wreck.Entity;
using Wreck.IO;
using Wreck.IO.Task;
using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.IO
{
	/// <summary>
	/// Description of CliVisitor.
	/// </summary>
	public class CliVisitor : AbstractFileVisitor
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(AbstractFileVisitor));
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		CliWorker progressWorker = null;
		ITask task = null;
		
		public CliVisitor(CliWorker progressWorker, ITask task) : base()
		{
			this.progressWorker = progressWorker;
			this.task = task;
		}
	
		public override FileVisitResult PreVisitDirectory(DirectoryInfo dir)
		{
			FileVisit visit = new FileVisit(dir);
			progressWorker.Publish(visit);
			
			base.PreVisitDirectory(dir);
			task.PreVisitDirectory(Suggestions, dir);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult PostVisitDirectory(DirectoryInfo dir, IOException exc)
		{			
			base.PostVisitDirectory(dir, exc);
			task.PostVisitDirectory(Suggestions, dir);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFile(FileInfo file)
		{
			if(file.Name.Equals(R.Strings.SkipDesktopIni) ||
			        file.Name.Equals(R.Strings.LogFileName))
				return FileVisitResult.Continue;
			
			base.VisitFile(file);
			
			FileVisit visit = new FileVisit(file);
			progressWorker.Publish(visit);
			
			task.VisitFile(Suggestions, file);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFileFailed(FileSystemInfo file, IOException exc)
		{			
			base.VisitFileFailed(file, exc);
			task.VisitFileFailed(Suggestions, file, exc);
			
			return FileVisitResult.Continue;
		}
	}
}
