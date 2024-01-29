
using System;
using System.Diagnostics;
using System.IO;

using log4net;
using Wreck.Controller;
using Wreck.Logging;

namespace Wreck.View
{
	/// <summary>
	/// Description of ConsoleView.
	/// </summary>
	public class ConsoleView
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ConsoleView));
		private IController controller;
		private Logger logger;
		
		private const string APP_STATE_IDLE = "Idle";
		private const string APP_STATE_RUNNING = "Running";
		
		public ConsoleView()
		{
			log.Debug("Initializing ConsoleView");
			
			this.controller = new CliController(this);
			
			this.logger = new Logger();
			logger.Version();
			
			SetAppState(false);
			
			log.Debug("Initialized ConsoleView");
		}
		
		public void Run(string[] args)
		{
			((CliController) controller).Analyze(args);
		}
		
		public void Version()
		{
			System.Console.Title = String.Format("{0} v{1}", Wreck.NAME, Wreck.VERSION);
		}
		
		public void UnknownPathType(string path)
		{
			log.FatalFormat("UnknownPathType: {0}", path);
		}
		
		private void CurrentPath(string p)
		{
			log.InfoFormat("{0}", p);
			SetCurrentFile(p);
		}
		
		private void CurrentFile(FileInfo fi)
		{
			log.InfoFormat("    - {0}", fi.Name);
			SetCurrentFile(fi.Name);
		}
		
		private void CurrentDirectory(DirectoryInfo di)
		{
			log.InfoFormat("  + {0}", di.FullName);
			SetCurrentFile(di.Name);
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
			log.WarnFormat("Skipped reparse point: {0}", d.Name);
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
			log.WarnFormat("Skipped reparse point: {0}", f.Name);
		}
		
		// For Corrector
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
			log.DebugFormat("MW: {0} : {1} -> {2}", fsi.Name, fsi.LastWriteTime, lastWrite);
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
			log.DebugFormat("MC: {0} : {1} -> {2}", fsi.Name, fsi.CreationTime, creation);
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
			log.DebugFormat("MA: {0} : {1} -> {2}", fsi.Name, fsi.LastAccessTime, lastAccess);
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
			if(creationOrLastAccess == fsi.CreationTime)
				log.DebugFormat("LC: {0} : {1} -> {2}", fsi.Name, fsi.CreationTime, fsi.LastWriteTime);
			else if(creationOrLastAccess == fsi.LastAccessTime)
				log.DebugFormat("LA: {0} : {1} -> {2}", fsi.Name, fsi.LastAccessTime, fsi.LastWriteTime);
		}
		
		private void Statistics(Statistics stats)
		{
			log.InfoFormat("Directories: {0}", stats.Directories);
			log.InfoFormat("Files: {0}", stats.Files);
			log.InfoFormat("Skipped: {0}", stats.Skipped);
		}
		
		public void SetCurrentFile(string p)
		{
			log.Info(p);
		}
		
		private void SetAppState(bool running)
		{
			string text = running ? APP_STATE_RUNNING : APP_STATE_IDLE;
			string title;
			int i = System.Console.Title.IndexOf(" - ");
			title = i > 0 ?
				System.Console.Title.Substring(0, i) :
				title = System.Console.Title;
			
			title = title + " - " + text;
			System.Console.Title = title;
		}
		
		// For error reporting
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
			log.ErrorFormat("Unauthorized Access: {0}",
			                ex.ToString());
		}
	}
}
