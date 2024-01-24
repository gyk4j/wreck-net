
using System;
using System.IO;
using log4net;

namespace Wreck.Controller
{
	/// <summary>
	/// Description of GuiController.
	/// </summary>
	public class GuiController : IController
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(GuiController));
		
		protected static GuiController instance;
		
		protected MainForm form;
		
		public GuiController(MainForm form)
		{
			this.form = form;
			instance = this;
		}
		
		public static IController Instance
		{
			get
			{
				if (instance == null)
					throw new NullReferenceException("GuiController instance is uninitialized");
				return instance;
			}
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
			if(form.BackgroundWorker != null)
				form.BackgroundWorker.ReportProgress(0, p);
		}
		
		public void CurrentFile(FileInfo f)
		{
			if(form.BackgroundWorker != null)
				form.BackgroundWorker.ReportProgress(0, f);
		}
		
		public void CurrentDirectory(DirectoryInfo d)
		{
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
			if(form.BackgroundWorker != null)
				form.BackgroundWorker.ReportProgress(0, stats);
		}
		
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
			form.UnauthorizedAccessException(ex);
		}
	}
}
