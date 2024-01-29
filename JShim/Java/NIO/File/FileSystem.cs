
using System;
using System.Collections.Generic;
using Java.NIO.File.SPI;

namespace Java.NIO.File
{
	/// <summary>
	/// Description of FileSystem.
	/// </summary>
	public abstract class FileSystem
	{
		protected FileSystem() : base()
		{
		}
		
		public abstract void close();
		public abstract IEnumerable<FileStore> GetFileStores();
		public abstract Path GetPath(params string[] subpaths);
//		public abstract PathMatcher GetPathMatcher(string syntaxAndPattern);
		public abstract IEnumerable<Path> GetRootDirectories();
		public abstract string GetSeparator();
//		public abstract UserPrincipalLookupService	GetUserPrincipalLookupService();
		public abstract bool IsOpen();
		public abstract bool IsReadOnly();
//		public abstract WatchService NewWatchService();
		public abstract FileSystemProvider Provider();
	}
}
