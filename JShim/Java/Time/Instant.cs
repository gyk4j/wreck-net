
using System;
using System.Globalization;

namespace Java.Time
{
	/// <summary>
	/// Description of Instant.
	/// </summary>
	public class Instant
	{
		public static DateTime Parse(string s)
		{
			return DateTime.Parse(s, null, DateTimeStyles.RoundtripKind);
		}
	}
}
