
using System;
using System.IO;
using System.Runtime.Serialization;

using Java.IO;
using Java.Util;

namespace Javax.Swing.Event
{
	/// <summary>
	/// A class that holds a list of EventListeners.  A single instance
	/// can be used to hold all listeners (of all types) for the instance
	/// using the list.  It is the responsiblity of the class using the
	/// EventListenerList to provide type-safe API (preferably conforming
	/// to the JavaBeans spec) and methods which dispatch event notification
	/// methods to appropriate Event Listeners on the list.
	/// 
	/// The main benefits that this class provides are that it is relatively
	/// cheap in the case of no listeners, and it provides serialization for
	/// event-listener lists in a single place, as well as a degree of MT safety
	/// (when used correctly).
	/// 
	/// Usage example:
	/// 	Say one is defining a class that sends out FooEvents, and one wants
	/// to allow users of the class to register FooListeners and receive
	/// notification when FooEvents occur.  The following should be added
	/// to the class definition:
	/// <code>
	/// EventListenerList listenerList = new EventListenerList();
	/// FooEvent fooEvent = null;
	/// 
	/// public void AddFooListener(FooListener l)
	/// {
	/// 	listenerList.Add(typeof(FooListener), l);
	/// }
	/// 
	/// public void RemoveFooListener(FooListener l)
	/// {
	/// 	listenerList.Remove(typeof(FooListener), l);
	/// }
	/// 
	/// // Notify all listeners that have registered interest for
	/// // notification on this event type.  The event instance
	/// // is lazily created using the parameters passed into
	/// // the fire method.
	///
	/// protected void fireFooXXX() {
	///     // Guaranteed to return a non-null array
	///     Object[] listeners = listenerList.getListenerList();
	///     // Process the listeners last to first, notifying
	///     // those that are interested in this event
	///     for (int i = listeners.length-2; i&gt;=0; i-=2) {
	///         if (listeners[i]==FooListener.class) {
	///             // Lazily create the event:
	///             if (fooEvent == null)
	///                 fooEvent = new FooEvent(this);
	///             ((FooListener)listeners[i+1]).fooXXX(fooEvent);
	///         }
	///     }
	/// }
	/// </code>
	/// foo should be changed to the appropriate name, and fireFooXxx to the
	/// appropriate method name.  One fire method should exist for each
	/// notification method in the FooListener interface.
	/// <para>
	/// 
	/// <strong>Warning:</strong>
	/// Serialized objects of this class will not be compatible with
	/// future Swing releases. The current serialization support is
	/// appropriate for short term storage or RMI between applications running
	/// the same version of Swing.  As of 1.4, support for long term storage
	/// of all JavaBeans&trade;
	/// has been added to the <c>Java.Beans</c> namespace.
	/// Please see <see cref="java.beans.XMLEncoder"></see>.
	/// </summary>
	public class EventListenerList
	{
		/* A null array to be shared by all empty listener lists*/
		private readonly static object[] NULL_ARRAY = new object[0];
		/* The list of ListenerType - Listener pairs */
		protected object[] listenerList = NULL_ARRAY;
		
		/// <summary>
		/// Passes back the event listener list as an array
		/// of ListenerType-listener pairs.  Note that for
		/// performance reasons, this implementation passes back
		/// the actual data structure in which the listener data
		/// is stored internally!
		/// 
		/// This method is guaranteed to pass back a non-null
		/// array, so that no null-checking is required in
		/// fire methods.  A zero-length array of Object should
		/// be returned if there are currently no listeners.
		/// 
		/// WARNING!!! Absolutely NO modification of
		/// the data contained in this array should be made -- if
		/// any such manipulation is necessary, it should be done
		/// on a copy of the array returned rather than the array
		/// itself.
		/// </summary>
		/// <returns></returns>
		public object[] GetListenerList()
		{
			return listenerList;
		}
		
		/// <summary>
		/// Return an array of all the listeners of the given type.
		/// </summary>
		/// <param name="t"></param>
		/// <returns>all of the listeners of the specified type.</returns>
		public T[] GetListeners<T>(Type t) where T : EventListener
		{
			object[] lList = listenerList;
			int n = GetListenerCount(lList, t);
			T[] result = (T[])Array.CreateInstance(t, n);
			int j = 0;
			for (int i = lList.Length-2; i>=0; i-=2)
			{
				if (lList[i] == t)
				{
					result[j++] = (T)lList[i+1];
				}
			}
			return result;
		}
		
