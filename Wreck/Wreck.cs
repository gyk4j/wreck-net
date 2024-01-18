
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using JShim.NIO.File;
using log4net;
using log4net.Config;
using Wreck.Corrector;
using Wreck.Logging;
using Wreck.Parser;

namespace Wreck
{
	/// <summary>
	/// Description of Wreck.
	/// </summary>
	public class Wreck : IDisposable
	{
		public const string NAME = "WRECK.NET";
		public const string VERSION = "1.00a";
		
		private static readonly ILog log = LogManager.GetLogger(typeof(Wreck));
		
		private ILogger logger;
		
		private Statistics stats;
		public enum FileInfoTools {
			ExifTool,
			MediaInfo,
			SevenZip
		};
		private Dictionary<FileInfoTools, IFileDateable> parsers;
		private ICorrector corrector;
		
		public Wreck(ILogger logger, ICorrector corrector)
		{
			this.logger = logger;
			this.stats = new Statistics();
			
			this.parsers = new Dictionary<FileInfoTools, IFileDateable>();
			
			try
			{
				this.parsers.Add(FileInfoTools.ExifTool, new ExifToolParser());
			}
			catch(ApplicationException ex)
			{
				log.Error(ex);
			}
			
			this.parsers.Add(FileInfoTools.MediaInfo, new MediaInfoParser());
			this.parsers.Add(FileInfoTools.SevenZip, new SevenZipParser());
			
			this.corrector = corrector;
		}
		
		public void Dispose()
		{
			IFileDateable exifToolParser; 
			if(this.parsers.TryGetValue(FileInfoTools.ExifTool, out exifToolParser))
			{
				((ExifToolParser) exifToolParser).Dispose();
			}
			
			GC.SuppressFinalize(this);
		}
		
		public Statistics GetStatistics()
		{
			return stats;
		}
		
		public void Walk(string startingPath)
		{
			FileSystemInfo start;
			
			if(Directory.Exists(startingPath))
				start = new DirectoryInfo(startingPath);
			else if(File.Exists(startingPath))
				start = new FileInfo(startingPath);
			else
				throw new IOException(startingPath + " is neither directory or file.");
			
			FileVisitor visitor = new EchoFileVisitor();
			Files.WalkFileTree(start, visitor);
		}
		
		// HACK: To be replaced by actual implementation.
		class EchoFileVisitor : FileVisitor
		{
			public FileVisitResult PreVisitDirectory(DirectoryInfo dir)
			{
				log.InfoFormat("PreVisitDirectory: {0}", dir.FullName);
				return FileVisitResult.Continue;
			}
			
			public FileVisitResult VisitFile(FileInfo file)
			{
				log.InfoFormat("VisitFile: {0}", file.Name);
				return FileVisitResult.Continue;
			}
			
			public FileVisitResult VisitFileFailed(FileSystemInfo file, IOException exc)
			{
				log.ErrorFormat("VisitFileFailed: {0}, Exception: {1}", file.FullName, exc.Message);
				return FileVisitResult.Continue;
			}
			
			public FileVisitResult PostVisitDirectory(DirectoryInfo dir, IOException exc)
			{
				log.InfoFormat("PostVisitDirectory: {0}", dir.FullName);
				return FileVisitResult.Continue;
			}
		}
		
		/// <summary>
		/// Extract metadata by piping a file or directory through the
		/// appropriate third party tool(s) e.g. exiftool, mediainfo.
		/// 
		/// Extracted metadata time are returned in creation, lastWrite and
		/// lastAccess passed by reference. Null value reflects absence of
		/// usable metadata date time tag or value.
		/// </summary>
		/// <param name="fsi">FileSystemInfo object representing a file or directory</param>
		/// <param name="creation">Metadata creation time</param>
		/// <param name="lastWrite">Metadata modification time</param>
		/// <param name="lastAccess">Metadata access time</param>
		public void Extract(
			FileSystemInfo fsi,
			out DateTime? creation,
			out DateTime? lastWrite,
			out DateTime? lastAccess)
		{
			// TODO: To be updated with real metadata extraction calls
			DateTime test = new DateTime(1980, 1, 1, 0, 0, 0);
			creation = test;
			lastWrite = test;
			lastAccess = test;
		}
		
