
using System;
using System.Collections.Generic;

namespace Wreck.Util.Logging
{
	/// <summary>
	/// Description of StatisticsCollector.
	/// </summary>
	public class StatisticsCollector
	{
		private static StatisticsCollector instance;
		
		private readonly IDictionary<FileEvent, int> statistics = new SortedDictionary<FileEvent, int>();
		private readonly IDictionary<string, int> metadataKeys = new SortedDictionary<string, int>();
		private readonly IDictionary<string, ExtensionStatistics> fileExtensions = new SortedDictionary<string, ExtensionStatistics>();
//		private readonly IDictionary<SelectionEvent, int> selection = new SortedDictionary<SelectionEvent, int>();
		
		private StatisticsCollector() : base()
		{
		}
		
		public static StatisticsCollector Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new StatisticsCollector();
				}
				
				return instance;
			}
		}
		
		public IDictionary<FileEvent, int> Statistics
		{
			get { return statistics; }
		}
		
		public int Get(FileEvent e) {
			int val;
			bool ok = statistics.TryGetValue(e, out val);
			return ok ? val : 0;
		}
		
		public int Count(FileEvent e)
		{
			int val;
			bool ok = statistics.TryGetValue(e, out val);
			
			val = ok ? ++val : 1;
			statistics[e] = val;
			
			return val;
		}
		
		/*
		private float GetFileCorrectedCreationPercentage()
		{
			return ((float) get(LogEvent.CorrectedCreation) / (float) get(LogEvent.FileFound))*100;
		}
		
		private float GetFileCorrectedLastModifiedPercentage()
		{
			return ((float) get(LogEvent.CorrectedModified) / (float) get(LogEvent.FileFound))*100;
		}
	
		private float GetFileCorrectedLastAccessedPercentage()
		{
			return ((float) get(LogEvent.CorrectedAccessed) / (float) get(LogEvent.FileFound))*100;
		}
		 */

		public IDictionary<string, int> MetadataKeys
		{
			get { return metadataKeys; }
		}
		
		public int Get(TagEvent e)
		{
			int val;
			bool ok = metadataKeys.TryGetValue(e.ToString(), out val);
			return ok ? val : 0;
		}
		
		public int Count(TagEvent e)
		{
			int val;
			bool ok = metadataKeys.TryGetValue(e.ToString(), out val);
			
			val = ok ? ++val : 1;
			metadataKeys[e.ToString()] = val;
			
			return val;
		}
		
		public IDictionary<string, ExtensionStatistics> FileExtensions
		{
			get { return fileExtensions; }
		}
		
		public void Count(ExtensionEvent e)
		{
			ExtensionStatistics ov, nv = new ExtensionStatistics(e.File, e.HasMetadata);
			bool ok = fileExtensions.TryGetValue(e.ToString(), out ov);
			
			if(ok)
			{
				ov.AddTotal();

				if(nv.HasMetadata > 0)
					ov.AddMetadata();
			}
			else
				ov = nv;
			
			fileExtensions[e.ToString()] = ov;
		}
		
//		public IDictionary<SelectionEvent, int> Selection
//		{
//			get { return selection; }
//		}
//
//		public int Count(SelectionEvent e)
//		{
//			int val;
//			bool ok = selection.TryGetValue(e, out val);
//
//			val = ok ? ++val : 1;
//			selection[e] = val;
//
//			return val;
//		}
//
//		public int Get(SelectionEvent e)
//		{
//			int val;
//			bool found = selection.TryGetValue(e, out val);
//			return found ? val : 0;
//		}
		
		public void Reset()
		{
			statistics.Clear();
			metadataKeys.Clear();
			fileExtensions.Clear();
//			selection.Clear();
		}
	}
}