		/// <summary>
		/// Returns the total number of listeners for this listener list.
		/// </summary>
		/// <returns></returns>
		public int GetListenerCount()
		{
			return listenerList.Length/2;
		}
		
		/**
		 * Returns the total number of listeners of the supplied type
		 * for this listener list.
		 */
		
		/// <summary>
		/// Returns the total number of listeners of the supplied type
		/// for this listener list.
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public int GetListenerCount(Type t)
		{
			object[] lList = listenerList;
			return GetListenerCount(lList, t);
		}
		
		private int GetListenerCount(object[] list, Type t)
		{
			int count = 0;
			for (int i = 0; i < list.Length; i+=2)
			{
				if (t == (Type)list[i])
					count++;
			}
			return count;
		}
		
		/// <summary>
		/// Adds the listener as a listener of the specified type.
		/// </summary>
		/// <param name="t">the type of the listener to be added</param>
		/// <param name="l">the listener to be added</param>
		public void Add<T>(Type t, T l) where T : EventListener
		{
			if (l==null)
			{
				// In an ideal world, we would do an assertion here
				// to help developers know they are probably doing
				// something wrong
				return;
			}
			if (l.GetType() != t)
			{
				throw new ArgumentException("Listener " + l +
				                            " is not of type " + t);
			}
			if (listenerList == NULL_ARRAY)
			{
				// if this is the first listener added,
				// initialize the lists
				listenerList = new object[] { t, l };
			}
			else
			{
				// Otherwise copy the array and add the new listener
				int i = listenerList.Length;
				object[] tmp = new object[i+2];
				Array.Copy(listenerList, 0, tmp, 0, i);

				tmp[i] = t;
				tmp[i+1] = l;

				listenerList = tmp;
			}
		}
		
		/// <summary>
		/// Removes the listener as a listener of the specified type.
		/// </summary>
		/// <param name="t">the type of the listener to be removed</param>
		/// <param name="l">the listener to be removed</param>
		public void Remove<T>(Type t, T l) where T : EventListener
		{
			if (l ==null)
			{
				// In an ideal world, we would do an assertion here
				// to help developers know they are probably doing
				// something wrong
				return;
			}
			if (l.GetType() != t)
			{
				throw new ArgumentException("Listener " + l +
				                            " is not of type " + t);
			}
			// Is l on the list?
			int index = -1;
			for (int i = listenerList.Length-2; i>=0; i-=2)
			{
				if ((listenerList[i]==t) && (listenerList[i+1].Equals(l) == true))
				{
					index = i;
					break;
				}
			}

			// If so,  remove it
			if (index != -1)
			{
				object[] tmp = new object[listenerList.Length-2];
				// Copy the list up to index
				Array.Copy(listenerList, 0, tmp, 0, index);
				// Copy from two past the index, up to
				// the end of tmp (which is two elements
				// shorter than the old list)
				if (index < tmp.Length)
					Array.Copy(listenerList, index+2, tmp, index,
					           tmp.Length - index);
				// set the listener array to the new array or null
				listenerList = (tmp.Length == 0) ? NULL_ARRAY : tmp;
			}
		}
		
		// Serialization support.
		private void WriteObject(ObjectOutputStream s)
		{
			object[] lList = listenerList;
			s.DefaultWriteObject();

			// Save the non-null event listeners:
			for (int i = 0; i < lList.Length; i+=2)
			{
				Type t = (Type)lList[i];
				EventListener l = (EventListener)lList[i+1];
				if ((l!=null) && (l is ISerializable))
				{
					s.WriteObject(t.FullName);
					s.WriteObject(l);
				}
			}

			s.WriteObject(null);
		}
		
		private void ReadObject(ObjectInputStream s)
		{
			listenerList = NULL_ARRAY;
			s.DefaultReadObject();
			object listenerTypeOrNull;

			while (null != (listenerTypeOrNull = s.ReadObject()))
			{
				EventListener l = (EventListener)s.ReadObject();
				string name = (string) listenerTypeOrNull;
				Add(Type.GetType(name, true, false), l);
			}
		}
		
		/// <summary>
		/// Returns a string representation of the EventListenerList.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			object[] lList = listenerList;
			string s = "EventListenerList: ";
			s += lList.Length/2 + " listeners: ";
			for (int i = 0 ; i <= lList.Length-2 ; i+=2)
			{
				s += " type " + ((Type)lList[i]).FullName;
				s += " listener " + lList[i+1];
			}
			return s;
		}
	}
}
