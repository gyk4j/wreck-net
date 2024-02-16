
using System;
using Javax.Swing.Event;

namespace Javax.Swing
{
	/// <summary>
	/// A generic implementation of BoundedRangeModel.
	/// </summary>
	public class DefaultBoundedRangeModel : BoundedRangeModel
	{
		protected ChangeEvent changeEvent = null;
		
		/** The listeners waiting for model changes. */
		protected EventListenerList listenerList = new EventListenerList();

		private int val = 0;
		private int extent = 0;
		private int min = 0;
		private int max = 100;
		private bool isAdjusting = false;
		
		/// <summary>
		/// Initializes all of the properties with default values.
		/// Those values are:
		/// <list type="bullet">
		/// <listheader>
		/// 	<term>property</term>
		///		<description>value</description>
		/// </listheader>
		/// <item>
		/// 	<term><code>value</code></term>
		/// 	<description>0</description>
		/// </item>
		/// <item>
		/// 	<term><code>extent</code></term>
		/// 	<description>0</description>
		/// </item>
		/// <item>
		/// 	<term><code>minimum</code></term>
		/// 	<description>0</description>
		/// </item>
		/// <item>
		/// 	<term><code>maximum</code></term>
		/// 	<description>100</description>
		/// </item>
		/// <item>
		/// 	<term><code>adjusting</code></term>
		/// 	<description>false</description>
		/// </item>
		/// </list>
		/// </summary>
		public DefaultBoundedRangeModel()
		{
		}
		
		/// <summary>
		/// Initializes value, extent, minimum and maximum. Adjusting is false.
		/// Throws an <code>IllegalArgumentException</code> if the following
		/// constraints aren't satisfied:
		/// <code>
		/// min &lt;= value &lt;= value+extent &lt;= max
		/// </code>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="extent"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public DefaultBoundedRangeModel(int val, int extent, int min, int max)
		{
			if ((max >= min) &&
			    (val >= min) &&
			    ((val + extent) >= val) &&
			    ((val + extent) <= max))
			{
				this.val = val;
				this.extent = extent;
				this.min = min;
				this.max = max;
			}
			else
			{
				throw new ArgumentException("invalid range properties");
			}
		}
		
		/// <summary>
		/// Adds a <c>ChangeListener</c>.  The change listeners are run each
		/// time any one of the Bounded Range model properties changes.
		/// </summary>
		/// <param name="l">the ChangeListener to add</param>
		/// <see cref="#RemoveChangeListener" />
		/// <see cref="BoundedRangeModel#AddChangeListener" />
		public void AddChangeListener(ChangeListener l)
		{
			listenerList.Add(typeof(ChangeListener), l);
		}
		
		/// <summary>
		/// Returns the model's extent.
		/// </summary>
		/// <returns>the model's extent</returns>
		public int GetExtent()
		{
			return extent;
		}
		
		/// <summary>
		/// Returns the model's maximum.
		/// </summary>
		/// <returns>the model's maximum</returns>
		public int GetMaximum()
		{
			return max;
		}
		
		/// <summary>
		/// Returns the model's minimum.
		/// </summary>
		/// <returns>the model's minimum</returns>
		public int GetMinimum()
		{
			return min;
		}
		
		/// <summary>
		/// Returns the model's current value.
		/// </summary>
		/// <returns>the model's current value</returns>
		/// <see cref="#SetValue">#SetValue</see>
		/// <see cref="BoundedRangeModel#GetValue">BoundedRangeModel#GetValue</see>
		public int GetValue()
		{
			return val;
		}
		
		/// <summary>
		/// Returns true if the value is in the process of changing
		/// as a result of actions being taken by the user.
		/// 
		/// </summary>
		/// <returns>the value of the <c>valueIsAdjusting</c> property</returns>
		/// <see cref="#SetValue" />
		/// <see cref="BoundedRangeModel#GetValueIsAdjusting" />
		public bool GetValueIsAdjusting()
		{
			return isAdjusting;
		}
		
		/// <summary>
		/// Removes a <c>ChangeListener</c>.
		/// </summary>
		/// <param name="l">the <c>ChangeListener</c> to remove</param>
		/// <see cref="#AddChangeListener" />
		/// <see cref="BoundedRangeModel#RemoveChangeListener" />
		public void RemoveChangeListener(ChangeListener l)
		{
			listenerList.Remove(typeof(ChangeListener), l);
		}
		
		/// <summary>
		/// Sets the extent to <paramref name="n" /> after ensuring that <paramref name="n" />
		/// is greater than or equal to zero and falls within the model's
		/// constraints:
		/// <code>
		/// 	minimum &lt;= value &lt;= value+extent &lt;= maximum
		/// </code>
		/// <see cref="BoundedRangeModel#setExtent"></see>
		/// </summary>
		/// <param name="newExtent"></param>
		public void SetExtent(int n)
		{
			int newExtent = Math.Max(0, n);
			if(val + newExtent > max)
			{
				newExtent = max - val;
			}
			SetRangeProperties(val, newExtent, min, max, isAdjusting);
		}
		
		/// <summary>
		/// Sets the maximum to <paramref name="n" /> after ensuring that <paramref name="n" />
		/// that the other three properties obey the model's constraints:
		/// <code>
		/// 	minimum &lt;= value &lt;= value+extent &lt;= maximum
		/// </code>
		/// <see cref="BoundedRangeModel#setMaximum" />
		/// </summary>
		/// <param name="newMaximum"></param>
		public void SetMaximum(int n)
		{
			int newMin = Math.Min(n, min);
			int newExtent = Math.Min(n - newMin, extent);
			int newValue = Math.Min(n - newExtent, val);
			SetRangeProperties(newValue, newExtent, newMin, n, isAdjusting);
		}
		
