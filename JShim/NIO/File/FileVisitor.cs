
using System;
using System.IO;

namespace JShim.NIO.File
{
	public interface FileVisitor<T> where T : FileSystemInfo
	{
		/// Invoked for a directory before entries in the directory are visited.
		/// 
		/// If this method returns <see cref="FileVisitResult.CONTINUE">CONTINUE</see>,
		/// then entries in the directory are visited. If this method returns
		/// <see cref="FileVisitResult.SKIP_SUBTREE">SKIP_SUBTREE</see> or
		/// <see cref="FileVisitResult.SKIP_SIBLINGS">SKIP_SIBLINGS</see> then entries in the
		/// directory (and any descendants) will not be visited.
		/// <param name="dir">a reference to the directory</param>
		/// <returns>the visit result</returns>
		FileVisitResult PreVisitDirectory<T>(T dir) where T : FileSystemInfo;
		
		/// <summary>
		/// Invoked for a file in a directory.
		/// </summary>
		/// <param name="file">a reference to the file</param>
		/// <returns></returns>
		FileVisitResult VisitFile<T>(T file) where T : FileSystemInfo;
		
		/// <summary>
		/// Invoked for a file that could not be visited. This method is invoked
		/// if the file's attributes could not be read, the file is a directory
		/// that could not be opened, and other reasons.
		/// </summary>
		/// <param name="file">a reference to the file</param>
		/// <param name="exc">the I/O exception that prevented the file from being visited</param>
		/// <returns>the visit result</returns>
		FileVisitResult VisitFileFailed<T>(T file, IOException exc) where T : FileSystemInfo;
		
		/// <summary>
		/// Invoked for a directory after entries in the directory, and all of their
		/// descendants, have been visited. This method is also invoked when iteration
		/// of the directory completes prematurely (by a <see cref="VisitFile">VisitFile</see>
		/// method returning <see cref="FileVisitResult.SKIP_SIBLINGS">SKIP_SIBLINGS</see>,
		/// or an I/O error when iterating over the directory).
		/// </summary>
		/// <param name="dir">a reference to the directory</param>
		/// <param name="exc">
		/// <c>null</c> if the iteration of the directory completes without
		/// an error; otherwise the I/O exception that caused the iteration
		/// of the directory to complete prematurely
		/// </param>
		/// <returns>the visit result</returns>
		FileVisitResult PostVisitDirectory<T>(T dir, IOException exc) where T : FileSystemInfo;
	}
}
