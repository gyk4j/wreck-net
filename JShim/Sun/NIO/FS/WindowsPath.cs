
using System;
using System.Collections.Generic;
using JShim.NIO.File;

namespace Sun.NIO.FS
{
	/// <summary>
	/// Description of WindowsPath.
	/// </summary>
	public class WindowsPath : Path
	{
		// The maximum path that does not require long path prefix. On Windows
		// the maximum path is 260 minus 1 (NUL) but for directories it is 260
		// minus 12 minus 1 (to allow for the creation of a 8.3 file in the
		// directory).
		private const int MAX_PATH = 247;

		// Maximum extended-length path
		private const int MAX_LONG_PATH = 32000;

		// FIXME - eliminate this reference to reduce space
		private readonly WindowsFileSystem fs;

		// path type
		private readonly WindowsPathType type;
		// root component (may be empty)
		private readonly string root;
		// normalized path
		private readonly string path;

		// the path to use in Win32 calls. This differs from path for relative
		// paths and has a long path prefix for all paths longer than MAX_PATH.
		// Typed WeakReference is only available from .NET Framework 4.5 onward.
//		private WeakReference pathForWin32Calls;

		// offsets into name components (computed lazily)
//		private volatile int[] offsets;

		// computed hash code (computed lazily, no need to be volatile)
//		private int hash;
		
		/// <summary>
		/// Initializes a new instance of this class.
		/// </summary>
		/// <param name="fs"></param>
		/// <param name="type"></param>
		/// <param name="root"></param>
		/// <param name="path"></param>
		public WindowsPath(WindowsFileSystem fs,
		                   WindowsPathType type,
		                   string root,
		                   string path)
		{
			this.fs = fs;
			this.type = type;
			this.root = root;
			this.path = path;
		}
		
		public int CompareTo(Path other)
		{
			throw new NotImplementedException();
		}
		
		public bool EndsWith(Path other)
		{
			throw new NotImplementedException();
		}
		
		public bool EndsWith(string other)
		{
			throw new NotImplementedException();
		}
		
		public Path GetFileName()
		{
			throw new NotImplementedException();
		}
		
		public FileSystem GetFileSystem()
		{
			return fs;
		}
		
		public Path GetName(int index)
		{
			throw new NotImplementedException();
		}
		
		public int GetNameCount()
		{
			throw new NotImplementedException();
		}
		
		public Path GetParent()
		{
			throw new NotImplementedException();
		}
		
		public Path GetRoot()
		{
			throw new NotImplementedException();
		}
		
		public int HashCode()
		{
			throw new NotImplementedException();
		}
		
		public bool IsAbsolute()
		{
			throw new NotImplementedException();
		}
		
		public IEnumerable<Path> Iterator()
		{
			throw new NotImplementedException();
		}
		
		public Path Normalize()
		{
			throw new NotImplementedException();
		}
		
		public Path Relativize(Path other)
		{
			throw new NotImplementedException();
		}
		
		public Path Resolve(Path other)
		{
			throw new NotImplementedException();
		}
		
		public Path Resolve(string other)
		{
			throw new NotImplementedException();
		}
		
		public Path ResolveSibling(Path other)
		{
			throw new NotImplementedException();
		}
		
		public Path ResolveSibling(string other)
		{
			throw new NotImplementedException();
		}
		
		public bool StartsWith(Path other)
		{
			throw new NotImplementedException();
		}
		
		public bool StartsWith(string other)
		{
			throw new NotImplementedException();
		}
		
		public Path SubPath(int beginIndex, int endIndex)
		{
			throw new NotImplementedException();
		}
		
		public Path ToAbsolutePath()
		{
			throw new NotImplementedException();
		}
		
		public System.IO.FileSystemInfo ToFile()
		{
			throw new NotImplementedException();
		}
		
		public Path ToRealPath(params LinkOption[] options)
		{
			throw new NotImplementedException();
		}
		
		public Uri ToURI()
		{
			throw new NotImplementedException();
		}
	}
}
