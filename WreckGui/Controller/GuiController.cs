
using System;
using System.Collections.Generic;
using System.IO;

using Java.Beans;
using Javax.Swing;
using log4net;
using Wreck.Entity;
using Wreck.IO;
using Wreck.IO.Reader;
using Wreck.IO.Task;
using Wreck.IO.Writer;
using Wreck.Model;
using Wreck.Resources;
using Wreck.Time;
using Wreck.Util.Logging;
using WreckGui.View;

namespace Wreck.Controller
{
	/// <summary>
	/// Description of GuiController.
	/// </summary>
	public class GuiController : AbstractController
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(GuiController));
		
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		private GuiWorker worker;
		
		private readonly GuiModel model;
		private readonly GuiView view;
		
		public GuiController(GuiModel model, GuiView view) : base()
		{
			this.model = model;
			this.view = view;
			PrefillPaths();
			Init();
		}
		
		public GuiModel Model
		{
			get { return model; }
		}
		
		public GuiView View
		{
			get { return view; }
		}
		
		public GuiWorker Worker
		{
			get { return worker; }
			set { worker = value; }
		}
		
		/*
		public override void Start(string startPath)
		{
			try
			{
				this.startPath = TimestampFormatter.IsValidPath(startPath);
				Init();
				View.GetMain().Show();
			}
			catch(Exception e)
			{
				LOG.ErrorFormat("Invalid path: {0}", e.Message);
				Application.Exit();
			}
		}
		
		public override void Stop()
		{
			LOG.Info("Stopping and cleaning up...");
			try
			{
				if(WriterFactory.IsInitialized())
				{
					LOG.Info("Closing writers...");
					WriterFactory.Instance.Dispose();
				}
				
				if(ReaderFactory.IsInitialized()) {
					LOG.Info("Closing readers...");
					ReaderFactory.Instance.Dispose();
				}
			}
			catch (Exception e)
			{
				LOG.Error(e.StackTrace);
			}
		}
		*/
		
		protected override void Init()
		{
//			base.Init();
			
			foreach(SourceEnum s in SourceEnum.Values)
			{
				string member = Model.GetBindingDataMember(s);
				View.GetMain().Bind(Model, s, member);
			}
			
			foreach(CorrectionEnum c in CorrectionEnum.Values)
			{
				string member = Model.GetBindingDataMember(c);
				View.GetMain().Bind(Model, c, member);
			}
		}
		
		public override void Error()
		{
			View.GetMain().Error(StartPath.FullName);
		}
		
		private void PrefillPaths()
		{
			string[] args = Environment.GetCommandLineArgs();
			string[] paths;
			
			// First arg is always null/empty.
			if(args.Length > 1)
			{
				paths = new string[args.Length-1];
				Array.Copy(args, 1, paths, 0, args.Length-1);
			}
			else
			{
				paths = new string[0];
				Error();
			}
			
			foreach(string p in paths)
			{
				if(Directory.Exists(p) || File.Exists(p))
					View.GetMain().AddPath(p);
				else
					LOG.ErrorFormat("Unknown path type: {0}", p);
			}
		}

		public override void Run(CorrectionMode mode, FileSystemInfo fsi)
		{
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
			
			// FIXME: To check GUI control checkbox and textbox
			DateTime customDateTime = DateTime.Now;
			
			ITask task = Service.Run(fsi, mode, sources, corrections, customDateTime);
			
			PropertyChangeListener propertyChangeListener = new ProgressPropertyChangeListener(this);
			
			GuiWorker pw = new GuiWorker(task, fsi);
			pw.AddPropertyChangeListener(propertyChangeListener);
			pw.Execute();
			Worker = pw; // Need to save to Worker for cleanup later.
		}
		
		private class ProgressPropertyChangeListener : PropertyChangeListener
		{
			private static readonly ILog LOG = LogManager.GetLogger(typeof(GuiController.ProgressPropertyChangeListener));
			GuiController controller;
			public ProgressPropertyChangeListener(GuiController controller)
			{
				this.controller = controller;
			}
			
			public void PropertyChange(PropertyChangeEvent evt)
			{
				if(R.Strings.PropertyState.Equals(evt.PropertyName))
				{
					SwingWorker<int, FileVisit>.StateValue state = (SwingWorker<int, FileVisit>.StateValue) evt.NewValue;
					if(SwingWorker<int, FileVisit>.StateValue.Done.Equals(state))
					{
						if(controller.Worker != null && controller.Worker.IsDone())
							controller.Done();
					}
					
					bool running = SwingWorker<int, FileVisit>.StateValue.Started.Equals(state);
					controller.View.GetMain().SetAppState(running);
				}
				else if (R.Strings.PropertyProgress.Equals(evt.PropertyName))
				{
					int progress = (int)evt.NewValue;
					LOG.InfoFormat("Progress: {0}%", progress);
					controller.View.GetMain().SetProgress(progress);
				}
				else if (R.Strings.PropertyVisits.Equals(evt.PropertyName))
				{
					FileVisit visit = (FileVisit) evt.NewValue;
					LOG.InfoFormat("Progress: {0}% - Visit: {1}", visit.Progress, visit.File.Name);
					controller.View.GetMain().SetProgress(visit.Progress);
					controller.View.GetMain().SetAction(visit);
				}
				else if(R.Strings.PropertyBean.Equals(evt.PropertyName))
				{
					FileBean update = (FileBean) evt.NewValue;
					LOG.InfoFormat("FileBean: {0} {1} {2} {3} {4}",
					               update.Path.Name,
					               update.Creation.ToString(),
					               update.Modified.ToString(),
					               update.Metadata.ToString(),
					               update.Period.ToString());
//					Model.GetTableModel().AddRow(update);
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
		
		private void UpdateForecastStatistics()
		{
			// No charts to update.
		}
		
		private void UpdateRestoreState()
		{
			// No "Restore" button to enable or disable.
		}
		
		public override void Done()
		{
			base.Done();
			UpdateForecastStatistics();
			UpdateRestoreState();
			
			View.GetMain().Done();
//			Worker = null;
		}
		
		protected override void UpdateStatistics()
		{
			base.UpdateStatistics();
			Statistics stats = new Statistics();
			stats.Directories = STATS.Get(FileEvent.DirectoryFound);
			stats.Files = STATS.Get(FileEvent.FileFound);
			stats.Skipped = STATS.Get(FileEvent.FileError);
			View.GetMain().Statistics(stats);
			
			LOG.DebugFormat("Dir  Found: {0}", STATS.Get(FileEvent.DirectoryFound));
			LOG.DebugFormat("File Found: {0}", STATS.Get(FileEvent.FileFound));
			LOG.DebugFormat("File Error: {0}", STATS.Get(FileEvent.FileError));
			LOG.DebugFormat("Fixed Creation: {0}", STATS.Get(FileEvent.CorrectibleCreation));
			LOG.DebugFormat("Fixed Modified: {0}", STATS.Get(FileEvent.CorrectibleModified));
			LOG.DebugFormat("Fixed Accessed: {0}", STATS.Get(FileEvent.CorrectibleAccessed));
		}
	}
}
