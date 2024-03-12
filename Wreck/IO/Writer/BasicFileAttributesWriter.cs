
using System;
using System.Collections.Generic;
using System.IO;

using Java.NIO.File;
using Java.NIO.File.Attribute;
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
		
		public override void Write(
			FileSystemInfo file,
			Dictionary<CorrectionEnum, DateTime> values)
		{
			// TODO: To be removed after tracing.
			LOG.DebugFormat("--- {0} ---", file.FullName);
			Logging.Dumper.Dump(values);
			// TODO: To be removed after tracing.
			
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
			
			BasicFileAttributeView basicView = Files.GetFileAttributeView<BasicFileAttributeView>(file, typeof(BasicFileAttributeView));
			try
			{
				basicView.SetTimes(modified, accessed, creation);
				LOG.DebugFormat("Updated {0}: {1}, {2}, {3}",
				                file.FullName,
				                creation.ToString(),
				                modified.ToString(),
				                accessed.ToString());
			}
			catch (IOException e)
			{
				LOG.Error(e.Message);
			}
		}
	}
}
