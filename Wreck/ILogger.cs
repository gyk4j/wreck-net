
using System;
using System.IO;

namespace Wreck
{
	/// <summary>
	/// Description of ILogger.
	/// </summary>
	public interface ILogger
	{
		void Version();
		void UnknownPathType(string path);
		void CurrentPath(string p);
		void CurrentFile(FileInfo f);
		void CurrentDirectory(DirectoryInfo d);
		void SkipReparsePoint(DirectoryInfo d);
		void SkipReparsePoint(FileInfo f);
		void Statistics(Statistics stats);
	}
	
}
