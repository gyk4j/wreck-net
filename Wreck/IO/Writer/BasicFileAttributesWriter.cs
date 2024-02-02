
using System;
using System.Collections.Generic;
using System.IO;

using log4net;
using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.IO.Writer
{
	/// <summary>
	/// Description of BasicFileAttributesWriter.
	/// </summary>
	public class BasicFileAttributesWriter : AbstractTimestampWriter
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(BasicFileAttributesWriter));
		/*
		public BasicFileAttributesWriter()
		{
		}
		 */
		
		public override void Write(
			FileSystemInfo file,
			Dictionary<CorrectionEnum, DateTime> values)
		{
			
			DateTime? creation = WriteAttribute(
				FileEvent.CorrectibleCreation,
				FileEvent.UncorrectibleCreation,
				file.CreationTime,
				values[CorrectionEnum.CREATION]);
			
			DateTime? modified = WriteAttribute(
				FileEvent.CorrectibleModified,
				FileEvent.UncorrectibleModified,
				file.LastWriteTime,
				values[CorrectionEnum.MODIFIED]);
			
			DateTime? accessed = WriteAttribute(
				FileEvent.CorrectibleAccessed,
				FileEvent.UncorrectibleAccessed,
				file.LastAccessTime,
				values[CorrectionEnum.ACCESSED]);
			
			if(creation.HasValue)
				file.CreationTime = creation.Value;
			
			if(modified.HasValue)
				file.LastWriteTime = modified.Value;
			
			if(accessed.HasValue)
				file.LastAccessTime = accessed.Value;
			
			LOG.InfoFormat("Updated {0}: {1}, {2}, {3}",
			         file.FullName,
			         creation.Value,
			         modified.Value,
			         accessed.Value);
		}
	}
}
