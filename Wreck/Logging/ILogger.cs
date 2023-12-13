
using System;
using System.IO;

namespace Wreck.Logging
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
		void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite);
		void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation);
		void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess);
		void CorrectedByLastWriteTime(FileSystemInfo fsi);
		void Statistics(Statistics stats);
		void UnauthorizedAccessException(UnauthorizedAccessException ex);
	}
	
}
