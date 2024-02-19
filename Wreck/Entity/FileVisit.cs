
using System;
using System.IO;

namespace Wreck.Entity
{
	/// <summary>
	/// Description of FileVisit.
	/// </summary>
	public class FileVisit
	{
		private FileSystemInfo file;
		private int progress;
		
		public FileVisit(FileSystemInfo file) : this(file, 0)
		{
		}

		public FileVisit(FileSystemInfo file, int progress)
		{
			this.file = file;
			this.progress = progress;
		}
		
		public FileSystemInfo File
		{
			get { return file; }
			set { File = value; }
		}
		
		public int Progress
		{
			get { return progress; }
			set { progress = value; }
		}
	}
}
