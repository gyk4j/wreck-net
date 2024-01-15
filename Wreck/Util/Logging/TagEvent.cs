
using System;
using Wreck.IO.Reader;

namespace Wreck.Util.Logging
{
	/// <summary>
	/// Description of TagEvent.
	/// </summary>
	public class TagEvent
	{
		protected readonly ITimestampReader reader;
		protected readonly string name;
		
		public TagEvent(ITimestampReader reader, string name) : base()
		{
			this.reader = reader;
			this.name = name;
		}

		public ITimestampReader Reader
		{
			get { return reader; }
		}
		
		public string Name
		{
			get { return name; }
		}

		public override string ToString()
		{
			return reader.GetType().Name + ":" + name;
		}
	}
}
