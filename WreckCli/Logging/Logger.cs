
using System;
using System.IO;

namespace Wreck.Logging
{
	/// <summary>
	/// Description of Logger.
	/// </summary>
	public class Logger : ILogger
	{
		public Logger()
		{
		}
		
		// For Start
		public void Version()
		{
			Console.WriteLine("{0} v{1}\n", Wreck.NAME, Wreck.VERSION);
		}
		
		// For Directory Tree Walking
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
		
		// For Corrector
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
			Console.WriteLine("        MW: {0}", FormatTimeSpan(fsi.LastWriteTime.Subtract(lastWrite)));
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
			Console.WriteLine("        MC: {0}", FormatTimeSpan(fsi.CreationTime.Subtract(creation)));
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
			Console.WriteLine("        MA: {0}", FormatTimeSpan(fsi.LastAccessTime.Subtract(lastAccess)));
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi)
		{
			Console.WriteLine("        LC: {0}", FormatTimeSpan(fsi.CreationTime.Subtract(fsi.LastWriteTime)));
			Console.WriteLine("        LA: {0}", FormatTimeSpan(fsi.LastAccessTime.Subtract(fsi.LastWriteTime)));
		}
		
		private string FormatTimeSpan(TimeSpan ts)
		{
			String s;
			if(ts.Days > 365)
				s = (ts.Days / 365) + " year(s)";
			else if(ts.Days > 30)
				s = (ts.Days / 30) + " month(s)";
			else if(ts.Days > 0)
				s = ts.Days + " day(s)";
			else if(ts.Hours > 0)
				s = ts.Hours + " hour(s)";
			else if(ts.Minutes > 0)
				s = ts.Minutes + " minute(s)";
			else if(ts.Seconds > 0)
				s = ts.Seconds + " second(s)";
			else
				s = ts.Milliseconds + " millisec(s)";
			
			return s;
		}
		
		// For End
		public void Statistics(Statistics stats)
		{
			Console.WriteLine("\n### Dirs: {0}, Files: {1}, Skipped: {2} ###\n", 
			                  stats.Directories, 
			                  stats.Files, 
			                  stats.Skipped);
		}
		
		// For error reporting
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
			Console.Error.WriteLine(ex.ToString());
		}
	}
}
