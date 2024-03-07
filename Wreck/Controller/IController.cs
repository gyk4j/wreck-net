
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
		void Run(CorrectionMode mode, string[] paths);
		void Analyze();
		void Backup();
		void Repair();
		void Restore();
		void Verify();
	}
}
