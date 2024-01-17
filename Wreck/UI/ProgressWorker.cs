
using System;
using System.Collections.Generic;
using System.IO;

using log4net;
using Wreck.Entity;
using Wreck.IO.Task;
using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.UI
{
	/// <summary>
	/// Description of ProgressWorker.
	/// </summary>
	public abstract class ProgressWorker
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(ProgressWorker));
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		protected const string DONE = "done";
		protected const string CANCELLED = "cancelled";
		protected const string ERROR = "error";
		
		private readonly ITask task;
//		private readonly FileCountVisitor countVisitor;
//		private readonly FileVisitor visitor;
		private readonly FileSystemInfo startPath;
		
		private long total;
		private int count;
		
		public ProgressWorker(ITask task, FileSystemInfo startPath)
		{
			this.total = 0;
			this.count = 0;
			this.task = task;
			this.startPath = startPath;
		}
		
		private ITask Task
		{
			get { return task; }
		}
		
//		private FileCountVisitor CountVisitor
//		{
//			get { return countVisitor; }
//		}
//
//		private FileVisitor Visitor
//		{
//			get { return visitor; }
//		}
		
		private FileSystemInfo StartPath
		{
			get { return startPath; }
		}

		private long Total
		{
			get { return total; }
			set { total = value; }
		}
		
		private void IncrementTotal()
		{
			Total = ++Total;
		}

		private int Count
		{
			get { return count; }
			set { count = value; }
		}
		
		private void IncrementProgress()
		{
			Count = ++Count;
		}
		
		protected abstract void SetFileVisit(FileVisit visit);
		protected abstract void SetFileEntity(FileEntity fileEntity);
		
		private void UpdateFileList(FileSystemInfo file, Dictionary<CorrectionEnum, DateTime> suggestions)
		{
			DateTime metadata = file.CreationTime;
			TimeSpan diff = Diff(suggestions, CorrectionEnum.CREATION, metadata);
			
			if(TimeSpan.Zero.Equals(diff))
			{
				metadata = file.LastWriteTime;
				diff = Diff(suggestions, CorrectionEnum.MODIFIED, metadata);
			}
			
			if(TimeSpan.Zero.Equals(diff))
			{
				metadata = file.LastAccessTime;
				diff = Diff(suggestions, CorrectionEnum.ACCESSED, metadata);
			}
			
			FileEntity update = new FileEntity(
				file,
				file.CreationTime,
				file.LastWriteTime,
				metadata,
				diff);

			LOG.DebugFormat("Updating: {0}, {1}", update.Path, update.Period);
			SetFileEntity(update);
		}
		
		private TimeSpan Diff(
			Dictionary<CorrectionEnum, DateTime> suggestions,
			CorrectionEnum attrib,
			DateTime current)
		{
			// Calculate the time drift between current file system attributes
			// and correct metadata timestamps to adjust back.
			TimeSpan diff = TimeSpan.Zero;
			DateTime metadata;
			
			bool ok = suggestions.TryGetValue(attrib, out metadata);

			if(ok)
			{
				DateTime start = metadata.Date;
				DateTime end = current.Date;
				diff = end.Subtract(start);
			}
			
			return diff;
		}
	}
}
