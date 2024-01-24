﻿
using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using log4net;
using log4net.Config;

namespace Wreck.Logging
{
	/// <summary>
	/// Description of Logger.
	/// </summary>
	public class Logger : ILogger
	{
		private const string LOG4NET_XML = "log4net.xml";
		
		private static readonly ILog log = LogManager.GetLogger(typeof(Logger));
		
		// TODO: Shift to controller?
		private MainForm form;
		
		// TODO: Initialize with controller instead?
		public Logger(MainForm form)
		{
			//BasicConfigurator.Configure();
			XmlConfigurator.Configure(new System.IO.FileInfo(LOG4NET_XML));
			
			this.form = form;
		}
		
		public void Version()
		{
			form.Text = String.Format("{0} v{1}", Wreck.NAME, Wreck.VERSION);
		}
		
		public void UnknownPathType(string path)
		{
			form.UnknownPathType(path);
		}
		
		public void CurrentPath(string p)
		{
			log.DebugFormat("P {0}", p);
			
//			TODO: Shift to controller?
			if(form.BackgroundWorker != null)
				form.BackgroundWorker.ReportProgress(0, p);
		}
		
		public void CurrentFile(FileInfo f)
		{
			log.DebugFormat("F     - {0}", f.Name);
			
//			TODO: Shift to controller?
			if(form.BackgroundWorker != null)
				form.BackgroundWorker.ReportProgress(0, f);
		}
		
		public void CurrentDirectory(DirectoryInfo d)
		{
			log.DebugFormat("D   - {0}", d.FullName);
			
//			TODO: Shift to controller?
			if(form.BackgroundWorker != null)
				form.BackgroundWorker.ReportProgress(0, d);
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
			form.SkipReparsePoint(d);
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
			form.SkipReparsePoint(f);
		}
		
		// For Corrector
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
			form.CorrectedByLastWriteMetadata(fsi, lastWrite);
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
			form.CorrectedByCreationMetadata(fsi, creation);
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
			form.CorrectedByLastAccessMetadata(fsi, lastAccess);
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
			form.CorrectedByLastWriteTime(fsi, creationOrLastAccess);
		}
		
		public void Statistics(Statistics stats)
		{
//			TODO: Shift to controller?
			if(form.BackgroundWorker != null)
				form.BackgroundWorker.ReportProgress(0, stats);
		}
		
		// For error reporting
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
			form.UnauthorizedAccessException(ex);
		}
	}
}
