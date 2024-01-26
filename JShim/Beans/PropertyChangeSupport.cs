
using System;
using System.Collections.Generic;

namespace JShim.Beans
{
	/// <summary>
	/// Description of PropertyChangeSupport.
	/// </summary>
	public class PropertyChangeSupport
	{
		private readonly object o;
		
		protected List<PropertyChangeListener> propertyChangeListeners;
		
		public PropertyChangeSupport(object o)
		{
			this.o = o;
			this.propertyChangeListeners = new List<PropertyChangeListener>();
		}
		
		public void AddPropertyChangeListener(PropertyChangeListener listener)
		{
			this.propertyChangeListeners.Add(listener);
		}
		
		public bool RemovePropertyChangeListener(PropertyChangeListener listener)
		{
			return this.propertyChangeListeners.Remove(listener);
		}
		
		public virtual void FirePropertyChange(PropertyChangeEvent evt)
		{
			if(!evt.NewValue.Equals(evt.OldValue))
			{
				// Find the property change listener to trigger.
				this.propertyChangeListeners.ForEach(
					pcl =>
					{
						pcl.PropertyChange(evt);
					}
				);
			}
		}
		
		public virtual void FirePropertyChange(string propertyName,
		                                       object oldValue,
		                                       object newValue)
		{
			FirePropertyChange(
				new PropertyChangeEvent(
					o, 
					propertyName, 
					oldValue, 
					newValue));
		}
	}
}
