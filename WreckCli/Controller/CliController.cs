
using System;
using System.Collections.Generic;
using System.IO;

using Java.Beans;
using Javax.Swing;
using log4net;
using Wreck.Entity;
using Wreck.IO;
using Wreck.IO.Reader;
using Wreck.IO.Task;
using Wreck.IO.Writer;
using Wreck.Logging;
using Wreck.Model;
using Wreck.Resources;
using Wreck.Service;
using Wreck.Util.Logging;
using Wreck.View;

namespace Wreck.Controller
{
	/// <summary>
	/// Description of CliController.
	/// </summary>
	public class CliController : IController
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(CliController));
		
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		private FileSystemInfo startPath;
		private CliWorker worker;
		
		private readonly ConsoleModel model;
		private readonly ConsoleView view;
		private readonly PreviewService service;
		
		protected static CliController instance;
		
		public CliController(ConsoleView view)
		{
			this.model = new ConsoleModel();
			this.view = view;
			this.service = new PreviewService();
			instance = this;
		}
		
		public static IController Instance
		{
			get
			{
				if (instance == null)
					throw new NullReferenceException("CliController instance is uninitialized");
				return instance;
			}
		}
		
		public ConsoleModel Model
		{
			get { return model; }
		}
		
		public ConsoleView View
		{
			get { return view; }
		}
		
		public FileSystemInfo StartPath
		{
			get { return startPath; }
		}
		
		public CliWorker Worker
		{
			get { return worker; }
			set { worker = value; }
		}
		
		public void Start(string startPath)
		{
			if(Directory.Exists(startPath))
				this.startPath =  new DirectoryInfo(startPath);
			else if(File.Exists(startPath))
				this.startPath = new FileInfo(startPath);
			else
			{
				LOG.Error(startPath + " is invalid.");
				Environment.Exit(-1);
			}
			
			if(this.startPath != null)
			{
				Init();
//				View.GetPreviewDialog().Open();
			}
		}
		
		public void Stop()
		{
			LOG.Info("Stopping and cleaning up...");
			try
			{
				if(WriterFactory.IsInitialized())
				{
					LOG.Info("Closing writers...");
					WriterFactory.Instance.Dispose();
				}
				
				if(ReaderFactory.IsInitialized())
				{
					LOG.Info("Closing readers...");
					ReaderFactory.Instance.Dispose();
				}
			}
			catch (Exception e)
			{
				LOG.Error(e.ToString());
			}
		}
		
		private void Init()
		{
			string title = startPath.FullName + " - " + R.strings.APP_TITLE;
			LOG.InfoFormat("{0}\n", title);
		}
		
		public void Version()
		{
			LOG.InfoFormat("{0}\n", R.strings.APP_TITLE);
		}
		
		public void UnknownPathType(string path)
		{
			LOG.WarnFormat("UnknownPathType: {0}", path);
		}
		
		public void CurrentPath(string p)
		{
			LOG.InfoFormat("> {0}", p);
		}
		
		public void CurrentFile(FileInfo f)
		{
			LOG.InfoFormat("    - {0}", f.Name);
		}
		
		public void CurrentDirectory(DirectoryInfo d)
		{
			LOG.InfoFormat("  + {0}", d.FullName);
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
			LOG.InfoFormat("Skipped reparse point: {0}", d.Name);
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
			LOG.InfoFormat("Skipped reparse point: {0}", f.Name);
		}
		
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
			LOG.InfoFormat("        MW: {0}", TextFormatter.Format(fsi.LastWriteTime.Subtract(lastWrite)));
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
			LOG.InfoFormat("        MC: {0}", TextFormatter.Format(fsi.CreationTime.Subtract(creation)));
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
			LOG.InfoFormat("        MA: {0}", TextFormatter.Format(fsi.LastAccessTime.Subtract(lastAccess)));
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
			if(creationOrLastAccess == fsi.CreationTime)
				LOG.InfoFormat("        LC: {0}", TextFormatter.Format(fsi.CreationTime.Subtract(fsi.LastWriteTime)));
			else if(creationOrLastAccess == fsi.LastAccessTime)
				LOG.InfoFormat("        LA: {0}", TextFormatter.Format(fsi.LastAccessTime.Subtract(fsi.LastWriteTime)));
		}
		
		public void Statistics(Statistics stats)
		{
			LOG.InfoFormat("\n### Dirs: {0}, Files: {1}, Skipped: {2} ###\n",
			                  stats.Directories,
			                  stats.Files,
			                  stats.Skipped);
		}
		
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
			LOG.ErrorFormat(ex.ToString());
		}
		
		// Event Handlers
		
		public void Analyze(string[] args)
		{
			Run(CorrectionMode.Analyze, args);
		}
		
		public void Backup(string[] args)
		{
			Run(CorrectionMode.BackupAttributes, args);
		}
		
		public void Repair(string[] args)
		{
			Run(CorrectionMode.SaveAttributes, args);
		}
		
		public void Restore(string[] args)
		{
			Run(CorrectionMode.RestoreAttributes, args);
		}
		
		public void Verify(string[] args)
		{
			Run(CorrectionMode.VerifyAttributes, args);
		}

		private void Run(CorrectionMode mode, string[] args)
		{
			foreach(string path in args)
			{
				FileSystemInfo fsi;
				
				if(Directory.Exists(path))
					fsi = new DirectoryInfo(path);
				else if(File.Exists(path))
					fsi = new FileInfo(path);
				else
				{
					view.UnknownPathType(path);
					continue;
				}
				
				Dictionary<SourceEnum, bool> sources = new Dictionary<SourceEnum, bool>();
				foreach(SourceEnum s in SourceEnum.Values)
				{
					sources.Add(s, true);
				}
				
				Dictionary<CorrectionEnum, bool> corrections = new Dictionary<CorrectionEnum, bool>();
				foreach(CorrectionEnum c in CorrectionEnum.Values)
				{
					corrections.Add(c, true);
				}
				
				DateTime customDateTime = DateTime.Now;
				
				ITask task = service.Run(fsi, mode, sources, corrections, customDateTime);
				
				CliWorker pw = new CliWorker(task, fsi);
				pw.Run();
				Done();
			}
		}
		
		// Update statistics
		
		private void UpdateStatistics()
		{
			LOG.InfoFormat("Metadata: {0}, Last Modified: {1}, Custom: {2}",
				STATS.Get(SelectionEvent.Metadata),
				STATS.Get(SelectionEvent.LastModified),
				STATS.Get(SelectionEvent.Custom));
		}
		
		private void UpdateForecastStatistics()
		{
			// No charts to update.
		}
		
		private void UpdateRestoreState()
		{
			// No "Restore" button to enable or disable. 
		}
		
		public void Done()
		{
//			View.GetScanningDialog().Close();
			
			Stop();
			Worker = null;
			UpdateRestoreState();
			
			UpdateStatistics();
			UpdateForecastStatistics();
		}
	}
}
