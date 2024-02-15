
using System;
using Java.Util;

namespace Javax.Swing.Event
{
	/// <summary>
	/// ChangeEvent is used to notify interested parties that state has changed 
	/// in the event source. 
	/// </summary>
	public class ChangeEvent : EventObject
	{
		public ChangeEvent(object source) : base(source)
		{
			
		}
	}
}
