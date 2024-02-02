﻿
using System;
using System.Collections.Generic;
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
		
		private readonly ITask task;
		private readonly FileSystemInfo startPath;
		
		private readonly GuiCountVisitor countVisitor;
		private readonly GuiVisitor visitor;
		
		private long total;
		private int count;
		
		private FileVisit visit;
		private FileBean fileBean;
		
		public GuiWorker(ITask task, FileSystemInfo startPath)
		{
			this.total = 0;
			this.count = 0;
			this.task = task;
			this.startPath = startPath;
			this.countVisitor = new GuiCountVisitor(this);
			this.visitor = new GuiVisitor(this);
		}
		
		public ITask Task { get { return task; } }
		private FileSystemInfo StartPath { get { return startPath; } }
		private GuiCountVisitor CountVisitor { get { return countVisitor; } }
		private GuiVisitor Visitor { get { return visitor; } }
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
			
			Files.WalkFileTree(
				StartPath,
				CountVisitor);

			LOG.InfoFormat("{0} total files detected.", Total);

			Files.WalkFileTree(
				StartPath,
				Visitor);
			
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
			FirePropertyChange(R.strings.PROPERTY_VISITS, old, this.visit);
		}
		
		protected void SetFileBean(FileBean fileBean)
		{
			FileBean old = this.fileBean;
			this.fileBean = fileBean;
			FirePropertyChange(R.strings.PROPERTY_BEAN, old, this.fileBean);
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