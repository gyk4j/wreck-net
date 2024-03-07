
using System;
using System.Collections.Generic;
using System.IO;

using log4net;
using Wreck.IO.Task;
using Wreck.Resources;
using Wreck.UI;

namespace Wreck.Service
{
	/// <summary>
	/// Description of PreviewService.
	/// </summary>
	public class WreckService
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(WreckService));
		/*
		public bool IsRestorable(FileSystemInfo startPath)
		{
			CsvLogRepository r = CsvLogRepository.Instance;
			return r.Exists(startPath);
		}
		 */
		public List<ITask> Run(
			string[] startPaths,
			CorrectionMode mode,
			Dictionary<SourceEnum, bool> sources,
			Dictionary<CorrectionEnum, bool> corrections,
			DateTime custom)
//			PropertyChangeListener pcl) // Belongs to Java Beans
		{
			List<ITask> tasks = new List<ITask>();
			
			FileSystemInfo startPath;
			foreach(string p in startPaths)
			{
				if(Directory.Exists(p))
					startPath = new DirectoryInfo(p);
				else if(File.Exists(p))
					startPath = new FileInfo(p);
				else
				{
					LOG.WarnFormat("Skipped unsupported type: {0}", p);
					continue;
				}
				
				switch(mode)
				{
					case CorrectionMode.Analyze:
						tasks.Add(
							new AnalyzeTask(
								startPath,
								sources,
								custom,
								corrections));
						break;
					case CorrectionMode.SaveAttributes:
						tasks.Add(
							new CorrectTask(
								startPath,
								sources,
								custom,
								corrections));
						break;
//				case CorrectionMode.BackupAttributes:
//					task = new BackupTask(startPath);
//					break;
//				case CorrectionMode.RestoreAttributes:
//					task = new RestoreTask(startPath);
//					break;
//				case CorrectionMode.VerifyAttributes:
//					task = new VerifyTask(startPath);
//					break;
					default:
						throw new ArgumentException("Unknown correction mode");
				}
			}
			
			return tasks;
		}
	}
}
