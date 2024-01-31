
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
		
		public static DateTime Parse(string s, string dateTimeFormatter)
		{
			DateTime l = DateTime.MaxValue;
			bool isParsed = DateTime.TryParseExact(
					s,
					dateTimeFormatter,
					CultureInfo.InvariantCulture,
					DateTimeStyles.RoundtripKind,
					out l);
			
			if(!isParsed)
				LOG.WarnFormat("Parse failed: {0}", s);
			
			//Debug.Assert(!l.Equals(DateTime.MaxValue));
			
			return l;
		}
	}
}
