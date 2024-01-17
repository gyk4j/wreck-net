
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
				RetrieveFile(new FileInfo(start.FullName));
			}
			else if(System.IO.Directory.Exists(start.FullName))
			{
				RetrieveDirectory(new DirectoryInfo(start.FullName));
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
		
		private static void RetrieveDirectory(DirectoryInfo dir)
		{
			if(FSUtils.IsReparsePoint(dir))
			{
//				logger.SkipReparsePoint(dir);
//				stats.Skip(dir);
				return;
			}
			
//			logger.CurrentDirectory(dir);
			
			FileInfo[] files = dir.GetFiles();
			foreach(FileInfo f in files)
			{
				RetrieveFile(f);
			}
			
			DirectoryInfo[] dirs = dir.GetDirectories();
			foreach(DirectoryInfo d in dirs)
			{
				RetrieveDirectory(d);
			}
			
//			stats.Count(dir);
			
			// TODO: Shift processing logic to FileVisitor
//			DateTime? creation, lastWrite, lastAccess;
//			Extract(dir, out creation, out lastWrite, out lastAccess);
//			Correct(dir, creation, lastWrite, lastAccess);
		}
		
		private static void RetrieveFile(FileInfo file)
		{
			if(FSUtils.IsReparsePoint(file))
			{
//				logger.SkipReparsePoint(file);
//				stats.Skip(file);
				return;
			}
			
//			logger.CurrentFile(file);
//			stats.Count(file);
			
			// TODO: Shift processing logic to FileVisitor
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