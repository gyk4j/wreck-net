
using System;

namespace Wreck.Entity
{
	/// <summary>
	/// Description of MetadataStatisticsBean.
	/// </summary>
	public class MetadataStatisticsBean
	{
		private readonly string parser;
		private readonly string tag;
		private readonly int count;
		
		public MetadataStatisticsBean(string parser, string tag, int count) : base()
		{
			this.parser = parser;
			this.tag = tag;
			this.count = count;
		}
		
		public string Parser
		{
			get { return parser; }
		}
		
		public string Tag
		{
			get { return tag; }
		}

		public int Count
		{
			get { return count; }
		}
	}
}
