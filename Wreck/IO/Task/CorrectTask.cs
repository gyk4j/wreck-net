
using System;
using System.Collections.Generic;
using System.IO;

using Wreck.IO.Writer;
using Wreck.Resources;

namespace Wreck.IO.Task
{
	/// <summary>
	/// Description of CorrectTask.
	/// </summary>
	public class CorrectTask : AnalyzeTask
	{
		public CorrectTask(
			FileSystemInfo startPath,
			Dictionary<SourceEnum, bool> sources,
			DateTime customDateTime,
			Dictionary<CorrectionEnum, bool> corrections) :
			base(startPath, sources, customDateTime, corrections)
		{
		}
		
		protected override ITimestampWriter[] Writers
		{
			get
			{
				return new ITimestampWriter[]
				{
					WriterFactory.BasicFileAttributesWriter,
				};
			}
		}
	}
}
