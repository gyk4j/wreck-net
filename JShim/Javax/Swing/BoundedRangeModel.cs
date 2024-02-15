
using System;
using Javax.Swing.Event;

namespace Javax.Swing
{
	/// <summary>
	/// Description of BoundedRangeModel.
	/// </summary>
	public interface BoundedRangeModel
	{
		/// <summary>
		/// Adds a ChangeListener to the model's listener list.
		/// </summary>
		/// <param name="x">the ChangeListener to add</param>
		void AddChangeListener(ChangeListener x);
		
		/// <summary>
		/// Returns the model's extent, the length of the inner range that 
		/// begins at the model's value.
		/// </summary>
		/// <returns>the value of the model's extent property</returns>
		int GetExtent();
		
		/// <summary>
		/// Returns the model's maximum. Note that the upper limit on the 
		/// model's value is (maximum - extent).
		/// </summary>
		/// <returns>the value of the maximum property.</returns>
		int GetMaximum();
		
		/// <summary>
		/// Returns the minimum acceptable value.
		/// </summary>
		/// <returns>the value of the minimum property</returns>
		int GetMinimum();
		
		/// <summary>
		/// Returns the model's current value. Note that the upper limit on the
		/// model's value is maximum - extent and the lower limit is minimum.
		/// </summary>
		/// <returns>the model's value</returns>
		int GetValue();
		
		/// <summary>
		/// Returns true if the current changes to the value property are part 
		/// of a series of changes.
		/// </summary>
		/// <returns>the valueIsAdjustingProperty.</returns>
		bool GetValueIsAdjusting();
		
		/// <summary>
		/// Removes a ChangeListener from the model's listener list.
		/// </summary>
		/// <param name="x">the ChangeListener to remove</param>
		void RemoveChangeListener(ChangeListener x);
		
		/// <summary>
		/// Sets the model's extent. The newExtent is forced to be greater than 
		/// or equal to zero and less than or equal to maximum - value. 
		/// 
		/// When a BoundedRange model is used with a scrollbar the extent 
		/// defines the length of the scrollbar knob (aka the "thumb" or 
		/// "elevator"). The extent usually represents how much of the object 
		/// being scrolled is visible. When used with a slider, the extent 
		/// determines how much the value can "jump", for example when the user 
		/// presses PgUp or PgDn. 
		/// 
		/// Notifies any listeners if the model changes.
		/// 
		/// </summary>
		/// <param name="newExtent">the model's new extent</param>
		void SetExtent(int newExtent);
		
		/// <summary>
		/// Sets the model's maximum to newMaximum. The other three properties 
		/// may be changed as well, to ensure that  
		/// minimum &lt;= value &lt;= value+extent &lt;= maximum
		/// 
		/// Notifies any listeners if the model changes.
		/// 
		/// </summary>
		/// <param name="newMaximum">the model's new maximum</param>
		void SetMaximum(int newMaximum);
		
		/// <summary>
		/// Sets the model's minimum to newMinimum. The other three properties 
		/// may be changed as well, to ensure that:  
		/// minimum &lt;= value &lt;= value+extent &lt;= maximum
		/// 
		/// Notifies any listeners if the model changes.
		/// </summary>
		/// <param name="newMinimum">the model's new minimum</param>
		void SetMinimum(int newMinimum);
		
		/// <summary>
		/// This method sets all of the model's data with a single method call. 
		/// The method results in a single change event being generated. This is 
		/// convenient when you need to adjust all the model data simultaneously 
		/// and do not want individual change events to occur.
		/// </summary>
		/// <param name="value">an int giving the current value</param>
		/// <param name="extent">an int giving the amount by which the value can "jump"</param>
		/// <param name="min">an int giving the minimum value</param>
		/// <param name="max">an int giving the maximum value</param>
		/// <param name="adjusting">a boolean, true if a series of changes are in progress</param>
		void SetRangeProperties(int value, int extent, int min, int max, bool adjusting);
		
		/// <summary>
		/// Sets the model's current value to newValue if newValue satisfies the
		/// model's constraints. Those constraints are:
		/// minimum <= value <= value+extent <= maximum
		/// 
		/// Otherwise, if newValue is less than minimum it's set to minimum, if 
		/// its greater than maximum then it's set to maximum, and if it's 
		/// greater than value+extent then it's set to value+extent.
		/// 
		/// When a BoundedRange model is used with a scrollbar the value 
		/// specifies the origin of the scrollbar knob (aka the "thumb" or 
		/// "elevator"). The value usually represents the origin of the visible
		/// part of the object being scrolled.
		/// 
		/// Notifies any listeners if the model changes.
		/// 
		/// </summary>
		/// <param name="newValue">the model's new value</param>
		void SetValue(int newValue);
		
		/// <summary>
		/// This attribute indicates that any upcoming changes to the value of 
		/// the model should be considered a single event. This attribute will 
		/// be set to true at the start of a series of changes to the value, and
		/// will be set to false when the value has finished changing. Normally
		/// this allows a listener to only take action when the final value 
		/// change in committed, instead of having to do updates for all 
		/// intermediate values.
		/// 
		/// Sliders and scrollbars use this property when a drag is underway.
		/// 
		/// </summary>
		/// <param name="b">
		/// true if the upcoming changes to the value property 
		/// are part of a series
		/// </param>
		void SetValueIsAdjusting(bool b);
		
	}
}
