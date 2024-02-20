
using System;

namespace Wreck.Time
{
	/// <summary>
	/// Static convenience methods to handle date time.
	/// </summary>
	public class TimeUtils
	{
		// Windows Explorer will not show any date time before 1980-01-01T00:00:00.
		// Safe to assume that any legit file time should be after that.
		// Anything other than that are likely some placeholder or fake values.
		public static readonly DateTime VALID_PERIOD_MIN = 
			new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	
		// No file should be created, modified or accessed beyond the current latest time.
		// Any future date time values found are likely placeholder values.
		public static readonly DateTime VALID_PERIOD_MAX =
			DateTime.Now;
		
		public static DateTime GetInstant(TimeOrder order, DateTime instant, DateTime filetime)
		{
			DateTime fileInstance = filetime; // Java requires FileTime to Instant conversion.
			if(order == TimeOrder.Before) 
			{
				return instant.CompareTo(fileInstance) < 0? instant: fileInstance;
			}
			else if(order == TimeOrder.After) 
			{
				return instant.CompareTo(fileInstance) > 0? instant: fileInstance;
			}
			else {
				throw new Exception("");
			}
		}
		
		public static bool IsLaterThan(DateTime a, DateTime b) {
			if(a == null || b == null)
				return false;
			
			DateTime ldtA = a.ToLocalTime();
			DateTime ldtB = b.ToLocalTime();
			
			return ldtA.Date.CompareTo(ldtB.Date) > 0;
		}
		
		public static bool IsSameTime(DateTime a, DateTime b) 
		{
			if(a == null && b == null)
				return true;
			else if(a == null || b == null)
				return false;
			
			return a.Equals(b);
		}
		
		public static DateTime? UpdateDateTime(DateTime? dateTimeStamp, bool before, DateTime time, long epochTime) 
		{
			DateTime? instant = null;
			
			// Use zip extended attributes as first choice for better accuracy.
			if(time != null) 
			{
				instant = time; // Java requires FileTime to Instant conversion
			}
			// Fallback for older format zip archive without the extended attributes.
			else if(epochTime > 0) {
				instant = new DateTime(epochTime, DateTimeKind.Utc);			
			}
			else {
				instant = null;
			}
			
			if(instant != null) 
			{
				int compare = instant.Value.CompareTo(dateTimeStamp);
				if(compare == 0) 
				{
					// Expected == Actual. No need to fix.
	//				Console.WriteLine("Same timestamp. Ignored.");
				}
				else if((before && compare < 0) || (!before && compare > 0)) 
				{
					// Update the last found timestamp with an earlier or later one.
					dateTimeStamp = instant.Value;
				}
				else 
				{
	//				Console.WriteLine("Worse timestamp. Ignored.");
				}
			}
			else 
			{
				// Unlikely. 
				// For standard zip32, at least one time (usually last modified) should be available.
				// For zip64, there are Creation, Modified and Accessed time that may or may not be filled.
	//			Console.WriteLine("No metadata timestamp found.");
				dateTimeStamp = null;
			}
			
			return dateTimeStamp;
		}
	}
}
