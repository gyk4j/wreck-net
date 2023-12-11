
using System;
using System.IO;

namespace Wreck
{
	/// <summary>
	/// Description of Logger.
	/// </summary>
	public class Logger : ILogger
	{
		public Logger()
		{
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
		
		public void Statistics(Statistics stats)
		{
			Console.WriteLine("\n### Dirs: {0}, Files: {1}, Skipped: {2} ###\n", 
			                  stats.Directories, 
			                  stats.Files, 
			                  stats.Skipped);
		}
	}
}
