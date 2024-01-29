
using System;
using System.IO;

namespace Java.NIO.File
{
	/// <summary>
	/// A simple visitor of files with default behavior to visit all files and to re-throw I/O errors.
	/// Methods in this class may be overridden subject to their general contract.
	/// </summary>
	public class SimpleFileVisitor : FileVisitor
	{
		public virtual FileVisitResult PreVisitDirectory(DirectoryInfo dir)
		{
			return FileVisitResult.Continue;
		}
		
		public virtual FileVisitResult VisitFile(FileInfo file)
		{
			return FileVisitResult.Continue;
		}
		
		public virtual FileVisitResult VisitFileFailed(FileSystemInfo file, IOException exc)
		{
			return FileVisitResult.Continue;
		}
		
		public virtual FileVisitResult PostVisitDirectory(DirectoryInfo dir, IOException exc)
		{
			return FileVisitResult.Continue;
		}
	}
}
