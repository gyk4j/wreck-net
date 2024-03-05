
using System;
using System.Collections.Generic;
using System.IO;

using log4net;
using Wreck.IO.Reader;
using Wreck.IO.Writer;
using Wreck.Logging;
using Wreck.Resources;
using Wreck.Service;
using Wreck.Util.Logging;

namespace Wreck.Controller
{
	/// <summary>
	/// Description of AbstractController.
	/// </summary>
	public abstract class AbstractController : IController
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(AbstractController));
		
		private static readonly StatisticsCollector STATS = StatisticsCollector.Instance;
		
		protected FileSystemInfo startPath;
		
		private readonly WreckService service;
		
		protected static IController instance;
		
		protected AbstractController()
		{
			this.service = new WreckService();
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
		
		public FileSystemInfo StartPath
		{
			get { return startPath; }
		}
		
		public WreckService Service
		{
			get { return service; }
		}
		
		public virtual void Error()
		{
			Console.Error.WriteLine(startPath + " is invalid.");
		}
		
		public virtual void Start(string startPath)
		{
			if(Directory.Exists(startPath))
				this.startPath =  new DirectoryInfo(startPath);
			else if(File.Exists(startPath))
				this.startPath = new FileInfo(startPath);
			else
				Error();
			
			if(this.startPath != null)
			{
				Init();
//				View.GetPreviewDialog().Open();
			}
		}
		
		public virtual void Stop()
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
		
		protected virtual void Init()
		{
			string title = startPath.FullName + " - " + R.Strings.AppTitle;
			LOG.InfoFormat("{0}\n", title);
		}
		
		public void Version()
		{
			LOG.InfoFormat("{0}\n", R.Strings.AppTitle);
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
		
		public virtual void Analyze()
		{
			Run(CorrectionMode.Analyze);
		}
		
		public virtual void Backup()
		{
			Run(CorrectionMode.BackupAttributes);
		}
		
		public virtual void Repair()
		{
			Run(CorrectionMode.SaveAttributes);
		}
		
		public virtual void Restore()
		{
			Run(CorrectionMode.RestoreAttributes);
		}
		
		public virtual void Verify()
		{
			Run(CorrectionMode.VerifyAttributes);
		}
		
		private void Run(CorrectionMode mode)
		{
			string[] args = Environment.GetCommandLineArgs();
			string[] dirs;
			
			if(args.Length > 1)
			{
				dirs = new string[args.Length-1];
				Array.Copy(args, 1, dirs, 0, args.Length-1);
			}
			else
			{
				dirs = new string[0];
				Error();
			}
			
			foreach(string d in dirs)
			{				
				if(Directory.Exists(d))
					Run(mode, new DirectoryInfo(d));
				else if(File.Exists(d))
					Run(mode, new FileInfo(d));
				else
					LOG.ErrorFormat("Unknown path type: {0}", d);
			}
		}
		
		public virtual void Run(CorrectionMode mode, FileSystemInfo fsi)
		{
			LOG.InfoFormat("Mode: {0}, File: {1}", 
			               Enum.GetName(typeof(CorrectionMode), mode), 
			               fsi.FullName);
			throw new NotImplementedException();
		}
		
		// Update statistics
		
		protected virtual void UpdateStatistics()
		{
			foreach(KeyValuePair<FileEvent, int> entry in STATS.Statistics)
			{
				FileEvent name = entry.Key;
				int count = entry.Value;
//				PrintKeyValueColumns(name, count);
//				model.GetFileStatisticsTableModel().AddRow(
//					new FileStatisticsBean(name.ToString(), count));
			}
			
//			PrintKeyValueColumns("Dir  Found", STATS.Get(FileEvent.DirectoryFound));
//			PrintKeyValueColumns("File Found", STATS.Get(FileEvent.FileFound));
//			PrintKeyValueColumns("File Error", STATS.Get(FileEvent.FileError));
//			PrintKeyValueColumns("Fixed Creation", STATS.Get(FileEvent.CorrectedCreation));
//			PrintKeyValueColumns("Fixed Modified", STATS.Get(FileEvent.CorrectedModified));
//			PrintKeyValueColumns("Fixed Accessed", STATS.Get(FileEvent.CorrectedAccessed));
			
			foreach(KeyValuePair<string, int> entry in STATS.MetadataKeys)
			{
				string name = entry.Key;
				int count = entry.Value;
				
//				PrintKeyValueColumns(name, count);
				
				string parser, tag;
				string[] pair = name.Split(new char[]{ ':' }, 2);
				parser = pair[0];
				tag = pair[1];
//				model.GetMetadataStatisticsTableModel().AddRow(
//					new MetadataStatisticsBean(parser, tag, count));
			}
			
			foreach(KeyValuePair<string, ExtensionStatistics> entry in STATS.FileExtensions)
			{
				string name = entry.Key;
				ExtensionStatistics count = entry.Value;
				
//				PrintKeyValueColumns(extension, count);
//				model.GetExtensionStatisticsTableModel().AddRow(
//					new ExtensionStatisticsBean(extension, count.GetTotal(), count.GetHasMetadata()));
			}
		}
		
		public virtual void Done()
		{			
			Stop();
			UpdateStatistics();
			
			/*
			LOG.InfoFormat("Metadata: {0}, Last Modified: {1}, Custom: {2}",
				STATS.Get(SelectionEvent.Metadata),
				STATS.Get(SelectionEvent.LastModified),
				STATS.Get(SelectionEvent.Custom));
			 */
		}
	}
}
