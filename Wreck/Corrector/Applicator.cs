﻿
using System;
using System.IO;

namespace Wreck.Corrector
{
	/// <summary>
	/// Description of Corrections.
	/// </summary>
	public class Applicator : ICorrector
	{
		public bool ByCreationMetadata(FileSystemInfo fsi, DateTime? creation) 
		{
			bool done = false;

			if (creation != null && fsi.CreationTime.CompareTo(creation) > 0) {
				fsi.CreationTime = (DateTime) creation;
				done = true;
			}

			return done;
		}

		public bool ByLastWriteMetadata(FileSystemInfo fsi, DateTime? lastWrite)
		{
			bool done = false;

			if (lastWrite != null && fsi.LastWriteTime.CompareTo(lastWrite) > 0) {
				fsi.LastWriteTime = (DateTime) lastWrite;
				done = true;
			}

			return done;
		}

		public bool ByLastAccessMetadata(FileSystemInfo fsi, DateTime? lastAccess)
		{
			bool done = false;

			if (lastAccess != null && fsi.LastAccessTime.CompareTo(lastAccess) > 0) {
				fsi.LastAccessTime = (DateTime) lastAccess;
				done = true;
			}

			return done;
		}

		public bool ByLastWriteTime(FileSystemInfo fsi)
		{
			bool done = false;

			if (fsi.CreationTime.CompareTo(fsi.LastWriteTime) > 0) {
				fsi.CreationTime = fsi.LastWriteTime;
				done = true;
			}

			if (fsi.LastAccessTime.CompareTo(fsi.LastWriteTime) > 0) {
				fsi.LastAccessTime = fsi.LastWriteTime;
				done = true;
			}

			return done;
		}
	}
}