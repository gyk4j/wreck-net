
using System;
using System.Collections.Generic;
using System.IO;

using log4net;
using Wreck.Resources;

namespace Wreck.IO.Reader.Fs
{
	/// <summary>
	/// Description of DirectoryReader.
	/// </summary>
	public class DirectoryReader : AbstractTimestampReader
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(DirectoryReader));
		
		protected static readonly string[] EARLIEST =
		{
			R.strings.DIR_EARLIEST
		};
		
		protected static readonly string[] LATEST =
		{
			R.strings.DIR_LATEST
		};
		
		public override string[] Creation()
		{
			return EARLIEST;
		}

		public override string[] Modified()
		{
			return LATEST;
		}

		public override string[] Accessed()
		{
			return LATEST;
		}
		
		/// <remarks>
		/// Confirmed that DirectoryInfo *does not* override Equals and HashCode
		/// which would two DirectoryInfo instances with the same path would
		/// be deemed as equal, thereby serving as a key.
		/// https://stackoverflow.com/questions/24661161/can-a-directoryinfo-object-be-safely-use-as-a-key-in-a-dictionary
		/// </remarks>
		private readonly IDictionary<string, List<DateTime>> fileTimes = new Dictionary<string, List<DateTime>>();

		public void Add(DirectoryInfo dir, IDictionary<CorrectionEnum, DateTime> suggestions)
		{
			List<DateTime> times;
			bool exist = fileTimes.TryGetValue(dir.FullName, out times);
			if(!exist)
			{
				times = new List<DateTime>();
				fileTimes.Add(dir.FullName, times);
			}
			times.AddRange(suggestions.Values);
			times.RemoveAll(t => t == default(DateTime));
		}
		
		public override void Extract(FileSystemInfo file, List<Metadata> metadata)
		{
			DirectoryInfo dir = (DirectoryInfo) file;
			List<DateTime> times = null;
			bool exist = fileTimes.TryGetValue(dir.FullName, out times);
			fileTimes.Remove(dir.FullName);
			
			if(!exist || times.Count == 0)
			{
				// No files or sub-directories found.
//				LOG.WarnFormat("Empty directory? {0}", dir.FullName);
				return;
			}
			
			times.Sort();
			
			DateTime min = times[0];
			DateTime max = times[times.Count-1];
			
//			if(min.HasValue)
			Add(metadata, EARLIEST[0], min.ToString(), min);
			
//			if(max.HasValue)
			Add(metadata, LATEST[0], max.ToString(), max);
		}
	}
}
