
using System;

namespace Wreck.Entity
{
	/// <summary>
	/// Description of ExtensionStatisticsBean.
	/// </summary>
	public class ExtensionStatisticsBean
	{
		private readonly string extension;
		private readonly int total;
		private readonly int hasMetadata;
		
		public ExtensionStatisticsBean(string extension, int total, int hasMetadata) : base()
		{
			this.extension = extension;
			this.total = total;
			this.hasMetadata = hasMetadata;
		}
		
		public string Extension
		{
			get { return extension; }
		}

		public int Total
		{
			get { return total; }
		}

		public int HasMetadata
		{
			get { return hasMetadata; }
		}
	}
}
