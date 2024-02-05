
using System;
using System.Collections.Generic;
using Wreck.Resources;

namespace Wreck.Entity
{
	/// <summary>
	/// Description of LogEntry.
	/// </summary>
	public class LogEntry : IComparable<LogEntry>
	{
		private readonly string path;
		private readonly long size;
		private readonly long crc;
		private readonly IDictionary<CorrectionEnum, DateTime> original;
		
		public LogEntry(
			string path,
			long size,
			long crc,
			IDictionary<CorrectionEnum, DateTime> original) : base()
		{
			this.path = path;
			this.size = size;
			this.crc = crc;
			this.original = original;
		}
		
		public string Path
		{
			get { return path; }
		}
		public long Size
		{
			get { return size; }
		}
		public long CRC
		{
			get { return crc; }
		}
		public IDictionary<CorrectionEnum, DateTime> Original
		{
			get { return original; }
		}
		
		public override bool Equals(Object obj) {
			if(this == obj)
				return true;
			
			if(obj is LogEntry)
			{
				LogEntry o = (LogEntry) obj;
				return Path.Equals(o.Path)
					&& CRC == o.CRC
					&& Size == o.Size
					&& IsFileTimeEquals(CorrectionEnum.CREATION, o)
					&& IsFileTimeEquals(CorrectionEnum.MODIFIED, o)
					&& IsFileTimeEquals(CorrectionEnum.ACCESSED, o);
			}
			else
			{
				return false;
			}
		}
		
		private bool IsFileTimeEquals(CorrectionEnum c, LogEntry o)
		{
			DateTime o1;
			bool f1 = Original.TryGetValue(c, out o1);
			
			DateTime o2;
			bool f2 = o.Original.TryGetValue(c, out o2);
			
			return (!f1 && !f2) || o1.Equals(o2);
		}
		
		public int CompareTo(LogEntry o)
		{
			int result;
			if(CRC == o.CRC)
				result = 0;
			else if(CRC < o.CRC)
				result = -1;
			else
				result = 1;
			return result;
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
