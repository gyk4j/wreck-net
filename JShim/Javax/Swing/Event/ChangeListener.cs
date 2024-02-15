
using System;

namespace Javax.Swing.Event
{
	/// <summary>
	/// Defines an object which listens for ChangeEvents.
	/// </summary>
	public interface ChangeListener
	{
		/// <summary>
		/// Invoked when the target of the listener has changed its state.
		/// </summary>
		/// <param name="e">a ChangeEvent object</param>
		void StateChanged(ChangeEvent e);
	}
}
