
using System;
using System.IO;

namespace Wreck.Corrector
{
	public interface ICorrector
	{
		void ByCreationMetadata(FileSystemInfo fsi, DateTime? creation);
		void ByLastWriteMetadata(FileSystemInfo fsi, DateTime? lastWrite);
		void ByLastAccessMetadata(FileSystemInfo fsi, DateTime? lastAccess);
		void ByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess);
	}
}
