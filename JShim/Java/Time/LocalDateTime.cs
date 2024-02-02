
using System;
using System.Diagnostics;
using System.Globalization;

using log4net;

namespace Java.Time
{
	/// <summary>
	/// Description of LocalDateTime.
	/// </summary>
	public sealed class LocalDateTime
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(LocalDateTime));
		
		public static readonly LocalDateTime MAX = new LocalDateTime(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Local));
		public static readonly LocalDateTime MIN = new LocalDateTime(new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Local));
		
		private DateTime localDateTime;
		public DateTime DateTime
		{
			get { return localDateTime; }
			set { localDateTime = value; }
		}
		
		private LocalDateTime(DateTime dt)
		{
			if(dt.Kind == DateTimeKind.Local)
				localDateTime = dt;
			else if(dt.Kind == DateTimeKind.Utc)
				localDateTime = dt.ToLocalTime();
			else
				localDateTime = dt.ToLocalTime();
			
			Debug.Assert(localDateTime.Kind == DateTimeKind.Local);
		}
		
		public static LocalDateTime	Of(int year, int month, int dayOfMonth, int hour, int minute, int second)
		{
			DateTime l = new DateTime(year, month, dayOfMonth, hour, minute, second, DateTimeKind.Local);
			return new LocalDateTime(l);
		}
		
		public static LocalDateTime Parse(string s, string dateTimeFormatter)
		{
			DateTime l;
			bool isParsed = DateTime.TryParseExact(
					s,
					dateTimeFormatter,
					CultureInfo.InvariantCulture,
					DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal,
					out l);
			
			if(!isParsed)
			{
				LOG.WarnFormat("Parse failed: {0} - {1}", s, dateTimeFormatter);
				return null;
			}
			else
			{
				// Since it's not a LocalDateTime (maybe a UTC or zone offset), we convert it.
				if(l.Kind == DateTimeKind.Utc)
					l = l.ToLocalTime();
				else if(l.Kind == DateTimeKind.Unspecified)
					l = l.ToUniversalTime().ToLocalTime();
			
				Debug.Assert(l.Kind == DateTimeKind.Local);
				return new LocalDateTime(l);
			}
		}
		
		public override string ToString()
		{
			return localDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
		}
	}
}
