
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using JShim.NIO.File;
using JShim.Swing;
using log4net;
using Wreck.Entity;
using Wreck.IO.Task;
using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.IO
{
	/// <summary>
	/// Description of ProgressWorker.
	/// </summary>
	public class ProgressWorker : SwingWorker<SwingWorkerResult, FileVisit>
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(ProgressWorker));
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;

		private readonly ITask task;
		private readonly FileCountVisitor countVisitor;
		private readonly FileVisitor visitor;
		private readonly FileSystemInfo startPath;
		
		private long total;
		private int count;
		
		private FileVisit visit;
		private FileBean fileBean;
		
		public ProgressWorker(ITask task, FileSystemInfo startPath)
		{
			this.total = 0;
			this.count = 0;
			this.task = task;
			this.countVisitor = new FileCountVisitor(this);
			this.visitor = new FileVisitor(this);
			this.startPath = startPath;
		}
		
		private ITask Task { get { return task; } }
		private FileCountVisitor CountVisitor { get { return countVisitor; } }
		private FileVisitor Visitor { get { return visitor; } }
		private FileSystemInfo StartPath { get { return startPath; } }
		private long Total
		{
			get { return total; }
			set { total = value; }
		}
		
		private void IncrementTotal()
		{
			Total++;
		}
		
		private int Count { set { count = value; } }
		
		private void IncrementProgress()
		{
			count++;
		}
		
		protected void SetFileVisit(FileVisit visit)
		{
			FileVisit old = this.visit;
			this.visit = visit;
			FirePropertyChange(R.strings.PROPERTY_VISITS, old, this.visit);
		}
		
		protected void SetFileBean(FileBean fileBean)
		{
			FileBean old = this.fileBean;
			this.fileBean = fileBean;
			FirePropertyChange(R.strings.PROPERTY_BEAN, old, this.fileBean);
		}
		
		private void UpdateFileList(FileSystemInfo file, IDictionary<CorrectionEnum, DateTime> suggestions)
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
			
			FileBean update = new FileBean(
				file,
				file.CreationTime,
				file.LastWriteTime,
				metadata,
				diff);

			// LOG.trace("Updating: {}, {}", update.getPath(), update.getPeriod());
			SetFileBean(update);
		}
		
		private TimeSpan Diff(
			IDictionary<CorrectionEnum, DateTime> suggestions,
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
				DateTime start = metadata;
				DateTime end = current;
				diff = end.Date - start.Date;
			}
			
			return diff;
		}
		
		protected override SwingWorkerResult DoInBackground()
		{
			Total = 0;
			Count = 0;
			
			Files.WalkFileTree(
				StartPath,
				CountVisitor);

			LOG.InfoFormat("{0} total files detected.", Total);

			Files.WalkFileTree(
				StartPath,
				Visitor);
			
			// Return if background worker is cancelled by user.
			return IsCancelled()? 
				SwingWorkerResult.Cancelled : 
				SwingWorkerResult.Done;
		}
		
		protected override void Process(List<FileVisit> chunks)
		{
			chunks.ForEach(
				v =>
				{
					IncrementProgress();
					int progress = (int) (Convert.ToDouble(count) / Convert.ToDouble(total) * 100);
					// LOG.DebugFormat("visits = {0}, count = {1}, progress = {2}", visits.Length, count, progress);
					SetProgress(progress);
					v.Progress = progress;
					SetFileVisit(v);
				}
			);
		}
		
		protected override void Done()
		{
			try
			{
				SwingWorkerResult result = Get();
				switch(result)
				{
					case SwingWorkerResult.Done:
						break;
					case SwingWorkerResult.Cancelled:
						break;
					default:
						LOG.Error("Unknown result");
						throw new NotSupportedException("Unknown result");						
				}
			}
			catch (Exception e)
			{
				LOG.Error(e.ToString());
				LOG.Error(e.StackTrace);
				
				MessageBox.Show(
					e.ToString(),
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}
		
		class FileCountVisitor : SimpleFileVisitor
		{
			ProgressWorker progressWorker = null;
			
			public FileCountVisitor(ProgressWorker progressWorker)
			{
				this.progressWorker = progressWorker;
			}
			
			public override FileVisitResult PreVisitDirectory(DirectoryInfo dir)
			{
				progressWorker.IncrementTotal();
				return FileVisitResult.Continue;
			}
			
			public override FileVisitResult VisitFile(FileInfo file)
			{
				if(file.Name.Equals(R.strings.SKIP_DESKTOP_INI) ||
				   file.Name.Equals(R.strings.LOG_FILE_NAME))
					return FileVisitResult.Continue;
				
				progressWorker.IncrementTotal();
				return FileVisitResult.Continue;
			}

			public override FileVisitResult VisitFileFailed(FileSystemInfo file, IOException exc)
			{
				return FileVisitResult.Continue;
			}
		}
		
		class FileVisitor : SimpleFileVisitor
		{
			ProgressWorker progressWorker = null;
			
			private readonly List<DirectoryInfo> directories;
			private readonly Dictionary<CorrectionEnum, DateTime> suggestions;
			
			public FileVisitor(ProgressWorker progressWorker) : base()
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
	
	public enum SwingWorkerResult
	{
		Done,
		Cancelled
	}
}
