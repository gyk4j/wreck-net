
using System;
using System.Collections.Generic;
using System.IO;

using log4net;
using Wreck.IO.Reader;
using Wreck.IO.Reducer;
using Wreck.IO.Writer;
using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.IO
{
	/// <summary>
	/// Description of MetadataBuilder.
	/// </summary>
	public class MetadataBuilder
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(MetadataBuilder));
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;

		private readonly List<ITimestampReader> readers;
		private readonly List<ITimestampWriter> writers;
		private readonly ITimestampReducer reducer;
		private readonly List<Metadata> metadata;
		private readonly Dictionary<CorrectionEnum, DateTime> suggestions;
		
		public MetadataBuilder()
		{
			readers = new List<ITimestampReader>();
			writers = new List<ITimestampWriter>();
			metadata = new List<Metadata>();
			reducer = new TimestampReducer();
			suggestions = new Dictionary<CorrectionEnum, DateTime>();
		}
		
		private List<ITimestampReader> Readers
		{
			get { return readers; }
		}
		
		private List<ITimestampWriter> Writers
		{
			get { return writers; }
		}
		
		private ITimestampReducer Reducer
		{
			get { return reducer; }
		}
		
		public List<Metadata> Metadata
		{
			get { return metadata; }
		}
		
		public Dictionary<CorrectionEnum, DateTime> Suggestions
		{
			get { return suggestions; }
		}
		
		public bool IsIncomplete()
		{
			if(suggestions.Count == 0)
				return true;
			
			foreach(CorrectionEnum c in CorrectionEnum.Values)
			{
				if(!suggestions.ContainsKey(c))
					return true;
			}
			
			return false;
		}
		
		public void AddReader(ITimestampReader reader)
		{
			Readers.Add(reader);
		}
		
		public void AddWriter(ITimestampWriter writer)
		{
			Writers.Add(writer);
		}
		
		public void Process(FileSystemInfo file)
		{
			foreach(ITimestampReader reader in Readers)
			{
				int prev = metadata.Count;
				reader.Extract(file, metadata);
				
				for(int t=prev; t < metadata.Count; t++)
					STATS.Count(new TagEvent(reader, metadata[t].Key));
			}
		}
		
		public void Save(FileSystemInfo file)
		{
			if(Metadata.Count == 0)
				LOG.WarnFormat("Metadata empty!!! {0}...", file.FullName);
			
			Reducer.Reduce(Metadata, Suggestions);
			foreach(ITimestampWriter writer in Writers)
			{
				writer.Write(file, Suggestions);
			}
			
			STATS.Count(new ExtensionEvent(file.FullName, !IsIncomplete()));
		}
	}
}
