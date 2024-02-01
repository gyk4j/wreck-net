
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
		
		public static readonly LocalDateTime MAX = new LocalDateTime(new DateTime(0,0,0,0,0,0, DateTimeKind.Local));
		public static readonly LocalDateTime MIN = new LocalDateTime(new DateTime(0,0,0,0,0,0, DateTimeKind.Local));
		
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
		
		public static LocalDateTime Parse(string s, string dateTimeFormatter)
		{
			DateTime l;
			bool isParsed = DateTime.TryParseExact(
					s,
					dateTimeFormatter,
					CultureInfo.InvariantCulture,
					DateTimeStyles.RoundtripKind,
					out l);
			
			if(!isParsed)
			{
				LOG.WarnFormat("Parse failed: {0}", s);
				return null;
			}
			else
			{
				// Since it's not a LocalDateTime (maybe a UTC or zone offset), we convert it.
				if(l.Kind != DateTimeKind.Local)
				{
					l = l.ToLocalTime();
				}
			
				Debug.Assert(l.Kind == DateTimeKind.Local);
				return new LocalDateTime(l);
			}
		}
	}
}
