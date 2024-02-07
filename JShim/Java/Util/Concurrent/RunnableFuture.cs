
using System;
using Java.Lang;

namespace Java.Util.Concurrent
{
	/// <summary>
	/// Description of RunnableFuture.
	/// </summary>
	public interface RunnableFuture<V> : Runnable, Future<V>
	{
		/// <summary>
		/// Sets this Future to the result of its computation unless it has been
		/// cancelled.
		/// </summary>
		new void Run();
	}
}
