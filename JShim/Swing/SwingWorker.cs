
using System;
using System.Collections.Generic;
using JShim.Beans;
using JShim.Util.Concurrent;

namespace JShim.Swing
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class SwingWorker<T,V> where T : new()
	{
		/// <summary>
		/// Constructs this SwingWorker.
		/// </summary>
		public SwingWorker()
		{
		}
		
		/// <summary>
		/// Adds a PropertyChangeListener to the listener list. The listener is 
		/// registered for all properties. The same listener object may be added
		/// more than once, and will be called as many times as it is added. If 
		/// listener is null, no exception is thrown and no action is taken.
		/// Note: This is merely a convenience wrapper. All work is delegated to
		/// PropertyChangeSupport from <see cref="getPropertyChangeSupport()">
		/// getPropertyChangeSupport()</see>.
		/// </summary>
		/// <param name="listener"></param>
		public void AddPropertyChangeListener(PropertyChangeListener listener)
		{
			
		}
		
		/// <summary>
		/// Attempts to cancel execution of this task. This attempt will fail if
		/// the task has already completed, has already been cancelled, or could
		/// not be cancelled for some other reason. If successful, and this task
		/// has not started when cancel is called, this task should never run. 
		/// If the task has already started, then the mayInterruptIfRunning 
		/// parameter determines whether the thread executing this task should 
		/// be interrupted in an attempt to stop the task.
		/// 
		/// After this method returns, subsequent calls to Future.isDone() will
		/// always return true. Subsequent calls to Future.isCancelled() will 
		/// always return true if this method returned true.
		/// </summary>
		/// <param name="mayInterruptIfRunning">
		/// true if the thread executing this task should be interrupted; 
		/// otherwise, in-progress tasks are allowed to complete
		/// </param>
		/// <returns>
		/// false if the task could not be cancelled, typically because it has 
		/// already completed normally; true otherwise
		/// </returns>
		public bool Cancel(bool mayInterruptIfRunning)
		{
			return false;
		}
		
		/// <summary>
		/// Computes a result, or throws an exception if unable to do so.
		/// Note that this method is executed only once.
		/// Note: this method is executed in a background thread.
		/// </summary>
		/// <returns>the computed result</returns>
		protected abstract T DoInBackground();
		
		/// <summary>
		/// Executed on the Event Dispatch Thread after the doInBackground 
		/// method is finished. The default implementation does nothing. 
		/// Subclasses may override this method to perform completion actions on
		/// the Event Dispatch Thread. Note that you can query status inside the
		/// implementation of this method to determine the result of this task 
		/// or whether this task has been cancelled.
		/// </summary>
		/// <seealso cref="#DoInBackground()">DoInBackground()</seealso>
		/// <seealso cref="#IsCancelled()">IsCancelled()</seealso>
		/// <seealso cref="#Get()">Get()</seealso>
		protected virtual void Done()
		{
			
		}
		
		/// <summary>
		/// Schedules this SwingWorker for execution on a worker thread. There 
		/// are a number of worker threads available. In the event all worker 
		/// threads are busy handling other SwingWorkers this SwingWorker is 
		/// placed in a waiting queue.
		/// 
		/// Note: SwingWorker is only designed to be executed once. Executing a 
		/// SwingWorker more than once will not result in invoking the 
		/// doInBackground method twice.
		/// </summary>
		public void Execute()
		{
			
		}
		
		/// <summary>
		/// Reports a bound property update to any registered listeners. No 
		/// event is fired if old and new are equal and non-null.
		/// 
		/// This SwingWorker will be the source for any generated events.
		/// 
		/// When called off the Event Dispatch Thread PropertyChangeListeners 
		/// are notified asynchronously on the Event Dispatch Thread.
		///
		/// Note: This is merely a convenience wrapper. All work is delegated to
		/// PropertyChangeSupport from <see cref="getPropertyChangeSupport()">
		/// getPropertyChangeSupport()</see>.
		/// 
		/// </summary>
		/// <param name="propertyName">the programmatic name of the property that was changed</param>
		/// <param name="oldValue">the old value of the property</param>
		/// <param name="newValue">the new value of the property</param>
		protected void FirePropertyChange(string propertyName, object oldValue, object newValue)
		{
			
		}
		
		/// <summary>
		/// Waits if necessary for the computation to complete, and then 
		/// retrieves its result.
		/// 
		/// Note: calling get on the Event Dispatch Thread blocks all events, 
		/// including repaints, from being processed until this SwingWorker is 
		/// complete.
		/// 
		/// When you want the SwingWorker to block on the Event Dispatch Thread
		/// we recommend that you use a modal dialog.
		/// </summary>
		/// <returns>the computed result</returns>
		public T Get()
		{
			return new T();
		}
		
		/// <summary>
		/// Waits if necessary for at most the given time for the computation to
		/// complete, and then retrieves its result, if available.
		/// Please refer to <see cref="get()">get()</see> for more details.
		/// </summary>
		/// <param name="timeout">the maximum time to wait</param>
		/// <param name="unit">the time unit of the timeout argument</param>
		/// <returns>the computed result</returns>
		public T Get(long timeout, TimeUnit unit)
		{
			return new T();
		}
		
		/// <summary>
		/// Returns the progress bound property.
		/// </summary>
		/// <returns>the progress bound property.</returns>
		public int GetProgress()
		{
			return -1;
		}
		
		/// <summary>
		/// Returns the PropertyChangeSupport for this SwingWorker. This method 
		/// is used when flexible access to bound properties support is needed.
		/// 
		/// This SwingWorker will be the source for any generated events.
		/// 
		/// Note: The returned PropertyChangeSupport notifies any 
		/// PropertyChangeListeners asynchronously on the Event Dispatch Thread 
		/// in the event that firePropertyChange or fireIndexedPropertyChange 
		/// are called off the Event Dispatch Thread.
		/// </summary>
		/// <returns>PropertyChangeSupport for this SwingWorker</returns>
		public PropertyChangeSupport GetPropertyChangeSupport()
		{
			return null;
		}
		
		/// <summary>
		/// Returns the SwingWorker state bound property.
		/// </summary>
		/// <returns>the current state</returns>
		public SwingWorker<T,V>.StateValue GetState()
		{
			return StateValue.Pending;
		}
		
		/// <summary>
		/// Returns true if this task was cancelled before it completed 
		/// normally.
		/// </summary>
		/// <returns>
		/// true if this task was cancelled before it completed
		/// </returns>
		public bool IsCancelled()
		{
			return false;
		}
		
		/// <summary>
		/// Returns true if this task completed. Completion may be due to normal
		/// termination, an exception, or cancellation -- in all of these cases,
		/// this method will return true.
		/// </summary>
		/// <returns>true if this task completed</returns>
		public bool IsDone()
		{
			return false;
		}
		
		/// <summary>
		/// Receives data chunks from the publish method asynchronously on the 
		/// Event Dispatch Thread.
		/// Please refer to the publish(V...) method for more details.
		/// </summary>
		/// <param name="chunks">intermediate results to process</param>
		protected virtual void Process(List<V> chunks)
		{
			
		}
		
		/// <summary>
		/// Sends data chunks to the process(java.util.List<V>) method. This 
		/// method is to be used from inside the doInBackground method to 
		/// deliver intermediate results for processing on the Event Dispatch 
		/// Thread inside the process method.
		/// 
		/// Because the process method is invoked asynchronously on the Event 
		/// Dispatch Thread multiple invocations to the publish method might 
		/// occur before the process method is executed. For performance 
		/// purposes all these invocations are coalesced into one invocation 
		/// with concatenated arguments.
		/// </summary>
		/// <param name="chunks">intermediate results to process</param>
		protected void Publish(params V[] chunks)
		{
			
		}
		
		/// <summary>
		/// Removes a PropertyChangeListener from the listener list. This 
		/// removes a PropertyChangeListener that was registered for all 
		/// properties. If listener was added more than once to the same event 
		/// source, it will be notified one less time after being removed. If 
		/// listener is null, or was never added, no exception is thrown and no 
		/// action is taken.
		/// Note: This is merely a convenience wrapper. All work is delegated to
		/// PropertyChangeSupport from <see cref="getPropertyChangeSupport()">
		/// getPropertyChangeSupport()</see>.
		/// </summary>
		/// <param name="listener">the PropertyChangeListener to be removed</param>
		public void RemovePropertyChangeListener(PropertyChangeListener listener)
		{
			
		}
		
		/// <summary>
		/// Sets this Future to the result of computation unless it has been cancelled.
		/// </summary>
		public void Run()
		{
			
		}
		
		/// <summary>
		/// Sets the progress bound property. The value should be from 0 to 100.
		/// Because PropertyChangeListeners are notified asynchronously on the 
		/// Event Dispatch Thread multiple invocations to the setProgress method
		/// might occur before any PropertyChangeListeners are invoked. For 
		/// performance purposes all these invocations are coalesced into one 
		/// invocation with the last invocation argument only.
		/// </summary>
		/// <param name="progress">the progress value to set</param>
		protected void SetProgress(int progress)
		{
			
		}
		
		public enum StateValue
		{
			Done,
			Pending,
			Started
		}
	}
}
