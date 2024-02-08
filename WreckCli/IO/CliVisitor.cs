
using System;
using System.Collections.Generic;
using System.IO;

using Java.NIO.File;
using log4net;
using Wreck.Entity;
using Wreck.IO;
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
		
		public CliVisitor(CliWorker progressWorker) : base()
		{
			this.progressWorker = progressWorker;
		}
	
		public override FileVisitResult PreVisitDirectory(DirectoryInfo dir)
		{
			FileVisit visit = new FileVisit(dir);
			progressWorker.Publish(visit);
			
			base.PreVisitDirectory(dir);
			progressWorker.Task.PreVisitDirectory(Suggestions, dir);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult PostVisitDirectory(DirectoryInfo dir, IOException exc)
		{			
			base.PostVisitDirectory(dir, exc);
			progressWorker.Task.PostVisitDirectory(Suggestions, dir);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFile(FileInfo file)
		{
			if(file.Name.Equals(R.strings.SKIP_DESKTOP_INI) ||
			        file.Name.Equals(R.strings.LOG_FILE_NAME))
				return FileVisitResult.Continue;
			
			base.VisitFile(file);
			
			FileVisit visit = new FileVisit(file);
			progressWorker.Publish(visit);
			
			progressWorker.Task.VisitFile(Suggestions, file);
			
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFileFailed(FileSystemInfo file, IOException exc)
		{			
			base.VisitFileFailed(file, exc);
			progressWorker.Task.VisitFileFailed(Suggestions, file, exc);
			
			return FileVisitResult.Continue;
		}
	}
}
