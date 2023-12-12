
using System;
using System.IO;

namespace Wreck
{
	/// <summary>
	/// Description of Previewer.
	/// </summary>
	public class Previewer : ICorrector
	{
		public bool ByCreationMetadata(FileSystemInfo fsi, DateTime? creation)
		{
			bool done = false;

			if (creation != null && fsi.CreationTime.CompareTo(creation) > 0) {
				done = true;
			}

			return done;
		}

		public bool ByLastWriteMetadata(FileSystemInfo fsi, DateTime? lastWrite)
		{
			bool done = false;

			if (lastWrite != null && fsi.LastWriteTime.CompareTo(lastWrite) > 0) {
				done = true;
			}

			return done;
		}

		public bool ByLastAccessMetadata(FileSystemInfo fsi, DateTime? lastAccess)
		{
			bool done = false;

			if (lastAccess != null && fsi.LastAccessTime.CompareTo(lastAccess) > 0) {
				done = true;
			}

			return done;
		}

		public bool ByLastWriteTime(FileSystemInfo fsi)
		{
			bool done = false;

			if (fsi.CreationTime.CompareTo(fsi.LastWriteTime) > 0) {
				done = true;
			}

			if (fsi.LastAccessTime.CompareTo(fsi.LastWriteTime) > 0) {
				done = true;
			}

			return done;
		}
	}
}
