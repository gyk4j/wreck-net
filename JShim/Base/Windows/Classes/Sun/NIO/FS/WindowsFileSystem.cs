
using System;
using System.Collections.Generic;
using JShim.NIO.File;
using JShim.NIO.File.SPI;

namespace Sun.NIO.FS
{
	/// <summary>
	/// Description of WindowsFileSystem.
	/// </summary>
	public class WindowsFileSystem : FileSystem
	{
		public WindowsFileSystem()
		{
		}
		
		public override void close()
		{
			
		}
		
		public override IEnumerable<FileStore> GetFileStores()
		{
			return null;
		}
		
		public override Path GetPath(params string[] subpaths)
		{
			return null;
		}
		
//		public override PathMatcher GetPathMatcher(string syntaxAndPattern) { }
		
		public override IEnumerable<Path> GetRootDirectories()
		{
			return null;
		}
		
		public override string GetSeparator()
		{
			return null;
		}
		
//		public override UserPrincipalLookupService	GetUserPrincipalLookupService() { }
		
		public override bool IsOpen()
		{
			return false;
		}
		
		public override bool IsReadOnly()
		{
			return false;
		}
		
//		public override WatchService NewWatchService() { }
		
		public override FileSystemProvider Provider()
		{
			return null;
		}
	}
}
