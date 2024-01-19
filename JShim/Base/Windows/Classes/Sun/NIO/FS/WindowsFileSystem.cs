
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
			WindowsPathType type;
			string root;
			
			Regex absoluteRegex = new Regex("^[A-Za-z]" +  + System.IO.Path.VolumeSeparatorChar + @"\\", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			Regex driveRelativeRegex = new Regex("^[A-Za-z]" + System.IO.Path.VolumeSeparatorChar, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			
			string fullPath = string.Empty;
			foreach(string subpath in subpaths)
			{
				fullPath = System.IO.Path.Combine(fullPath, subpath);
			}
			
			if(absoluteRegex.IsMatch(subpaths[0].Trim()))	
				type = WindowsPathType.Absolute;
			else if(driveRelativeRegex.IsMatch(subpaths[0].Trim()))
				type = WindowsPathType.DriveRelative;
			else if(subpaths[0].Trim().StartsWith(@"\\"))
				type = WindowsPathType.UNC;
			else if(subpaths[0].Trim().StartsWith(Char.ToString(System.IO.Path.DirectorySeparatorChar)))
				type = WindowsPathType.DirectoryRelative;
			else
				type = WindowsPathType.Relative;
			
			root = System.IO.Path.IsPathRooted(fullPath)? 
				System.IO.Path.GetPathRoot(fullPath) :
				null;
			
			string path = System.IO.Path.GetFullPath(fullPath);
			
			if(!string.IsNullOrEmpty(root))
				path = path.Substring(root.Length);
			
			return new WindowsPath(this, type, root, path);
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
