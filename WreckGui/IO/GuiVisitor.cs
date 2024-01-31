
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
	class GuiVisitor : AbstractFileVisitor
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(GuiVisitor));
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		GuiWorker progressWorker = null;
		
		public GuiVisitor(GuiWorker progressWorker) : base()
		{
			this.progressWorker = progressWorker;
		}
		
		public override FileVisitResult PreVisitDirectory(DirectoryInfo dir)
		{
			
			// Stop immediately once cancelled
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			
			FileVisit visit = new FileVisit(dir);
			progressWorker.Publish(visit);
			
			base.PreVisitDirectory(dir);
			progressWorker.Task.PreVisitDirectory(Suggestions, dir);
			progressWorker.UpdateFileList(dir, Suggestions);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult PostVisitDirectory(DirectoryInfo dir, IOException exc)
		{
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			
			base.PostVisitDirectory(dir, exc);
			
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
			
			base.VisitFile(file);
			
			FileVisit visit = new FileVisit(file);
			progressWorker.Publish(visit);
			
			progressWorker.Task.VisitFile(Suggestions, file);
			progressWorker.UpdateFileList(file, Suggestions);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFileFailed(FileSystemInfo file, IOException exc)
		{
			// Stop immediately once cancelled
			if(progressWorker.IsCancelled())
				return FileVisitResult.Terminate;
			
			base.VisitFileFailed(file, exc);
			
			progressWorker.Task.VisitFileFailed(Suggestions, file, exc);
			progressWorker.UpdateFileList(file, Suggestions);
			
			return FileVisitResult.Continue;
		}
	}
}
