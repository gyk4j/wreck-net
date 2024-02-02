
using System;
using Wreck.Resources;

namespace Wreck.IO
{
	/// <summary>
	/// Description of Metadata.
	/// </summary>
	public class Metadata : IComparable<Metadata>
	{
		private readonly string key;
		private readonly string value;
		private readonly DateTime time;
		private readonly CorrectionEnum group;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">Metadata Tag Key</param>
		/// <param name="value">Raw tag value as string</param>
		/// <param name="time">Parsed value as <paramref name="System.DateTime" /></param>
		/// <param name="group">Attribute as <paramref name="Wreck.Resources.ConnectionEnum" /></param>
		public Metadata(string key, string value, DateTime time, CorrectionEnum group)
		{
			this.key = key;
			this.value = value;
			this.time = time;
			this.group = group;
		}
		
		public Metadata Clone(DateTime ft) 
		{
			Metadata newInstance = new Metadata(key, ft.ToString(), ft, group);
			return newInstance;
		}
	
		public string Key
		{
			get { return key; }	
		}
		
		public string Value
		{
			get { return value; }
		}
		
		public DateTime Time
		{
			get { return time; }
		}
		
		public CorrectionEnum Group
		{
			get { return group; }
		}
		
		public override bool Equals(object obj)
		{
			if(this == obj)
				return true;
			
			Metadata m = (Metadata) obj;
			
			return Key.Equals(m.Key)
				&& Value.Equals(m.Value)
				&& Time.Equals(m.Time)
				&& Group.Equals(m.Group);
		}
		
		public override int GetHashCode()
        {
            return 
            	Key.GetHashCode() ^
                Value.GetHashCode() ^
            	Time.GetHashCode() ^
            	Group.GetHashCode();
        }
		
		public int CompareTo(Metadata o) {
			int result = 0;
			if (Time != null && o.Time != null)
				result = Time.CompareTo(o.Time);
			else if(Time == null)
				result = 1;
			else if(o.Time == null)
				result = -1;
			else
				result = 0;
			return result;
		}
		
		
		public override string ToString()
		{
			return string.Format("{0} {1} {2}",
			                     key,
			                     value,
			                     group.ToString());
		}
	}
}
