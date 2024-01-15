
using System;
using System.Collections.Generic;
using System.IO;

using log4net;
using Wreck.Resources;
using Wreck.Time;
using Wreck.Util.Logging;

namespace Wreck.IO.Writer
{
	/// <summary>
	/// Description of AbstractTimestampWriter.
	/// </summary>
	public abstract class AbstractTimestampWriter : ITimestampWriter
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(AbstractTimestampWriter));
//		private static readonly StatisticsCollector STATS = StatisticsCollector.getInstance();
		
		protected DateTime WriteAttribute(
			FileEvent required,
			FileEvent none,
			DateTime fileSystem,
			DateTime metadata)
		{
			DateTime? val = null;
			
			DateTime correct = metadata;
			
			if (correct == null)
			{
//				STATS.count(none);
			}
			else if(TimeUtils.IsLaterThan(fileSystem, correct))
			{
//				STATS.count(required);
				val = correct;
			}
			return val.Value;
		}
		
		protected DateTime RestoreAttribute(
			FileEvent required,
			FileEvent none,
			DateTime fileSystem,
			DateTime metadata)
		{
			
			if (metadata == null)
			{
//				STATS.count(none);
				LOG.Warn("Missing correct value");
			}
			else if(!TimeUtils.IsSameTime(fileSystem, metadata))
			{
//				STATS.count(required);
			}
			
			return metadata;
		}
		
		public abstract void Write(FileSystemInfo file, Dictionary<CorrectionEnum, DateTime> values);
	}
}
