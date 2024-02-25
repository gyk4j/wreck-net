
using System;
using System.Collections.Generic;
using System.IO;

using Java.NIO.File.Attribute;
using log4net;
using Microsoft.NET;

namespace Java.NIO.File
{
	/// <summary>
	/// Description of Files.
	/// </summary>
	public class Files
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(Files));
		
		public static FileSystemInfo WalkFileTree(FileSystemInfo start,
		                                          List<FileVisitOption> options,
		                                          int maxDepth,
		                                          FileVisitor visitor)
			
		{
			FileVisitResult result;
			
			if(System.IO.File.Exists(start.FullName)){
				result = VisitFile(new FileInfo(start.FullName), visitor);
			}
			else if(System.IO.Directory.Exists(start.FullName))
			{
				result = VisitDirectory(new DirectoryInfo(start.FullName), visitor);
			}
			else
			{
				LOG.ErrorFormat("Unknown path type: {0}", start.FullName);
			}
			return start;
		}
		
		public static FileSystemInfo WalkFileTree(FileSystemInfo start, FileVisitor visitor)
		{
			return WalkFileTree(start,
			                    new List<FileVisitOption>(),
			                    int.MaxValue,
			                    visitor);
		}
		
		private static FileVisitResult VisitDirectory(DirectoryInfo dir, FileVisitor visitor)
		{
			FileVisitResult result;
			List<IOException> exceptions = new List<IOException>();
			
			if(FSUtils.IsReparsePoint(dir))
			{
				result = visitor.VisitFileFailed(dir, new IOException("Skip reparse point"));
				return result;
			}
			
			result = visitor.PreVisitDirectory(dir);
			
			if(result == FileVisitResult.Terminate)
				return FileVisitResult.Terminate;
			else if(
				result == FileVisitResult.SkipSubTree)
				return FileVisitResult.Continue;
			
			FileInfo[] files = dir.GetFiles();
			foreach(FileInfo f in files)
			{
				try
				{
					result = VisitFile(f, visitor);
				}
				catch(Exception e)
				{
					result = visitor.VisitFileFailed(f, new IOException("Visit file failed", e));
				}
				
				if(result == FileVisitResult.Terminate)
					return FileVisitResult.Terminate;
				else if(result == FileVisitResult.SkipSubTree)
					return FileVisitResult.Continue;
				else if(result == FileVisitResult.SkipSiblings)
					break;
			}
			
			// Do not traverse down to sub-directories.
			if(result == FileVisitResult.SkipSubTree)
				return result;
			
			DirectoryInfo[] dirs = dir.GetDirectories();
			foreach(DirectoryInfo d in dirs)
			{
				try
				{
					result = VisitDirectory(d, visitor);
				}
				catch(Exception e)
				{
					result = visitor.VisitFileFailed(d, new IOException("Visit directory failed", e));
				}
				
				if(result == FileVisitResult.Terminate)
					return FileVisitResult.Terminate;
				else if(result == FileVisitResult.SkipSubTree)
					return FileVisitResult.Continue;
				else if(result == FileVisitResult.SkipSiblings)
					break;
			}
			
			IOException ex = exceptions.Count > 0 ? exceptions[0]: null;
			result = visitor.PostVisitDirectory(dir, ex);
			
			// Added for consistency and not drop down to the default action.
			if(result == FileVisitResult.Terminate)
				return result;
			
			return result;
		}
		
		private static FileVisitResult VisitFile(FileInfo file, FileVisitor visitor)
		{
			FileVisitResult result;
			
			if(FSUtils.IsReparsePoint(file))
			{
				result = visitor.VisitFileFailed(file, new IOException("Skip reparse point"));
				return result;
			}
			
			result = visitor.VisitFile(file);
			return result;
		}
		
		public static V GetFileAttributeView<V>(FileSystemInfo fsi, Type type, params LinkOption[] options) where V : FileAttributeView
		{
			V view = (V) Activator.CreateInstance(type, new object[] { fsi });
			return view;
		}
		
		/// <summary>
		/// Tests if two paths locate the same file.
		/// 
		/// If both Path objects are equal then this method returns true 
		/// without checking if the file exists. If the two Path objects are 
		/// associated with different providers then this method returns false. 
		/// Otherwise, this method checks if both Path objects locate the same 
		/// file, and depending on the implementation, may require to open or 
		/// access both files.
		/// 
		/// If the file system and files remain static, then this method 
		/// implements an equivalence relation for non-null Paths.
		/// 
		/// <list type="">
		/// 	<item>
		/// 		<description>
		/// 		It is reflexive: for Path f, isSameFile(f,f) should return true.
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<description>
		/// 			It is symmetric: for two Paths f and g, isSameFile(f,g) 
		/// 			will equal isSameFile(g,f).
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<description>
		/// 			It is transitive: for three Paths f, g, and h, if 
		/// 			isSameFile(f,g) returns true and isSameFile(g,h) 
		/// 			returns true, then isSameFile(f,h) will return return 
		/// 			true.
		/// 		</description>
		/// 	</item>
		/// </list>
		/// </summary>
		/// <param name="path">one path to the file</param>
		/// <param name="path2">the other path</param>
		/// <returns>true if, and only if, the two paths locate the same file</returns>
		public static bool IsSameFile(Path path, Path path2)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Tells whether or not a file is considered hidden. The exact 
		/// definition of hidden is platform or provider dependent. On UNIX for 
		/// example a file is considered to be hidden if its name begins with a 
		/// period character ('.'). On Windows a file is considered hidden if 
		/// it isn't a directory and the DOS hidden attribute is set.
		/// 
		/// Depending on the implementation this method may require to access 
		/// the file system to determine if the file is considered hidden.
		/// </summary>
		/// <param name="path">the path to the file to test</param>
		/// <returns>true if the file is considered hidden</returns>
		public static bool IsHidden(Path path)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Tests whether a file is a symbolic link.
		/// 
		/// Where it is required to distinguish an I/O exception from the case 
		/// that the file is not a symbolic link then the file attributes can 
		/// be read with the <c>readAttributes</c> method and the file type 
		/// tested with the <c>BasicFileAttributes.isSymbolicLink()</c> method.
		/// </summary>
		/// <param name="path">The path to the file</param>
		/// <returns>
		/// true if the file is a symbolic link; false if the file does not 
		/// exist, is not a symbolic link, or it cannot be determined if the 
		/// file is a symbolic link or not.
		/// </returns>
		public static bool IsSymbolicLink(Path path)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Tests whether a file is a directory.
		/// 
		/// The options array may be used to indicate how symbolic links are 
		/// handled for the case that the file is a symbolic link. By default, 
		/// symbolic links are followed and the file attribute of the final 
		/// target of the link is read. If the option <c>NOFOLLOW_LINKS</c> is 
		/// present then symbolic links are not followed.
		/// 
		/// Where it is required to distinguish an I/O exception from the case 
		/// that the file is not a directory then the file attributes can be 
		/// read with the <c>readAttributes</c> method and the file type tested 
		/// with the <c>BasicFileAttributes.isDirectory()</c> method.
		/// </summary>
		/// <param name="path">the path to the file to test</param>
		/// <param name="options">options indicating how symbolic links are handled</param>
		/// <returns>
		/// true if the file is a directory; false if the file does not exist, 
		/// is not a directory, or it cannot be determined if the file is a 
		/// directory or not.
		/// </returns>
		public static bool IsDirectory(Path path, params LinkOption[] options)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Tests whether a file is a regular file with opaque content.
		/// 
		/// The options array may be used to indicate how symbolic links are 
		/// handled for the case that the file is a symbolic link. By default, 
		/// symbolic links are followed and the file attribute of the final 
		/// target of the link is read. If the option <c>NOFOLLOW_LINK</c>S is 
		/// present then symbolic links are not followed.
		/// 
		/// Where it is required to distinguish an I/O exception from the case 
		/// that the file is not a regular file then the file attributes can be 
		/// read with the <c>readAttributes</c> method and the file type tested 
		/// with the <c>BasicFileAttributes.isRegularFile()</c> method.
		/// </summary>
		/// <param name="path">the path to the file</param>
		/// <param name="options">options indicating how symbolic links are handled</param>
		/// <returns>
		/// true if the file is a regular file; false if the file does not 
		/// exist, is not a regular file, or it cannot be determined if the 
		/// file is a regular file or not.
		/// </returns>
		public static bool IsRegularFile(Path path, params LinkOption[] options)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Returns a file's last modified time.
		/// 
		/// The options array may be used to indicate how symbolic links are 
		/// handled for the case that the file is a symbolic link. By default, 
		/// symbolic links are followed and the file attribute of the final 
		/// target of the link is read. If the option NOFOLLOW_LINKS is present 
		/// then symbolic links are not followed.
		/// </summary>
		/// <param name="path">the path to the file</param>
		/// <param name="options">options indicating how symbolic links are handled</param>
		/// <returns>
		/// a DateTime representing the time the file was last modified, or an 
		/// implementation specific default when a time stamp to indicate the 
		/// time of last modification is not supported by the file system
		/// </returns>
		public static DateTime GetLastModifiedTime(Path path, params LinkOption[] options)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Updates a file's last modified time attribute. The file time is 
		/// converted to the epoch and precision supported by the file system. 
		/// Converting from finer to coarser granularities result in precision 
		/// loss. The behavior of this method when attempting to set the last 
		/// modified time when it is not supported by the file system or is 
		/// outside the range supported by the underlying file store is not 
		/// defined. It may or not fail by throwing an IOException.
		/// 
		/// Usage Example: Suppose we want to set the last modified time to the 
		/// current time:
		/// <code>
		/// 	Path path = ...
		///		FileTime now = FileTime.fromMillis(System.currentTimeMillis());
		///		Files.setLastModifiedTime(path, now);
		/// </code>
		/// </summary>
		/// <param name="path">the path to the file</param>
		/// <param name="time">the new last modified time</param>
		/// <returns>the path</returns>
		public static Path SetLastModifiedTime(Path path, DateTime time)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Returns the size of a file (in bytes). The size may differ from the 
		/// actual size on the file system due to compression, support for 
		/// sparse files, or other reasons. The size of files that are not 
		/// regular files is implementation specific and therefore unspecified.
		/// </summary>
		/// <param name="path">the path to the file</param>
		/// <returns>the file size, in bytes</returns>
		public static long Size(Path path)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Tests whether a file exists.
		/// 
		/// The options parameter may be used to indicate how symbolic links 
		/// are handled for the case that the file is a symbolic link. By 
		/// default, symbolic links are followed. If the option NOFOLLOW_LINKS 
		/// is present then symbolic links are not followed.
		/// 
		/// Note that the result of this method is immediately outdated. If 
		/// this method indicates the file exists then there is no guarantee 
		/// that a subsequence access will succeed. Care should be taken when 
		/// using this method in security sensitive applications.
		/// </summary>
		/// <param name="path">the path to the file to test</param>
		/// <param name="options">options indicating how symbolic links are handled .</param>
		/// <returns>
		/// true if the file exists; false if the file does not exist or its 
		/// existence cannot be determined.
		/// </returns>
		public static bool Exists(Path path, params LinkOption[] options)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Tests whether the file located by this path does not exist. This 
		/// method is intended for cases where it is required to take action 
		/// when it can be confirmed that a file does not exist.
		/// 
		/// The options parameter may be used to indicate how symbolic links 
		/// are handled for the case that the file is a symbolic link. By 
		/// default, symbolic links are followed. If the option NOFOLLOW_LINKS 
		/// is present then symbolic links are not followed.
		/// 
		/// Note that this method is not the complement of the exists method. 
		/// Where it is not possible to determine if a file exists or not then 
		/// both methods return false. As with the exists method, the result of 
		/// this method is immediately outdated. If this method indicates the 
		/// file does exist then there is no guarantee that a subsequence 
		/// attempt to create the file will succeed. Care should be taken when 
		/// using this method in security sensitive applications.
		/// </summary>
		/// <param name="path">the path to the file to test</param>
		/// <param name="options">options indicating how symbolic links are handled</param>
		/// <returns>
		/// true if the file does not exist; false if the file exists or its 
		/// existence cannot be determined
		/// </returns>
		public static bool NotExists(Path path, params LinkOption[] options)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// <para>
		/// Tests whether a file is readable. This method checks that a file 
		/// exists and that this Java virtual machine has appropriate 
		/// privileges that would allow it open the file for reading. Depending 
		/// on the implementation, this method may require to read file 
		/// permissions, access control lists, or other file attributes in 
		/// order to check the effective access to the file. Consequently, this 
		/// method may not be atomic with respect to other file system 
		/// operations.
		/// </para>
		/// <para>
		/// Note that the result of this method is immediately outdated, there 
		/// is no guarantee that a subsequent attempt to open the file for 
		/// reading will succeed (or even that it will access the same file). 
		/// Care should be taken when using this method in security sensitive 
		/// applications.
		/// </para>
		/// </summary>
		/// <param name="path">the path to the file to check</param>
		/// <returns>
		/// true if the file exists and is readable; false if the file does not 
		/// exist, read access would be denied because the Java virtual machine 
		/// has insufficient privileges, or access cannot be determined
		/// </returns>
		public static bool IsReadable(Path path)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Tests whether a file is writable. This method checks that a file 
		/// exists and that this Java virtual machine has appropriate 
		/// privileges that would allow it open the file for writing. Depending 
		/// on the implementation, this method may require to read file 
		/// permissions, access control lists, or other file attributes in 
		/// order to check the effective access to the file. Consequently, this 
		/// method may not be atomic with respect to other file system 
		/// operations.
		/// 
		/// Note that result of this method is immediately outdated, there is 
		/// no guarantee that a subsequent attempt to open the file for writing 
		/// will succeed (or even that it will access the same file). Care 
		/// should be taken when using this method in security sensitive 
		/// applications.
		/// </summary>
		/// <param name="path">the path to the file to check</param>
		/// <returns>
		/// true if the file exists and is writable; false if the file does not 
		/// exist, write access would be denied because the Java virtual 
		/// machine has insufficient privileges, or access cannot be determined
		/// </returns>
		public static bool IsWritable(Path path)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Tests whether a file is executable. This method checks that a file 
		/// exists and that this Java virtual machine has appropriate 
		/// privileges to execute the file. The semantics may differ when 
		/// checking access to a directory. For example, on UNIX systems, 
		/// checking for execute access checks that the Java virtual machine 
		/// has permission to search the directory in order to access file or 
		/// subdirectories.
		/// 
		/// Depending on the implementation, this method may require to read 
		/// file permissions, access control lists, or other file attributes in 
		/// order to check the effective access to the file. Consequently, this 
		/// method may not be atomic with respect to other file system 
		/// operations.
		/// 
		/// Note that the result of this method is immediately outdated, there 
		/// is no guarantee that a subsequent attempt to execute the file will 
		/// succeed (or even that it will access the same file). Care should be 
		/// taken when using this method in security sensitive applications.
		/// </summary>
		/// <param name="path">the path to the file to check</param>
		/// <returns>
		/// true if the file exists and is executable; false if the file does 
		/// not exist, execute access would be denied because the Java virtual 
		/// machine has insufficient privileges, or access cannot be determined
		/// </returns>
		public static bool IsExecutable(Path path)
		{
			throw new NotImplementedException();
		}
	}
}