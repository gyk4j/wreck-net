
using System;
using System.Collections.Generic;
using System.IO;

namespace Wreck.IO.Reader
{
	/// <summary>
	/// Description of ITimestampReader.
	/// </summary>
	public interface ITimestampReader : IDisposable
	{
		void extract(FileSystemInfo file, List<Metadata> metadata);
		string[] creation();
		string[] modified();
		string[] accessed();
	}
}
