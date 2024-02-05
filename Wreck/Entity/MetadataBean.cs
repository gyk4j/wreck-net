
using System;
using System.Collections.Generic;
using Java.Time;

namespace Wreck.Entity
{
	/// <summary>
	/// Description of MetadataBean.
	/// </summary>
	public class MetadataBean
	{
		private readonly IDictionary<string, Instant> tags = new Dictionary<string, Instant>();
		
		public IDictionary<string, Instant> Tags
		{
			get { return tags; }
		}
		
		public void AddTag(string key, Instant value)
		{
			if(Tags.ContainsKey(key))
				Tags[key] = value;
			else
				Tags.Add(key, value);
		}
		
		public Instant Earliest
		{
			get
			{
				Instant i = null;
				foreach(Instant v in Tags.Values)
				{
					if(i == null || (v != null && v.DateTime.CompareTo(i.DateTime) < 0))
						i = v;
						
				}
				return i;
			}
		}
		
		public Instant Latest
		{
			get
			{
				Instant i = null;
				foreach(Instant v in Tags.Values)
				{
					if(i == null || (v != null && v.DateTime.CompareTo(i.DateTime) > 0))
						i = v;
				}
				return i;
			}
		}
	}
}
