
using System;
using System.Collections.Generic;
using System.IO;

using Java.NIO.File;
using log4net;
using Wreck.Entity;
using Wreck.IO.Task;
using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.IO
{
	class GuiVisitor : AbstractFileVisitor
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(GuiVisitor));
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		GuiWorker progressWorker = null;
		ITask task = null;
		
		public GuiVisitor(GuiWorker progressWorker, ITask task) : base()
		{
			this.progressWorker = progressWorker;
			this.task = task;
		}
		
		public override FileVisitResult PreVisitDirectory(DirectoryInfo dir)
		{
			
			// Stop immediately once cancelled
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			
			FileVisit visit = new FileVisit(dir);
			progressWorker.Publish(visit);
			
			base.PreVisitDirectory(dir);
			task.PreVisitDirectory(Suggestions, dir);
			progressWorker.UpdateFileList(dir, Suggestions);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult PostVisitDirectory(DirectoryInfo dir, IOException exc)
		{
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			
			base.PostVisitDirectory(dir, exc);
			
			task.PostVisitDirectory(Suggestions, dir);
			progressWorker.UpdateFileList(dir, Suggestions);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFile(FileInfo file)
		{
			// Stop immediately once cancelled
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			else if(file.Name.Equals(R.Strings.SkipDesktopIni) ||
			        file.Name.Equals(R.Strings.LogFileName))
				return FileVisitResult.Continue;
			
			base.VisitFile(file);
			
			FileVisit visit = new FileVisit(file);
			progressWorker.Publish(visit);
			
			task.VisitFile(Suggestions, file);
			progressWorker.UpdateFileList(file, Suggestions);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFileFailed(FileSystemInfo file, IOException exc)
		{
			// Stop immediately once cancelled
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			
			base.VisitFileFailed(file, exc);
			
			task.VisitFileFailed(Suggestions, file, exc);
			progressWorker.UpdateFileList(file, Suggestions);
			
			return FileVisitResult.Continue;
		}
	}
}
