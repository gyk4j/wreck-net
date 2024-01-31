
using System;
using System.IO;
using Java.NIO.File;
using log4net;
using Wreck.IO.Task;
using Wreck.Util.Logging;

namespace Wreck.IO
{
	/// <summary>
	/// Description of CliWorker.
	/// </summary>
	public class CliWorker
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(CliWorker));
		
		private readonly ITask task;
		private readonly FileSystemInfo startPath;
		
		private readonly CliVisitor visitor;
		
		public CliWorker(ITask task, FileSystemInfo startPath)
		{
			this.task = task;
			this.startPath = startPath;
			this.visitor = new CliVisitor(this);
		}
		
		public ITask Task { get { return task; } }
		private FileSystemInfo StartPath { get { return startPath; } }
		private AbstractFileVisitor Visitor { get { return visitor; } }
		
		public void Run()
		{
			Files.WalkFileTree(
				StartPath,
				Visitor);
		}
	}
}
