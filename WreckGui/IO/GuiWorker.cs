
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Java.NIO.File;
using Javax.Swing;
using log4net;
using Wreck.Entity;
using Wreck.IO.Task;
using Wreck.Resources;

namespace Wreck.IO
{
	/// <summary>
	/// Description of GuiWorker.
	/// </summary>
	public class GuiWorker : SwingWorker<string, FileVisit>
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(GuiWorker));
		
		private readonly List<ITask> tasks;
		private readonly string[] startPaths;
		
		private long total;
		private int count;
		
		private FileVisit visit;
		private FileBean fileBean;
		
		public GuiWorker(List<ITask> tasks, string[] startPaths)
		{
			this.total = 0;
			this.count = 0;
			this.tasks = tasks;
			this.startPaths = startPaths;
		}
		
		public List<ITask> Tasks { get { return tasks; } }
		private string[] StartPaths { get { return startPaths; } }
		
		private long Total
		{
			get { return total; }
			set { total = value; }
		}
		
		public void IncrementTotal()
		{
			Total++;
		}
		
		private int Count { set { count = value; } }
		
		public void IncrementProgress()
		{
			count++;
		}
		
		protected override string DoInBackground()
		{
			Total = 0;
			Count = 0;
			
			Debug.Assert(StartPaths.Length == Tasks.Count);
			
			int len = Math.Min(StartPaths.Length, Tasks.Count);
			
			for(int i = 0; i < len; i++)
			{
				string p = StartPaths[i];
				FileSystemInfo startPath;
				
				if(Directory.Exists(p))
					startPath = new DirectoryInfo(p);
				else if(File.Exists(p))
					startPath = new FileInfo(p);
				else
					throw new IOException("Unsupported file type: " + p);
				
				ITask task = Tasks[i];
				
				GuiCountVisitor countVisitor = new GuiCountVisitor(this);
				
				Files.WalkFileTree(
					startPath,
					countVisitor);

				LOG.InfoFormat("{0} total files detected.", Total);
				
				
				GuiVisitor visitor = new GuiVisitor(this, task);

				Files.WalkFileTree(
					startPath,
					visitor);
			}
			
			// Return if background worker is cancelled by user.
			return IsCancelled()? "Cancelled" : "Done";
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
				string result = Get();
				LOG.DebugFormat("Done: {0}", result);
				base.Done();
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
		
		protected void SetFileVisit(FileVisit visit)
		{
			FileVisit old = this.visit;
			this.visit = visit;
			FirePropertyChange(R.Strings.PropertyVisits, old, this.visit);
		}
		
		protected void SetFileBean(FileBean fileBean)
		{
			FileBean old = this.fileBean;
			this.fileBean = fileBean;
			FirePropertyChange(R.Strings.PropertyBean, old, this.fileBean);
		}
		
		public void UpdateFileList(FileSystemInfo file, IDictionary<CorrectionEnum, DateTime> suggestions)
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
	}
}
