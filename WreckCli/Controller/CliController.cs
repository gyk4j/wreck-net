
using System;
using System.IO;
using log4net;
using Wreck.Logging;

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
			Console.WriteLine("{0} v{1}\n", Wreck.NAME, Wreck.VERSION);
		}
		
		public void UnknownPathType(string path)
		{
			Console.WriteLine("UnknownPathType: {0}", path);
		}
		
		public void CurrentPath(string p)
		{
			Console.WriteLine("> {0}", p);
		}
		
		public void CurrentFile(FileInfo f)
		{
			Console.WriteLine("    - {0}", f.Name);
		}
		
		public void CurrentDirectory(DirectoryInfo d)
		{
			Console.WriteLine("  + {0}", d.FullName);
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
			Console.WriteLine("Skipped reparse point: {0}", d.Name);
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
			Console.WriteLine("Skipped reparse point: {0}", f.Name);
		}
		
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
			Console.WriteLine("        MW: {0}", TextFormatter.Format(fsi.LastWriteTime.Subtract(lastWrite)));
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
			Console.WriteLine("        MC: {0}", TextFormatter.Format(fsi.CreationTime.Subtract(creation)));
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
			Console.WriteLine("        MA: {0}", TextFormatter.Format(fsi.LastAccessTime.Subtract(lastAccess)));
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
			if(creationOrLastAccess == fsi.CreationTime)
				Console.WriteLine("        LC: {0}", TextFormatter.Format(fsi.CreationTime.Subtract(fsi.LastWriteTime)));
			else if(creationOrLastAccess == fsi.LastAccessTime)
				Console.WriteLine("        LA: {0}", TextFormatter.Format(fsi.LastAccessTime.Subtract(fsi.LastWriteTime)));
		}
		
		public void Statistics(Statistics stats)
		{
			Console.WriteLine("\n### Dirs: {0}, Files: {1}, Skipped: {2} ###\n", 
			                  stats.Directories, 
			                  stats.Files, 
			                  stats.Skipped);
		}
		
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
			Console.Error.WriteLine(ex.ToString());
		}
	}
}
