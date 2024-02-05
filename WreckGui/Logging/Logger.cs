
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using log4net;
using log4net.Config;
using Wreck.Controller;

namespace Wreck.Logging
{
	/// <summary>
	/// Description of Logger.
	/// </summary>
	public class Logger : ILogger
	{
		// FIXME: Refactor AbstractController to implement ILogger instead.
		
		private const string LOG4NET_XML = "log4net.xml";
		
		private static readonly ILog log = LogManager.GetLogger(typeof(Logger));
		
		public Logger(MainForm form)
		{
			//BasicConfigurator.Configure();
//			XmlConfigurator.Configure(new System.IO.FileInfo(LOG4NET_XML));
		}
		
		public void Version()
		{
//			GuiController.Instance.Version();
		}
		
		public void UnknownPathType(string path)
		{
//			GuiController.Instance.UnknownPathType(path);
		}
		
		public void CurrentPath(string p)
		{
			log.DebugFormat("P {0}", p);
//			GuiController.Instance.CurrentPath(p);
		}
		
		public void CurrentFile(FileInfo f)
		{
			log.DebugFormat("F     - {0}", f.Name);
//			GuiController.Instance.CurrentFile(f);
		}
		
		public void CurrentDirectory(DirectoryInfo d)
		{
			log.DebugFormat("D   - {0}", d.FullName);
//			GuiController.Instance.CurrentDirectory(d);
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
//			GuiController.Instance.SkipReparsePoint(d);
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
//			GuiController.Instance.SkipReparsePoint(f);
		}
		
		// For Corrector
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
//			GuiController.Instance.CorrectedByLastWriteMetadata(fsi, lastWrite);
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
//			GuiController.Instance.CorrectedByCreationMetadata(fsi, creation);
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
//			GuiController.Instance.CorrectedByLastAccessMetadata(fsi, lastAccess);
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
//			GuiController.Instance.CorrectedByLastWriteTime(fsi, creationOrLastAccess);
		}
		
		public void Statistics(Statistics stats)
		{
//			GuiController.Instance.Statistics(stats);
		}
		
		// For error reporting
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
//			GuiController.Instance.UnauthorizedAccessException(ex);
		}
	}
}
