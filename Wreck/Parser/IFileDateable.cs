
using System;
using System.IO;

namespace Wreck.Parser
{
	/// <summary>
	/// Description of IDateable.
	/// </summary>
	public interface IFileDateable
	{
		void GetDateTimes(
			FileInfo fi, 
			out DateTime? creationTime, 
			out DateTime? lastWriteTime, 
			out DateTime? lastAccessTime);
	}
}
