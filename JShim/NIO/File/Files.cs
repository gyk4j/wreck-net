
using System;
using System.Collections.Generic;
using System.IO;

using log4net;

namespace JShim.NIO.File
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class Files 
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(Files));
		
		public static FileSystemInfo WalkFileTree<T>(FileSystemInfo start,
                                    List<FileVisitOption> options,
                                    int maxDepth,
                                    FileVisitor<T> visitor)
			where T : FileSystemInfo
		{
			if(System.IO.File.Exists(start.FullName)){
				VisitFile(new FileInfo(start.FullName), visitor);
			}
			else if(System.IO.Directory.Exists(start.FullName))
			{
				VisitDirectory(new DirectoryInfo(start.FullName), visitor);
			}
			else
			{
				LOG.ErrorFormat("Unknown path type: {0}", start.FullName);
//				logger.UnknownPathType(start);
			}
			return start;
		}
		
		public static FileSystemInfo WalkFileTree<T>(FileSystemInfo start, FileVisitor<T> visitor) 
			where T : FileSystemInfo
		{
			return WalkFileTree(start,
			                    new List<FileVisitOption>(),
			                    int.MaxValue,
			                    visitor);
		}
		
		private static void VisitDirectory<T>(DirectoryInfo dir, FileVisitor<T> visitor)
			where T : FileSystemInfo
		{
			if(FSUtils.IsReparsePoint(dir))
			{
//				logger.SkipReparsePoint(dir);
//				stats.Skip(dir);
				visitor.VisitFileFailed(dir, new IOException("Skip reparse point"));
				return;
			}
			
//			logger.CurrentDirectory(dir);
			visitor.PreVisitDirectory(dir);
			
			FileInfo[] files = dir.GetFiles();
			foreach(FileInfo f in files)
			{
				VisitFile(f, visitor);
			}
			
			DirectoryInfo[] dirs = dir.GetDirectories();
			foreach(DirectoryInfo d in dirs)
			{
				VisitDirectory(d, visitor);
			}
			
//			stats.Count(dir);
			
			// TODO: To add detailed error reporting if exception occurs.
			visitor.PostVisitDirectory(dir, null);
			
			// TODO: Shift processing logic to FileVisitor
//			DateTime? creation, lastWrite, lastAccess;
//			Extract(dir, out creation, out lastWrite, out lastAccess);
//			Correct(dir, creation, lastWrite, lastAccess);
		}
		
		private static void VisitFile<T>(FileInfo file, FileVisitor<T> visitor)
			where T : FileSystemInfo
		{
			if(FSUtils.IsReparsePoint(file))
			{
//				logger.SkipReparsePoint(file);
//				stats.Skip(file);
				visitor.VisitFileFailed(file, new IOException("Skip reparse point"));
				return;
			}
			
//			logger.CurrentFile(file);
//			stats.Count(file);
			
			// TODO: Shift processing logic to FileVisitor
			visitor.VisitFile(file);
			/*
			DateTime? creation, lastWrite, lastAccess;
			IEnumerator<IFileDateable> e = this.parsers.Values.GetEnumerator();
			while(e.MoveNext())
			{
				IFileDateable parser = e.Current;
				try
				{
					parser.GetDateTimes(file, out creation, out lastWrite, out lastAccess);
				}
				catch(ApplicationException ex)
				{
					creation = lastWrite = lastAccess = null;
					log.Error(ex);
				}
				
				if(creation.HasValue || lastWrite.HasValue || lastAccess.HasValue)
				{
					// Backup and restore Read-Only attribute prior to updating any 
					// timestamps.
					bool readOnly = file.IsReadOnly;
					file.IsReadOnly = false;
					Correct(file, creation, lastWrite, lastAccess);
					file.IsReadOnly = readOnly;
				}
				
				creation = lastWrite = lastAccess = null;
			}
			*/
		}
	}
}