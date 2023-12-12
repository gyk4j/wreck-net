
using System;
using System.IO;

namespace Wreck
{
	public interface ICorrector
	{
		bool ByCreationMetadata(FileSystemInfo fsi, DateTime? creation);
		bool ByLastWriteMetadata(FileSystemInfo fsi, DateTime? lastWrite);
		bool ByLastAccessMetadata(FileSystemInfo fsi, DateTime? lastAccess);
		bool ByLastWriteTime(FileSystemInfo fsi);
	}
}
