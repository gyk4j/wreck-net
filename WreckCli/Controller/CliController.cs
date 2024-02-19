
using System;
using System.Collections.Generic;
using System.IO;

using Java.Beans;
using log4net;
using Wreck.Entity;
using Wreck.IO;
using Wreck.IO.Task;
using Wreck.Model;
using Wreck.Resources;
using Wreck.View;

namespace Wreck.Controller
{
	/// <summary>
	/// Description of CliController.
	/// </summary>
	public class CliController : AbstractController
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(CliController));
		
		private CliWorker worker;
		
		private readonly ConsoleModel model;
		private readonly ConsoleView view;
		
		public CliController(ConsoleView view) : base()
		{
			this.model = new ConsoleModel();
			this.view = view;
		}
		
		public ConsoleModel Model
		{
			get { return model; }
		}
		
		public ConsoleView View
		{
			get { return view; }
		}
		
		public CliWorker Worker
		{
			get { return worker; }
			set { worker = value; }
		}
		
		public override void Error()
		{
			LOG.Error(StartPath + " is invalid.");
			Environment.Exit(-1);
		}
		
		public override void Run(CorrectionMode mode, FileSystemInfo fsi)
		{
			Dictionary<SourceEnum, bool> sources = new Dictionary<SourceEnum, bool>();
			foreach(SourceEnum s in SourceEnum.Values)
			{
				// FIXME: To check command line parameters
				sources.Add(s, true);
			}
			
			Dictionary<CorrectionEnum, bool> corrections = new Dictionary<CorrectionEnum, bool>();
			foreach(CorrectionEnum c in CorrectionEnum.Values)
			{
				// FIXME: To check command line parameters
				corrections.Add(c, true);
			}
			
			// FIXME: To check command line parameters
			DateTime customDateTime = DateTime.Now;
			
			ITask task = Service.Run(fsi, mode, sources, corrections, customDateTime);
			
			PropertyChangeListener propertyChangeListener = new ProgressPropertyChangeListener(this);
			
			CliWorker pw = new CliWorker(task, fsi);
			pw.AddPropertyChangeListener(propertyChangeListener);
			
			// HACK: Worker must save the reference before Execute as this is single-threaded. 
			// Saving the Worker after Execute is useless as 
			// PropertyChangeListener PropertyChange() will be triggered to do 
			// cleanup immediately within Execute before the reference to the 
			// worker is saved, which will cause controller.Done() call to be 
			// skipped.
			Worker = pw; // Need to save to Worker for cleanup later.
			pw.Execute();
		}
		
		private class ProgressPropertyChangeListener : PropertyChangeListener
		{
			CliController controller;
			public ProgressPropertyChangeListener(CliController controller)
			{
				this.controller = controller;
			}
			
			public void PropertyChange(PropertyChangeEvent evt)
			{
				if(R.Strings.PropertyState.Equals(evt.PropertyName))
				{
					CliWorker.StateValue state = (CliWorker.StateValue) evt.NewValue;
					LOG.InfoFormat(
						"State = {0}",
						Enum.GetName(typeof(CliWorker.StateValue), state));
					
					if(CliWorker.StateValue.Done.Equals(state))
					{
						if(controller.Worker != null && controller.Worker.IsDone())
							controller.Done();
					}
				}
				else if (R.Strings.PropertyProgress.Equals(evt.PropertyName))
				{
					int progress = (int)evt.NewValue;
					LOG.InfoFormat("{0}%", progress);
				}
				else if (R.Strings.PropertyVisits.Equals(evt.PropertyName))
				{
					FileVisit visit = (FileVisit) evt.NewValue;
					
					LOG.InfoFormat("{0} - {1}%", visit.File.FullName, visit.Progress);
				}
				else if(R.Strings.PropertyBean.Equals(evt.PropertyName))
				{
					FileBean update = (FileBean) evt.NewValue;
					LOG.InfoFormat("{0}, {1}, {2}, {3}, {4}",
					               update.Creation.ToString(),
					               update.Modified.ToString(),
					               update.Metadata.ToString(),
					               update.Period.ToString(),
					               update.Path.FullName);
				}
			}
		}
	}
}
