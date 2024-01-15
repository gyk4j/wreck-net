
using System;
using System.Collections.Generic;
using System.IO;

using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.IO.Writer
{
	/// <summary>
	/// Description of AnalyzeWriter.
	/// </summary>
	public class AnalyzeWriter : AbstractTimestampWriter
	{
		public override void Write(
			FileSystemInfo file,
			Dictionary<CorrectionEnum, DateTime> values)
		{
			
			WriteAttribute(
				FileEvent.CorrectibleCreation,
				FileEvent.UncorrectibleCreation,
				file.CreationTime,
				values[CorrectionEnum.CREATION]);
			
			WriteAttribute(
				FileEvent.CorrectibleModified,
				FileEvent.UncorrectibleModified,
				file.LastWriteTime,
				values[CorrectionEnum.MODIFIED]);
			
			WriteAttribute(
				FileEvent.CorrectibleAccessed,
				FileEvent.UncorrectibleAccessed,
				file.LastAccessTime,
				values[CorrectionEnum.ACCESSED]);
		}
	}
}
