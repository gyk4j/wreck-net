
using System;
using System.Collections.Generic;
using System.IO;

using Java.NIO.File;
using log4net;
using Wreck.IO;
using Wreck.Resources;
using Wreck.Util.Logging;

namespace Wreck.IO
{
	/// <summary>
	/// Description of CliVisitor.
	/// </summary>
	public class CliVisitor : SimpleFileVisitor
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(CliVisitor));
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		CliWorker progressWorker = null;
		
		private readonly List<DirectoryInfo> directories;
		private readonly Dictionary<CorrectionEnum, DateTime> suggestions;
		
		public CliVisitor(CliWorker progressWorker) : base()
		{
			this.progressWorker = progressWorker;
			this.directories = new List<DirectoryInfo>();
			this.suggestions = new Dictionary<CorrectionEnum, DateTime>();
		}
		
		public List<DirectoryInfo> Directories { get { return directories; } }
		public Dictionary<CorrectionEnum, DateTime> Suggestions { get { return suggestions; } }
	}
}
