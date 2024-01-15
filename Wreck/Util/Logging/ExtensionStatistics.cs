
using System;
using System.IO;

namespace Wreck.Util.Logging
{
	/// <summary>
	/// Description of ExtensionStatistics.
	/// </summary>
	public class ExtensionStatistics : IComparable<ExtensionStatistics>
	{
		private string id;
		private int total;
		private int hasMetadata;
		
		public static string GetExtensionKey(string file)
		{
			string filename = Path.GetFileName(file);
			String extension = Path.GetExtension(file);
			
			if(string.IsNullOrEmpty(extension))
			{
				return string.Empty;
			}
			
			extension = extension.ToLower();
			return extension;
		}
		
		public ExtensionStatistics(string id, bool hasMetadata) : base()
		{
			string path = Path.GetFullPath(id);
			
			if(string.IsNullOrEmpty(path))
			{
				this.id = id;
				this.total = 1;
				this.hasMetadata = (hasMetadata)? 1: 0;
			}
		}
		
		public ExtensionStatistics(FileSystemInfo file, bool hasMetadata) : 
			this(GetExtensionKey(file.FullName), hasMetadata)
		{
			
		}

		public string Id
		{
			get { return id; }
		}

		public int Total
		{
			get { return total; }
		}
		
		public int AddTotal()
		{
			return ++total;
		}

		public int HasMetadata
		{
			get { return hasMetadata; }
		}
		
		public int AddMetadata()
		{
			return ++hasMetadata;
		}

		public override string ToString()
		{
			return string.Format("{0:-10} {1:D6} {2:D6} {3:D6}", id, total, hasMetadata, total-hasMetadata);
		}
		
		public int CompareTo(ExtensionStatistics o)
		{
			if(Total > o.Total)
				return -1;
			else if(Total == o.Total)
				return 0;
			else
				return 1;
		}		
	}
}
