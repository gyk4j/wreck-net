
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Java.Time
{
	/// <summary>
	/// Description of Instant.
	/// </summary>
	public class Instant
	{
		public static readonly Instant EPOCH = new Instant(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
		public static readonly Instant MAX = new Instant(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc));
		public static readonly Instant MIN = new Instant(new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc));
		
		private DateTime instant;
		public DateTime DateTime
		{
			get { return instant; }
			set { instant = value; }
		}
		
		private Instant(DateTime d)
		{
			if(d.Kind == DateTimeKind.Utc)
				instant = d;
			else if(d.Kind == DateTimeKind.Local)
				instant = d.ToUniversalTime();
			else // Assume local time if unspecified
				instant = d.ToUniversalTime();
			
			Debug.Assert(instant.Kind == DateTimeKind.Utc);
		}
		
		public ZonedDateTime AtZone(ZoneId zone)
		{
			return ZonedDateTime.From(this);
		}
		
		// HACK: Implement Instant.From(TemporalAccessor interface) for accuracy and correctness to Java API.
		public static Instant From(LocalDateTime temporal)
		{
			return new Instant(temporal.DateTime);
		}
		
		public static Instant From(DateTime temporal)
		{
			return new Instant(temporal);
		}
		
		public static Instant Now()
		{
			return new Instant(DateTime.Now);
		}
		
		public static Instant OfEpochMillis(long epochMilli)
		{
			return new Instant(new DateTime(0, DateTimeKind.Utc));
		}
		
		public static Instant OfEpochSecond(long epochSecond)
		{
			return new Instant(new DateTime(epochSecond * 1000, DateTimeKind.Utc));
		}
		
		public static Instant OfEpochSecond(long epochSecond, long nanoAdjustment)
		{
			return new Instant(new DateTime((epochSecond * 1000) + nanoAdjustment, DateTimeKind.Utc));
		}
		
		public static Instant Parse(string s)
		{
			DateTime parsed;
			
			bool ok = DateTime.TryParse(
				s, 
				null, 
				DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, 
				out parsed);
			
			return ok ? new Instant(parsed) : null;
		}
		
		public override string ToString()
		{
			return instant.ToString("yyyy-MM-ddTHH:mm:ssZ");
		}
	}
}
