
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Java.Beans;
using Javax.Swing;
using log4net;
using Wreck.Entity;
using Wreck.IO;
using Wreck.IO.Reader;
using Wreck.IO.Task;
using Wreck.IO.Writer;
using Wreck.Resources;
using Wreck.Service;
using Wreck.Util.Logging;
using WreckGui.Model;

namespace Wreck.Controller
{
	/// <summary>
	/// Description of GuiController.
	/// </summary>
	public class GuiController : IController
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(GuiController));
		
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		private FileSystemInfo startPath;
		private ProgressWorker worker;
		
		private readonly GuiModel model;
		private readonly MainForm view;
		private readonly PreviewService service;
		
		protected static GuiController instance;
		
		public GuiController(MainForm view)
		{
			this.model = new GuiModel();
			this.view = view;
			this.service = new PreviewService();
			instance = this;
		}
		
		public static IController Instance
		{
			get
			{
				if (instance == null)
					throw new NullReferenceException("GuiController instance is uninitialized");
				return instance;
			}
		}
		
		public GuiModel Model
		{
			get { return model; }
		}
		
		public MainForm View
		{
			get { return view; }
		}
		
		public FileSystemInfo StartPath
		{
			get { return startPath; }
		}
		
		public ProgressWorker Worker
		{
			get { return worker; }
			set { worker = value; }
		}
		
		public void Start(string startPath)
		{
			if(Directory.Exists(startPath))
				this.startPath =  new DirectoryInfo(startPath);
			else if(File.Exists(startPath))
				this.startPath = new FileInfo(startPath);
			else
			{
				MessageBox.Show(
					startPath + " is invalid.",
					"Invalid path",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				Application.Exit();
			}
			
			if(this.startPath != null)
			{
				Init();
//				View.GetPreviewDialog().Open();
			}
		}
		
		public void Stop()
		{
			LOG.Info("Stopping and cleaning up...");
			try
			{
				if(WriterFactory.IsInitialized())
				{
					LOG.Info("Closing writers...");
					WriterFactory.Instance.Dispose();
				}
				
				if(ReaderFactory.IsInitialized())
				{
					LOG.Info("Closing readers...");
					ReaderFactory.Instance.Dispose();
				}
			}
			catch (Exception e)
			{
				LOG.Error(e.ToString());
			}
		}
		
		private void Init()
		{
			
		}
		
		public void Version()
		{
			view.Version();
		}
		
		public void UnknownPathType(string path)
		{
			view.UnknownPathType(path);
		}
		
		public void CurrentPath(string p)
		{
			// TODO: Remove
//			if(view.BackgroundWorker != null)
//				view.BackgroundWorker.ReportProgress(0, p);
		}
		
		public void CurrentFile(FileInfo f)
		{
			// TODO: Remove
//			if(view.BackgroundWorker != null)
//				view.BackgroundWorker.ReportProgress(0, f);
		}
		
		public void CurrentDirectory(DirectoryInfo d)
		{
			// TODO: Remove
//			if(view.BackgroundWorker != null)
//				view.BackgroundWorker.ReportProgress(0, d);
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
			view.SkipReparsePoint(d);
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
			view.SkipReparsePoint(f);
		}
		
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
			view.CorrectedByLastWriteMetadata(fsi, lastWrite);
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
			view.CorrectedByCreationMetadata(fsi, creation);
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
			view.CorrectedByLastAccessMetadata(fsi, lastAccess);
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
			view.CorrectedByLastWriteTime(fsi, creationOrLastAccess);
		}
		
		public void Statistics(Statistics stats)
		{
			// TODO: Remove
//			if(view.BackgroundWorker != null)
//				view.BackgroundWorker.ReportProgress(0, stats);
		}
		
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
			view.UnauthorizedAccessException(ex);
		}
		
		// Event Handlers
		
		public void Analyze()
		{
			Run(CorrectionMode.Analyze);
		}
		
		public void Backup()
		{
			Run(CorrectionMode.BackupAttributes);
		}
		
		public void Repair()
		{
			Run(CorrectionMode.SaveAttributes);
		}
		
		public void Restore()
		{
			Run(CorrectionMode.RestoreAttributes);
		}
		
		public void Verify()
		{
			Run(CorrectionMode.VerifyAttributes);
		}

		private void Run(CorrectionMode mode)
		{
			string[] args = Environment.GetCommandLineArgs();
			string[] dirs;
			
			if(args.Length > 1)
			{
				dirs = new string[args.Length-1];
				Array.Copy(args, 1, dirs, 0, args.Length-1);
			}
			else
			{
				dirs = new string[]
				{
					Directory.GetCurrentDirectory()
				};
			}
			
			foreach(string d in dirs)
			{
				FileSystemInfo fsi;
				
				if(Directory.Exists(d))
					fsi = new DirectoryInfo(d);
				else if(File.Exists(d))
					fsi = new FileInfo(d);
				else
				{
					view.UnknownPathType(d);
					continue;
				}
				
				// FIXME: More like MakeTask or CreateTask
				Dictionary<SourceEnum, bool> sources = new Dictionary<SourceEnum, bool>();
				foreach(SourceEnum s in SourceEnum.Values)
				{
					// FIXME: To check GUI control checkbox
					sources.Add(s, true);
				}
				
				Dictionary<CorrectionEnum, bool> corrections = new Dictionary<CorrectionEnum, bool>();
				foreach(CorrectionEnum c in CorrectionEnum.Values)
				{
					// FIXME: To check GUI control checkbox
					corrections.Add(c, true);
				}
				
				DateTime customDateTime = DateTime.Now;
				
				ITask task = service.Run(fsi, mode, sources, corrections, customDateTime);
				
				PropertyChangeListener propertyChangeListener = new ProgressPropertyChangeListener(this);
				
				ProgressWorker pw = new ProgressWorker(task, fsi);
				pw.AddPropertyChangeListener(propertyChangeListener);
				pw.Execute();
			}
		}
		
		private class ProgressPropertyChangeListener : PropertyChangeListener
		{
			GuiController controller;
			public ProgressPropertyChangeListener(GuiController controller)
			{
				this.controller = controller;
			}
			
			public void PropertyChange(PropertyChangeEvent evt)
			{
				if(R.strings.PROPERTY_STATE.Equals(evt.PropertyName))
				{
					SwingWorker<int, FileVisit>.StateValue state = (SwingWorker<int, FileVisit>.StateValue) evt.NewValue;
					if(SwingWorker<int, FileVisit>.StateValue.Done.Equals(state))
					{
						if(controller.Worker != null && controller.Worker.IsDone())
							controller.Done();
					}
				}
				else if (R.strings.PROPERTY_PROGRESS.Equals(evt.PropertyName))
				{
					int progress = (int)evt.NewValue;
//					Model.getScanningProgressModel().setValue(progress);
				}
				else if (R.strings.PROPERTY_VISITS.Equals(evt.PropertyName))
				{
					FileVisit visit = (FileVisit) evt.NewValue;
					
//					View.getScanningDialog().setProgress(visit.getProgress());
//					View.getScanningDialog().getAction().setText(visit.getFile().getFileName().toString());
				}
				else if(R.strings.PROPERTY_BEAN.Equals(evt.PropertyName))
				{
					FileBean update = (FileBean) evt.NewValue;
//					Model.getTableModel().addRow(update);
				}
			}
		}
		
		public bool CancelScanning()
		{
			bool cancelled = false;
			if(Worker != null && !Worker.IsCancelled() && !Worker.IsDone())
			{
				Worker.Cancel(false);
				cancelled = true;
			}
			return cancelled;
		}
		
		// Update statistics
		
		private void UpdateStatistics()
		{
			
		}
		
		private void UpdateForecastStatistics()
		{
			
		}
		
		private void UpdateRestoreState()
		{
			
		}
		
		public void Done()
		{
			if(Worker.IsDone())
			{
//				View.GetScanningDialog().Close();
				
				Stop();
				Worker = null;
				UpdateRestoreState();
			}
			
			UpdateStatistics();
			UpdateForecastStatistics();

			/*
			LOG.InfoFormat("Metadata: {0}, Last Modified: {1}, Custom: {2}",
				STATS.get(SelectionEvent.METADATA),
				STATS.get(SelectionEvent.LAST_MODIFIED),
				STATS.get(SelectionEvent.CUSTOM));
			 */
		}
	}
}
