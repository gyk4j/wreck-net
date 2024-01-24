
using System;
using System.IO;
using Wreck.Controller;

namespace Wreck.Logging
{
	/// <summary>
	/// Description of Logger.
	/// </summary>
	public class Logger : ILogger
	{
		private static IController controller = CliController.Instance;
			
		public Logger()
		{
		}
		
		// For Start
		public void Version()
		{
			controller.Version();
		}
		
		// For Directory Tree Walking
		public void UnknownPathType(string path)
		{
			controller.UnknownPathType(path);
		}
		
		public void CurrentPath(string p)
		{
			controller.CurrentPath(p);
		}
		
		public void CurrentFile(FileInfo f)
		{
			controller.CurrentFile(f);
		}
	
		public void CurrentDirectory(DirectoryInfo d)
		{
			controller.CurrentDirectory(d);
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
			controller.SkipReparsePoint(d);
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
			controller.SkipReparsePoint(f);
		}
		
		// For Corrector
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
			controller.CorrectedByLastWriteMetadata(fsi, lastWrite);
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
			controller.CorrectedByCreationMetadata(fsi, creation);
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
			controller.CorrectedByLastAccessMetadata(fsi, lastAccess);
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
			controller.CorrectedByLastWriteTime(fsi, creationOrLastAccess);
		}
		
		// For End
		public void Statistics(Statistics stats)
		{
			controller.Statistics(stats);
		}
		
		// For error reporting
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
			controller.UnauthorizedAccessException(ex);
		}
	}
}
