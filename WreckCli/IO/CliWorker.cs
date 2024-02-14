
using System;
using System.Collections.Generic;
using System.IO;

using Java.Beans;
using Java.NIO.File;
using Java.Util.Concurrent;
using log4net;
using Wreck.Entity;
using Wreck.IO.Task;
using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.IO
{
	/// <summary>
	/// Description of CliWorker.
	/// </summary>
	public class CliWorker : RunnableFuture<string>
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(CliWorker));
		
		protected bool cancelled;
		protected int progress;
		protected string result;
		protected StateValue state;
		
		private readonly PropertyChangeSupport propertyChangeSupport;
		
		private readonly ITask task;
		private readonly FileSystemInfo startPath;
		
		private readonly CliVisitor visitor;
		
		private FileVisit visit;
//		private FileBean fileBean;
		
		public CliWorker(ITask task, FileSystemInfo startPath)
		{
			state = StateValue.Pending;
			propertyChangeSupport = new PropertyChangeSupport(this);
			
			this.task = task;
			this.startPath = startPath;
			this.visitor = new CliVisitor(this);
		}
		
		public ITask Task { get { return task; } }
		private FileSystemInfo StartPath { get { return startPath; } }
		private AbstractFileVisitor Visitor { get { return visitor; } }
		
		public void Execute()
		{
			Run(); // Assume in a background thread.
			Done();
		}
		
		public void Run()
		{
			if(!cancelled)
			{
				SetState(StateValue.Started);
				result = DoInBackground();
			}
		}
		
		protected string DoInBackground()
		{
			Files.WalkFileTree(
				StartPath,
				Visitor);
			
			// Return if background worker is cancelled by user.
			return IsCancelled()? "Cancelled" : "Done";
		}
		
		protected void Process(List<FileVisit> chunks)
		{
			chunks.ForEach(
				v =>
				{
					SetFileVisit(v);
				}
			);
		}
		
		protected void Done()
		{
			try
			{
				SetState(StateValue.Done);
				string result = Get();
				LOG.DebugFormat("Done: {0}", result);
			}
			catch (Exception e)
			{
				LOG.Error(e.ToString());
				LOG.Error(e.StackTrace);
			}
		}
		
		protected void SetFileVisit(FileVisit visit)
		{
			FileVisit old = this.visit;
			this.visit = visit;
			FirePropertyChange(R.Strings.PropertyVisits, old, this.visit);
		}
		
		public void AddPropertyChangeListener(PropertyChangeListener listener)
		{
			GetPropertyChangeSupport().AddPropertyChangeListener(listener);
		}
		
		public void RemovePropertyChangeListener(PropertyChangeListener listener)
		{
			GetPropertyChangeSupport().RemovePropertyChangeListener(listener);
		}
		
		public bool Cancel(bool mayInterruptIfRunning)
		{
			if(!IsCancelled() && !IsDone() && mayInterruptIfRunning)
			{
				cancelled = true;
				return true;
			}
			else
				return false;
		}
		
		protected void FirePropertyChange(string propertyName, object oldValue,
		                                  object newValue)
		{
//			LOG.DebugFormat("FirePropertyChange: {0}, {1}, {2}",
//			                propertyName, oldValue, newValue);
			GetPropertyChangeSupport().FirePropertyChange(
				propertyName,
				oldValue, newValue);
		}
		
		public string Get()
		{
			return result;
		}
		
		public string Get(long timeout, TimeUnit unit)
		{
			return Get();
		}
		
		public PropertyChangeSupport GetPropertyChangeSupport()
		{
			return propertyChangeSupport;
		}
		
		public CliWorker.StateValue GetState()
		{
			if (IsDone())
			{
				return StateValue.Done;
			}
			else
			{
				return state;
			}
		}
		
		private void SetState(StateValue state)
		{
			StateValue old = this.state;
			this.state = state;
			FirePropertyChange("state", old, state);
		}
		
		public bool IsCancelled()
		{
			return cancelled;
		}
		
		public bool IsDone()
		{
			return state == StateValue.Done;
		}
		
		/*
		protected void Process(List<FileVisit> chunks)
		{
			chunks.ForEach(
				chunk =>
				{
					log.DebugFormat("Process: {0}", chunk.ToString());
				}
			);
		}
		*/
		
		public void Publish(params FileVisit[] chunks)
		{
			List<FileVisit> visits = new List<FileVisit>(chunks);
			Process(visits);
		}
		
		public enum StateValue
		{
			Done,
			Pending,
			Started
		}
	}
}
