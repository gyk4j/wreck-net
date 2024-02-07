
using System;

namespace Java.Util.Concurrent
{
	/// <summary>
	/// A Future represents the result of an asynchronous computation. Methods 
	/// are provided to check if the computation is complete, to wait for its 
	/// completion, and to retrieve the result of the computation. The result 
	/// can only be retrieved using method get when the computation has 
	/// completed, blocking if necessary until it is ready. Cancellation is 
	/// performed by the cancel method. Additional methods are provided to 
	/// determine if the task completed normally or was cancelled. Once a 
	/// computation has completed, the computation cannot be cancelled. If you 
	/// would like to use a Future for the sake of cancellability but not 
	/// provide a usable result, you can declare types of the form Future<?> and
	/// return null as a result of the underlying task.
	/// </summary>
	public interface Future<V>
	{
		/// <summary>
		/// Attempts to cancel execution of this task. This attempt will fail if
		/// the task has already completed, has already been cancelled, or could
		/// not be cancelled for some other reason. If successful, and this task
		/// has not started when cancel is called, this task should never run. If
		/// the task has already started, then the mayInterruptIfRunning 
		/// parameter determines whether the thread executing this task should 
		/// be interrupted in an attempt to stop the task.
		/// 
		/// After this method returns, subsequent calls to isDone() will always 
		/// return true. Subsequent calls to isCancelled() will always return 
		/// true if this method returned true.
		/// </summary>
		/// <param name="mayInterruptIfRunning">
		/// true if the thread executing this task should be interrupted; 
		/// otherwise, in-progress tasks are allowed to complete
		/// </param>
		/// <returns>
		/// false if the task could not be cancelled, typically because it has 
		/// already completed normally; true otherwise
		/// </returns>
		bool Cancel(bool mayInterruptIfRunning);
		
		/// <summary>
		/// Waits if necessary for the computation to complete, and then 
		/// retrieves its result.
		/// </summary>
		/// <returns>
		/// the computed result
		/// </returns>
		V Get();
		
		/// <summary>
		/// Waits if necessary for at most the given time for the computation to
		/// complete, and then retrieves its result, if available.
		/// </summary>
		/// <param name="timeout">the maximum time to wait</param>
		/// <param name="unit">the time unit of the timeout argument</param>
		/// <returns>
		/// the computed result
		/// </returns>
		V Get(long timeout, TimeUnit unit);
		
		/// <summary>
		/// Returns true if this task was cancelled before it completed 
		/// normally.
		/// </summary>
		/// <returns>
		/// true if this task was cancelled before it completed
		/// </returns>
		bool IsCancelled();
		
		/// <summary>
		/// Returns true if this task completed. Completion may be due to normal
		/// termination, an exception, or cancellation -- in all of these cases,
		/// this method will return true.
		/// </summary>
		/// <returns>
		/// true if this task completed
		/// </returns>
		bool IsDone();
	}
}
