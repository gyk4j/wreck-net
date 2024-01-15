
using System;

namespace Wreck.Util.Logging
{
	/// <summary>
	/// Description of ExtensionEvent.
	/// </summary>
	public class ExtensionEvent
	{
		protected readonly string file;
		protected readonly bool hasMetadata;
		
		public ExtensionEvent(string file, bool hasMetadata)
		{
			this.file = file;
			this.hasMetadata = hasMetadata;
		}
		
		public string File
		{
			get { return file; }
		}

		public bool HasMetadata
		{
			get { return hasMetadata; }
		}

		public override string ToString()
		{
			return ExtensionStatistics.GetExtensionKey(file);
		}
	}
}
