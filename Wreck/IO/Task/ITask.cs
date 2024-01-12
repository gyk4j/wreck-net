
using System;
using System.Collections.Generic;
using System.IO;

using Wreck.Resources;

namespace Wreck.IO.Task
{
	/// <summary>
	/// Description of ITask.
	/// </summary>
	public interface ITask
	{
		void PreVisitDirectory(
			Dictionary<CorrectionEnum, DateTime> suggestions,
			DirectoryInfo dir);
		
		void PostVisitDirectory(
			Dictionary<CorrectionEnum, DateTime> suggestions,
			DirectoryInfo dir);
		
		void VisitFile(
			Dictionary<CorrectionEnum, DateTime> suggestions,
			FileInfo file);
		
		void VisitFileFailed(
			Dictionary<CorrectionEnum, DateTime> suggestions,
			FileSystemInfo file,
			IOException exc);
	}
}
