
using System;

namespace Java.Time
{
	/// <summary>
	/// Description of ZoneId.
	/// </summary>
	public class ZoneId
	{
//		private readonly TimeZoneInfo zoneInfo;
		
		private ZoneId(string zoneId)
		{
//			zoneInfo = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
		}
		
		public static ZoneId Of(string zoneId)
		{
			return new ZoneId(zoneId);
		}
		
		public static ZoneId SystemDefault()
		{
			return new ZoneId(TimeZone.CurrentTimeZone.StandardName);
		}
	}
}
