
using System;
using System.IO;
using Wreck.Resources;

namespace Wreck.Controller
{
	/// <summary>
	/// Description of IController.
	/// </summary>
	public interface IController
	{
		void Error();
		void Run(CorrectionMode mode, FileSystemInfo fsi);
		void Analyze();
		void Backup();
		void Repair();
		void Restore();
		void Verify();
	}
}
