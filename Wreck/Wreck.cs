
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
		
		public Wreck(ILogger logger)
		{
			this.logger = logger;
			this.stats = new Statistics();
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
			if((dir.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
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
		}
		
		public void RetrieveFile(FileInfo file)
		{
			if((file.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
			{
				logger.SkipReparsePoint(file);
				stats.Skip(file);
				return;
			}
			
			logger.CurrentFile(file);
			stats.Count(file);
		}
		
		public void Extract()
		{
			
		}
		
		public void Correct()
		{
			
		}
		
		public void Keep()
		{
			
		}
	}
}