		/// <summary>
		/// Sets the minimum to <paramref name="n" /> after ensuring that <paramref name="n" />
		/// that the other three properties obey the model's constraints:
		/// <code>
		/// 	minimum &lt;= value &lt;= value+extent &lt;= maximum
		/// </code>
		/// <see cref="#GetMinimum" />
		/// <see cref="BoundedRangeModel#SetMinimum" />
		/// </summary>
		/// <param name="newMinimum"></param>
		public void SetMinimum(int n)
		{
			int newMax = Math.Max(n, max);
			int newValue = Math.Max(n, val);
			int newExtent = Math.Min(newMax - newValue, extent);
			SetRangeProperties(newValue, newExtent, n, newMax, isAdjusting);
		}
		
		/// <summary>
		/// Sets all of the <c>BoundedRangeModel</c> properties after forcing
		/// the arguments to obey the usual constraints:
		/// 
		/// <code>
		/// 	minimum &lt;= value &lt;= value+extent &lt;= maximum
		/// </code>
		/// <para>At most, one <c>ChangeEvent</c> is generated.</para>
		/// </summary>
		/// <see cref="BoundedRangeModel#SetRangeProperties" />
		/// <see cref="#SetValue" />
		/// <see cref="#SetExtent" />
		/// <see cref="#SetMinimum" />
		/// <see cref="#SetMaximum" />
		/// <see cref="#SetValueIsAdjusting" />
		/// <param name="value"></param>
		/// <param name="extent"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="adjusting"></param>
		public void SetRangeProperties(int newValue, int newExtent, int newMin, int newMax, bool adjusting)
		{
			if (newMin > newMax)
			{
				newMin = newMax;
			}
			if (newValue > newMax)
			{
				newMax = newValue;
			}
			if (newValue < newMin)
			{
				newMin = newValue;
			}

			/* Convert the addends to long so that extent can be
			 * Integer.MAX_VALUE without rolling over the sum.
			 * A JCK test covers this, see bug 4097718.
			 */
			if (((long)newExtent + (long)newValue) > newMax)
			{
				newExtent = newMax - newValue;
			}

			if (newExtent < 0)
			{
				newExtent = 0;
			}

			bool isChange =
				(newValue != val) ||
				(newExtent != extent) ||
				(newMin != min) ||
				(newMax != max) ||
				(adjusting != isAdjusting);

			if (isChange)
			{
				val = newValue;
				extent = newExtent;
				min = newMin;
				max = newMax;
				isAdjusting = adjusting;

				FireStateChanged();
			}
		}
		
		/// <summary>
		/// Sets the current value of the model. For a slider, that
		/// determines where the knob appears. Ensures that the new
		/// value, <paramref name="n"></paramref> falls within the model's constraints:
		/// <code>
		/// 	 minimum &lt;= value &lt;= value+extent &lt;= maximum
		/// </code>
		/// </summary>
		/// <param name="newValue"></param>
		/// <see cref="BoundedRangeModel#setValue"></see>
		public void SetValue(int n)
		{
			n = Math.Min(n, int.MaxValue - extent);

			int newValue = Math.Max(n, min);
			if (newValue + extent > max)
			{
				newValue = max - extent;
			}
			SetRangeProperties(newValue, extent, min, max, isAdjusting);
		}
		
		/// <summary>
		/// Sets the <c>valueIsAdjusting</c> property.
		/// <see cref="#GetValueIsAdjusting" />
		/// <see cref="#SetValue" />
		/// <see cref="BoundedRangeModel#setValueIsAdjusting" />
		/// </summary>
		/// <param name="b"></param>
		public void SetValueIsAdjusting(bool b)
		{
			SetRangeProperties(val, extent, min, max, b);
		}
		
		/// <summary>
		/// Returns an array of all the change listeners
		/// registered on this <c>DefaultBoundedRangeModel</c>.
		/// </summary>
		/// <returns>
		/// all of this model's <c>ChangeListener</c>s
		/// or an empty
		/// array if no change listeners are currently registered
		/// </returns>
		/// <see cref="#AddChangeListener" />
		/// <see cref="#RemoveChangeListener" />
		public ChangeListener[] GetChangeListeners()
		{
			return listenerList.GetListeners<ChangeListener>(typeof(ChangeListener));
		}
		
		/// <summary>
		/// Runs each <c>ChangeListener</c>'s <c>StateChanged</c> method.
		/// </summary>
		/// <see cref="#SetRangeProperties" />
		/// <see cref="EventListenerList" />
		protected void FireStateChanged()
		{
			object[] listeners = listenerList.GetListenerList();
			for (int i = listeners.Length - 2; i >= 0; i -=2 )
			{
				if (listeners[i] == typeof(ChangeListener))
				{
					if (changeEvent == null)
					{
						changeEvent = new ChangeEvent(this);
					}
					((ChangeListener)listeners[i+1]).StateChanged(changeEvent);
				}
			}
		}
		
		public override string ToString()
		{
			string modelString =
				"value=" + GetValue() + ", " +
				"extent=" + GetExtent() + ", " +
				"min=" + GetMinimum() + ", " +
				"max=" + GetMaximum() + ", " +
				"adj=" + GetValueIsAdjusting();

			return GetType().FullName + "[" + modelString + "]";
		}
	}
}