		/// <summary>
		/// Correct the creation, last write and last access file time according
		/// to precedence order: embedded metadata, or relative to last write
		/// time.
		/// 
		/// Creation time will be reset to last write time if it is a copy that
		/// reflects the copying date time.
		/// 
		/// Last Access time will be reset to last write time as a conservative
		/// estimate of the active time period of use, rather than some sporadic
		/// random access reset by ad-hoc document retrievals.
		/// </summary>
		/// <param name="fsi">FileSystemInfo object representing a file or directory</param>
		/// <param name="creation">Metadata creation time</param>
		/// <param name="lastWrite">Metadata modification time</param>
		/// <param name="lastAccess">Metadata access time</param>
		public void Correct(
			FileSystemInfo fsi,
			DateTime? creation,
			DateTime? lastWrite,
			DateTime? lastAccess)
		{			
			// Fix modification time.
			try
			{
				if (lastWrite.HasValue && !fsi.LastWriteTime.Equals(lastWrite.Value))
				{
					corrector.ByLastWriteMetadata(fsi, lastWrite.Value);
					logger.CorrectedByLastWriteMetadata(fsi, lastWrite.Value);
				}					
			}
			catch(UnauthorizedAccessException ex)
			{
				logger.UnauthorizedAccessException(ex);
			}
			
			
			// Fix creation time using specified time,
			try
			{
				if (creation.HasValue && !fsi.CreationTime.Equals(creation.Value))
				{
					corrector.ByCreationMetadata(fsi, creation.Value);
					logger.CorrectedByCreationMetadata(fsi, creation.Value);
				}
			}
			catch(UnauthorizedAccessException ex)
			{
				logger.UnauthorizedAccessException(ex);
			}
			
			// Fix access time using specified time, or from modified time.
			try
			{
				if (lastAccess.HasValue && !fsi.LastAccessTime.Equals(lastAccess.Value))
				{
					corrector.ByLastAccessMetadata(fsi, lastAccess.Value);
					logger.CorrectedByLastAccessMetadata(fsi, lastAccess.Value);
				}
			}
			catch(UnauthorizedAccessException ex)
			{
				logger.UnauthorizedAccessException(ex);
			}
			
			// Fix creation time from modified time.
			// Creation time will always be earlier than modification time.
			/*
			try
			{
				if (fsi.CreationTime.CompareTo(fsi.LastWriteTime) > 0)
				{
					corrector.ByLastWriteTime(fsi, fsi.CreationTime);
					logger.CorrectedByLastWriteTime(fsi, fsi.CreationTime);
				}
				
			}
			catch(UnauthorizedAccessException ex)
			{
				logger.UnauthorizedAccessException(ex);
			}
			*/
			// Fix last access time from modified time.
			// Last access time will always be earlier than modification time.
			/*
			try
			{
				if(fsi.LastAccessTime.CompareTo(fsi.LastWriteTime) > 0)
				{
					corrector.ByLastWriteTime(fsi, fsi.LastAccessTime);
					logger.CorrectedByLastWriteTime(fsi, fsi.LastAccessTime);
				}
			}
			catch(UnauthorizedAccessException ex)
			{
				logger.UnauthorizedAccessException(ex);
			}
			*/
		}
		
		/// <summary>
		/// Does nothing for now after the file system time stamps are corrected.
		/// Perhaps for some future post-processing tasks like:
		/// - packaging into a ZIP archive
		/// - generating a file manifest or hash file like md5sum 
		/// </summary>
		public void Keep()
		{
			
		}
	}
}