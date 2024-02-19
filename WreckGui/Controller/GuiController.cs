
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Java.Beans;
using Javax.Swing;
using log4net;
using Wreck.Entity;
using Wreck.IO;
using Wreck.IO.Task;
using Wreck.Model;
using Wreck.Resources;
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
		
		public override void Error()
		{
			MessageBox.Show(
				StartPath + " is invalid.",
				"Invalid path",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
			Application.Exit();
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
				}
				else if (R.Strings.PropertyProgress.Equals(evt.PropertyName))
				{
					int progress = (int)evt.NewValue;
					LOG.InfoFormat("Progress: {0}% MessageLoop: {1}", progress, Application.MessageLoop);
					controller.View.GetMain().SetProgress(progress);
				}
				else if (R.Strings.PropertyVisits.Equals(evt.PropertyName))
				{
					FileVisit visit = (FileVisit) evt.NewValue;
					LOG.InfoFormat("Progress: {0}% - Visit: {1} MessageLoop: {2}", visit.Progress, visit.File.Name, Application.MessageLoop);
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
			
//			View.GetScanningDialog().Close();
//			Worker = null;
		}
	}
}
