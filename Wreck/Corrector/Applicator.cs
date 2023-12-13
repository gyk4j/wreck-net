
using System;
using System.IO;

namespace Wreck.Corrector
{
	/// <summary>
	/// Description of Corrections.
	/// </summary>
	public class Applicator : ICorrector
	{
		public void ByCreationMetadata(FileSystemInfo fsi, DateTime? creation)
		{
			fsi.CreationTime = (DateTime) creation;
		}

		public void ByLastWriteMetadata(FileSystemInfo fsi, DateTime? lastWrite)
		{
			fsi.LastWriteTime = (DateTime) lastWrite;
		}

		public void ByLastAccessMetadata(FileSystemInfo fsi, DateTime? lastAccess)
		{
			fsi.LastAccessTime = (DateTime) lastAccess;
		}

		public void ByLastWriteTime(FileSystemInfo fsi)
		{
			fsi.CreationTime = fsi.LastWriteTime;
			fsi.LastAccessTime = fsi.LastWriteTime;
		}
	}
}
