
using System;
using System.Collections.Generic;
using System.IO;

using Wreck.Resources;

namespace Wreck.IO.Writer
{
	/// <summary>
	/// Description of ITimestampWriter.
	/// </summary>
	public interface ITimestampWriter
	{
		void Write(
			FileSystemInfo file, 
			Dictionary<CorrectionEnum, DateTime> values);
	}
}
