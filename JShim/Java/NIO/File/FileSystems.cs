
using System;
using Sun.NIO.FS;

namespace Java.NIO.File
{
	/// <summary>
	/// Description of FileSystems.
	/// </summary>
	public class FileSystems
	{
		protected static readonly FileSystem defaultFileSystem = new WindowsFileSystem();
		
		public static FileSystem GetDefault()
		{
			return defaultFileSystem;
		}
	}
}
