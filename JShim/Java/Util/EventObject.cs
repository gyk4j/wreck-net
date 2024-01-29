
using System;

namespace Java.Util
{
	/// <summary>
	/// The root class from which all event state objects shall be derived.
	/// 
	/// All Events are constructed with a reference to the object, the "source",
	/// that is logically deemed to be the object upon which the Event in question
	/// initially occurred upon.
	/// </summary>
	public class EventObject
	{
		/// <summary>
		/// The object on which the Event initially occurred.
		/// </summary>
		protected object source;
		
		/// <summary>
		/// Constructs a prototypical Event.
		/// </summary>
		/// <param name="source">The object on which the Event initially occurred.</param>
		public EventObject(object source)
		{
			if (source == null)
				throw new ArgumentException("null source");

			this.source = source;
		}
		
		/// <summary>
		/// The object on which the Event initially occurred.
		/// </summary>
		/// <returns>The object on which the Event initially occurred.</returns>
		public object Source
		{
			get { return source; }
		}
		
		/// <summary>
		/// Returns a String representation of this EventObject.
		/// </summary>
		/// <returns>A a String representation of this EventObject.</returns>
		public override string ToString()
		{
			return this.GetType().FullName + "[source=" + source.ToString() + "]";
		}
	}
}
