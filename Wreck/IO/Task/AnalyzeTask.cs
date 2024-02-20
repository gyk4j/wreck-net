
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Java;
using log4net;
using Microsoft.NET;
using Wreck.IO.Reader;
using Wreck.IO.Reader.User;
using Wreck.IO.Writer;
using Wreck.Resources;

namespace Wreck.IO.Task
{
	/// <summary>
	/// Description of AnalyzeTask.
	/// </summary>
	public class AnalyzeTask : ITask
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(AnalyzeTask));
		
		// Sources
		private readonly Dictionary<SourceEnum, bool> sources;
		private readonly DateTime customDateTime;
		
		// Targets
		private readonly Dictionary<CorrectionEnum, bool> corrections;
		
		private readonly ReaderFactory readerFactory;
		private readonly CustomDateTimeReader customReader;
		
		private readonly WriterFactory writerFactory;
		
		public AnalyzeTask(
			FileSystemInfo startPath,
			Dictionary<SourceEnum, bool> sources,
			DateTime customDateTime,
			Dictionary<CorrectionEnum, bool> corrections
		)
		{
			this.sources = sources;
			this.customDateTime = customDateTime;
			
			this.corrections = corrections;
			
			readerFactory = ReaderFactory.Instance;
			customReader = readerFactory.CustomDateTimeReader;
			customReader.Custom = customDateTime;
			
			writerFactory = WriterFactory.Instance;
		}
		
		protected IDictionary<SourceEnum, bool> Sources
		{
			get { return sources; }
		}
		
		protected DateTime CustomDateTime
		{
			get { return customDateTime; }
		}

		protected IDictionary<CorrectionEnum, Boolean> Corrections
		{
			get { return corrections; }
		}
		
		protected ReaderFactory ReaderFactory
		{
			get { return readerFactory; }
		}
		
		protected WriterFactory WriterFactory
		{
			get { return writerFactory; }
		}
		
		protected virtual ITimestampWriter[] Writers
		{
			get
			{
				return new ITimestampWriter[]
				{
					WriterFactory.AnalyzeWriter
				};
			}
		}
		
		protected virtual ITimestampReader[] FileReaders
		{
			get
			{
				List<ITimestampReader> readers = new List<ITimestampReader>();
				if(sources.ContainsKey(SourceEnum.METADATA))
				{
					readers.Add(readerFactory.ExifToolReader);
					readers.Add(readerFactory.MediaInfoReader);
					readers.Add(readerFactory.SevenZipReader);
//					readers.Add(readerFactory.TikaReader);
					readers.Add(readerFactory.PathReader);
				}
				
				// Use fallback option.
				if(sources.ContainsKey(SourceEnum.FILE_SYSTEM))
					readers.Add(readerFactory.FileSystemReader);

				if(sources.ContainsKey(SourceEnum.CUSTOM))
					readers.Add(customReader);
				
				return readers.ToArray();
			}
		}
		
		protected virtual ITimestampReader[] DirectoryReaders
		{
			get
			{
				List<ITimestampReader> readers = new List<ITimestampReader>();
				
				if(sources.ContainsKey(SourceEnum.METADATA))
					readers.Add(readerFactory.DirectoryReader);
				
				// Use fallback option.
				if(sources.ContainsKey(SourceEnum.FILE_SYSTEM))
					readers.Add(readerFactory.FileSystemReader);

				if(sources.ContainsKey(SourceEnum.CUSTOM))
					readers.Add(customReader);
				
				return readers.ToArray();
			}
		}
		
		private IDictionary<CorrectionEnum, DateTime> Process(
			FileSystemInfo path,
			ITimestampReader[] readers,
			ITimestampWriter[] writers)
		{
			
			MetadataBuilder mb = new MetadataBuilder();
			
			foreach(ITimestampReader reader in readers)
			{
				mb.AddReader(reader);
			}
			
			foreach(ITimestampWriter writer in writers)
			{
				mb.AddWriter(writer);
			}
			
			mb.Process(path);
			
			mb.Save(path);
			
			return mb.Suggestions;
		}
		
		private IDictionary<CorrectionEnum, DateTime> Analyze(FileSystemInfo path)
		{
			IDictionary<CorrectionEnum, DateTime> suggestions = null;
			
			ITimestampWriter[] writers = Writers;
			
			// Use embedded or inferred metadata as primary source.
			ITimestampReader[] readers;
			
			if(!FSUtils.IsDirectory(path) && !FSUtils.IsReparsePoint(path))
				readers = FileReaders;
			else if(FSUtils.IsDirectory(path))
				readers = DirectoryReaders;
			else
				readers = new ITimestampReader[0];
			
			suggestions = Process(
				path,
				readers,
				writers);
			
			DirectoryInfo parent = null;
			if(path is FileInfo)
				parent = ((FileInfo) path).Directory;
			else if(path is DirectoryInfo)
				parent = ((DirectoryInfo) path).Parent;
			else
				LOG.ErrorFormat("Unknown type: {0}", path.FullName);
			
			Debug.Assert(parent != null);
			
			readerFactory.DirectoryReader.Add(parent, suggestions);
			
			LOG.DebugFormat("{0} C: {1}, M: {2}, A: {3} {4}",
			         !FSUtils.IsDirectory(path) && !FSUtils.IsReparsePoint(path)? "<F>": "[D]",
			         suggestions[CorrectionEnum.CREATION],
			         suggestions[CorrectionEnum.MODIFIED],
			         suggestions[CorrectionEnum.ACCESSED],
			         path.Name);

			return suggestions;
		}
		
		public virtual void PreVisitDirectory(Dictionary<CorrectionEnum, DateTime> suggestions, DirectoryInfo dir)
		{
			
		}
		
		public virtual void PostVisitDirectory(Dictionary<CorrectionEnum, DateTime> suggestions, DirectoryInfo dir)
		{
			IDictionary<CorrectionEnum, DateTime> metadata = Analyze(dir);
			IEnumerator<KeyValuePair<CorrectionEnum, DateTime>> e = metadata.GetEnumerator();
			while(e.MoveNext())
			{
				suggestions.Add(e.Current.Key, e.Current.Value);
			}
		}
		
		public virtual void VisitFile(Dictionary<CorrectionEnum, DateTime> suggestions, FileInfo file)
		{
			IDictionary<CorrectionEnum, DateTime> metadata = Analyze(file);
			IEnumerator<KeyValuePair<CorrectionEnum, DateTime>> e = metadata.GetEnumerator();
			while(e.MoveNext())
			{
				if(!suggestions.ContainsKey(e.Current.Key))
					suggestions.Add(e.Current.Key, e.Current.Value);
				else
				{
					LOG.WarnFormat("{0}: Overwrite {1} with {2}",
					               e.Current.Key.Name,
					               suggestions[e.Current.Key],
					               e.Current.Value);
					suggestions[e.Current.Key] = e.Current.Value;
				}
			}
		}
		
		public virtual void VisitFileFailed(Dictionary<CorrectionEnum, DateTime> suggestions, FileSystemInfo file, IOException exc)
		{
			LOG.ErrorFormat("VisitFileFailed: {0}", file.FullName);
		}
	}
}
