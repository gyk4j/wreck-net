
using System;
using System.IO;
using log4net;

namespace Wreck.Controller
{
	/// <summary>
	/// Description of CliController.
	/// </summary>
	public class CliController : IController
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(CliController));
		
		protected static CliController instance;
		
		public CliController()
		{
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
		
		public void Version()
		{
			throw new NotImplementedException();
		}
		
		public void UnknownPathType(string path)
		{
			throw new NotImplementedException();
		}
		
		public void CurrentPath(string p)
		{
			throw new NotImplementedException();
		}
		
		public void CurrentFile(FileInfo f)
		{
			throw new NotImplementedException();
		}
		
		public void CurrentDirectory(DirectoryInfo d)
		{
			throw new NotImplementedException();
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
			throw new NotImplementedException();
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
			throw new NotImplementedException();
		}
		
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
			throw new NotImplementedException();
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
			throw new NotImplementedException();
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
			throw new NotImplementedException();
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
			throw new NotImplementedException();
		}
		
		public void Statistics(Statistics stats)
		{
			throw new NotImplementedException();
		}
		
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
			throw new NotImplementedException();
		}
	}
}
