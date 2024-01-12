
using System;
using System.Collections.Generic;
using System.IO;

using log4net;
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
		
		public void PreVisitDirectory(Dictionary<CorrectionEnum, DateTime> suggestions, DirectoryInfo dir)
		{
			throw new NotImplementedException();
		}
		
		public void PostVisitDirectory(Dictionary<CorrectionEnum, DateTime> suggestions, DirectoryInfo dir)
		{
			throw new NotImplementedException();
		}
		
		public void VisitFile(Dictionary<CorrectionEnum, DateTime> suggestions, FileInfo file)
		{
			throw new NotImplementedException();
		}
		
		public void VisitFileFailed(Dictionary<CorrectionEnum, DateTime> suggestions, FileSystemInfo file, IOException exc)
		{
			throw new NotImplementedException();
		}
	}
}
