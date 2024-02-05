﻿
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
using Wreck.Resources;
using Wreck.Util.Logging;
using WreckGui.Model;

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
		private readonly MainForm view;
		
		public GuiController(MainForm view) : base()
		{
			this.model = new GuiModel();
			this.view = view;
		}
		
		public GuiModel Model
		{
			get { return model; }
		}
		
		public MainForm View
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
//					Model.GetScanningProgressModel().SetValue(progress);
				}
				else if (R.strings.PROPERTY_VISITS.Equals(evt.PropertyName))
				{
					FileVisit visit = (FileVisit) evt.NewValue;
					
//					View.GetScanningDialog().SetProgress(visit.GetProgress());
//					View.GetScanningDialog().GetAction().SetText(visit.GetFile().GetFileName().ToString());
				}
				else if(R.strings.PROPERTY_BEAN.Equals(evt.PropertyName))
				{
					FileBean update = (FileBean) evt.NewValue;
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
