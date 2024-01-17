
using System;
using System.Text;
using JShim.Util;

namespace JShim.Beans
{
	/// <summary>
	/// Description of PropertyChangeEvent.
	/// </summary>
	public class PropertyChangeEvent : EventObject
	{
		/// <summary>
		/// Constructs a new <code>PropertyChangeEvent</code>.
		/// </summary>
		/// <param name="source">the bean that fired the event</param>
		/// <param name="propertyName">the programmatic name of the property that was changed</param>
		/// <param name="oldValue">the old value of the property</param>
		/// <param name="newValue">the new value of the property</param>
		public PropertyChangeEvent(
			object source, string propertyName,
			object oldValue, object newValue) :
			base(source)
		{
			this.propertyName = propertyName;
			this.newValue = newValue;
			this.oldValue = oldValue;
		}
		
		/// <summary>
		/// Gets the programmatic name of the property that was changed.
		/// </summary>
		/// <returns>
		/// The programmatic name of the property that was changed.
		/// May be null if multiple properties have changed.
		/// </returns>
		public string PropertyName
		{
			get { return propertyName; }
		}
		
		/// <summary>
		/// Gets the new value for the property, expressed as an Object.
		/// </summary>
		/// <returns>
		/// The new value for the property, expressed as an Object.
		/// May be null if multiple properties have changed.
		/// </returns>
		public object NewValue
		{
			get { return newValue; }
		}
		
		/// <summary>
		/// Gets the old value for the property, expressed as an Object.
		/// </summary>
		/// <returns>
		/// The old value for the property, expressed as an Object.
		/// May be null if multiple properties have changed.
		/// </returns>
		public object OldValue
		{
			get { return oldValue; }
		}
		
		/// <summary>
		/// The "propagationId" field is reserved for future use.  In Beans 1.0
		/// the sole requirement is that if a listener catches a PropertyChangeEvent
		/// and then fires a PropertyChangeEvent of its own, then it should
		/// make sure that it propagates the propagationId field from its
		/// incoming event to its outgoing event.
		/// </summary>
		/// <returns>
		/// the propagationId object associated with a bound/constrained
		/// property update.
		/// </returns>
		public object PropagationId
		{
			set { this.propagationId = value; }
			get { return propagationId; }
		}
		
		/// <summary>
		/// name of the property that changed.  May be null, if not known.
		/// </summary>
		private string propertyName;
		
		/// <summary>
		/// New value for property.  May be null if not known.
		/// </summary>
		private object newValue;
		
		/// <summary>
		/// Previous value for property.  May be null if not known.
		/// </summary>
		private Object oldValue;
		
		/// <summary>
		/// Propagation ID.  May be null.
		/// </summary>
		private object propagationId;
		
		/// <summary>
		/// Returns a string representation of the object.
		/// </summary>
		/// <returns>
		/// a string representation of the object
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(GetType().FullName);
			sb.Append("[propertyName=").Append(PropertyName);
			AppendTo(sb);
			sb.Append("; oldValue=").Append(OldValue);
			sb.Append("; newValue=").Append(NewValue);
			sb.Append("; propagationId=").Append(PropagationId);
			sb.Append("; source=").Append(Source);
			return sb.Append("]").ToString();
		}
		
		void AppendTo(StringBuilder sb)
		{
		}
	}
}
