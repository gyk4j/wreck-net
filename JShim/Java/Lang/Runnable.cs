
using System;

namespace Java.Lang
{
	/// <summary>
	/// Description of Runnable.
	/// </summary>
	public interface Runnable
	{
		/// <summary>
		/// When an object implementing interface Runnable is used to create a 
		/// thread, starting the thread causes the object's run method to be 
		/// called in that separately executing thread.
		/// 
		/// The general contract of the method run is that it may take any 
		/// action whatsoever.
		/// </summary>
		void Run();
	}
}
