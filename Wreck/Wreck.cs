
using System;
using System.Collections.Generic;
using System.IO;

namespace Wreck
{
	/// <summary>
	/// Description of Wreck.
	/// </summary>
	public class Wreck
	{
		public const string NAME = "WRECK.NET";
		public const string VERSION = "1.00a";
		
		private ILogger logger;
		
		private Statistics stats;
		
		private ICorrector corrector;
		
		public Wreck(ILogger logger, ICorrector corrector)
		{
			this.logger = logger;
			this.stats = new Statistics();
			this.corrector = corrector;
		}
		
		public Statistics GetStatistics()
		{
			return stats;
		}
		
		public void Walk(string startingPath)
		{
			if(File.Exists(startingPath)){
				FileInfo start = new FileInfo(startingPath);
				RetrieveFile(start);
			}
			else if(Directory.Exists(startingPath))
			{
				DirectoryInfo start = new DirectoryInfo(startingPath);
				RetrieveDirectory(start);
			}
			else
			{
				logger.UnknownPathType(startingPath);
			}
		}
		
		public void RetrieveDirectory(DirectoryInfo dir)
		{
			if(FSUtils.IsReparsePoint(dir))
			{
				logger.SkipReparsePoint(dir);
				stats.Skip(dir);
				return;
			}
			
			logger.CurrentDirectory(dir);
			
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
			
			stats.Count(dir);
			
			DateTime? creation, lastWrite, lastAccess;
			Extract(dir, out creation, out lastWrite, out lastAccess);
			Correct(dir, creation, lastWrite, lastAccess);
		}
		
		public void RetrieveFile(FileInfo file)
		{
			if(FSUtils.IsReparsePoint(file))
			{
				logger.SkipReparsePoint(file);
				stats.Skip(file);
				return;
			}
			
			logger.CurrentFile(file);
			stats.Count(file);
			
			DateTime? creation, lastWrite, lastAccess;
			Extract(file, out creation, out lastWrite, out lastAccess);
			Correct(file, creation, lastWrite, lastAccess);
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
			creation = null;
			lastWrite = fsi.LastWriteTime;
			lastAccess = null;
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
			if(corrector.ByLastWriteMetadata(fsi, lastWrite))
				logger.CorrectedByLastWriteMetadata(fsi, (DateTime) lastWrite);
			
			// Fix creation time using specified time,
			if(corrector.ByCreationMetadata(fsi, creation))
				logger.CorrectedByCreationMetadata(fsi, (DateTime) creation);
			
			// Fix access time using specified time, or from modified time.
			if(corrector.ByLastAccessMetadata(fsi, lastAccess))
				logger.CorrectedByLastAccessMetadata(fsi, (DateTime) lastAccess);
			
			// Fix creation and last access time from modified time.
			// Creation time will always be earlier than modification time.
			// Last access time will always be earlier than modification time.
			if(corrector.ByLastWriteTime(fsi))
				logger.CorrectedByLastWriteTime(fsi);
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