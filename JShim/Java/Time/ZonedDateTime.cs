
using System;

namespace Java.Time
{
	/// <summary>
	/// Description of ZonedDateTime.
	/// </summary>
	public class ZonedDateTime
	{
		private DateTime dateTime;
		protected DateTime DateTime
		{
			get { return dateTime; }
			set { dateTime = value; }
		}
		
		private TimeZone timeZone;
		protected TimeZone TimeZone
		{
			get { return timeZone; }
			set { timeZone = value; }
		}
		
		private ZonedDateTime(DateTime dateTime)
		{
			dateTime = dateTime.ToLocalTime();
			timeZone = TimeZone.CurrentTimeZone;
		}
		
		public static ZonedDateTime From(LocalDateTime localDateTime)
		{
			ZonedDateTime zdt = new ZonedDateTime(localDateTime.DateTime);
			return zdt;
		}
		
		public static ZonedDateTime From(Instant instant)
		{
			ZonedDateTime zdt = new ZonedDateTime(instant.DateTime);
			return zdt;
		}
		
		public LocalDateTime ToLocalDateTime()
		{
			DateTime dt = timeZone.ToLocalTime(dateTime);
			return LocalDateTime.Of(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
		}
		
		public Instant ToInstant()
		{
			return Instant.From(dateTime);
		}
		
		public override string ToString()
		{
			return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz") + 
				"[" + timeZone.StandardName + "]";
		}
	}
}
