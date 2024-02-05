
using System;
using System.IO;

namespace Wreck.Controller
{
	/// <summary>
	/// Description of IController.
	/// </summary>
	public interface IController
	{
		void Analyze();
		void Backup();
		void Repair();
		void Restore();
		void Verify();
	}
}
