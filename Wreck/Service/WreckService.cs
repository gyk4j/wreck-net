
using System;
using System.Collections.Generic;
using System.IO;

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
		/*
		public bool IsRestorable(FileSystemInfo startPath)
		{
			CsvLogRepository r = CsvLogRepository.Instance;
			return r.Exists(startPath);
		}
		*/
		public ITask Run(
			FileSystemInfo startPath,
			CorrectionMode mode,
			Dictionary<SourceEnum, bool> sources,
			Dictionary<CorrectionEnum, bool> corrections,
			DateTime custom)
//			PropertyChangeListener pcl) // Belongs to Java Beans
		{
			
			ITask task = null;
			
			switch(mode)
			{
				case CorrectionMode.Analyze:
					task = new AnalyzeTask(
						startPath,
						sources,
						custom,
						corrections);
					break;
				case CorrectionMode.SaveAttributes:
					task = new CorrectTask(
						startPath,
						sources,
						custom,
						corrections);
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
			
			return task;
		}
	}
}
