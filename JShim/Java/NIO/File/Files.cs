
using System;
using System.Collections.Generic;
using System.IO;

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
	}
}