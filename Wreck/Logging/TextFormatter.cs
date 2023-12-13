
using System;

namespace Wreck.Logging
{
	/// <summary>
	/// Provides various helper function to generate formatted text string.
	/// </summary>
	public class TextFormatter
	{
		public static string Format(TimeSpan ts)
		{
			String s;
			if(ts.Days >= 365)
				s = (ts.Days / 365) + " years";
			else if(ts.Days >= 30)
				s = (ts.Days / 30) + " months";
			else if(ts.Days >= 7)
				s = (ts.Days / 7) + " weeks";
			else if(ts.Days >= 1)
				s = ts.Days + " days";
			else if(ts.Hours > 0)
				s = ts.Hours + " hours";
			else if(ts.Minutes > 0)
				s = ts.Minutes + " mins";
			else if(ts.Seconds > 0)
				s = ts.Seconds + " secs";
			else if(ts.Milliseconds > 0)
				s = ts.Milliseconds + " msecs";
			else
				s = string.Empty;
			
			return s;
		}
	}
}
