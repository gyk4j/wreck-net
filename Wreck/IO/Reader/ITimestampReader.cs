
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
		void Extract(FileSystemInfo file, List<Metadata> metadata);
		string[] Creation();
		string[] Modified();
		string[] Accessed();
	}
}
