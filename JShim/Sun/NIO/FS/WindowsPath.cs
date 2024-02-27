
using System;
using System.Collections.Generic;
using System.Text;

using Java.NIO.File;

namespace Sun.NIO.FS
{
	/// <summary>
	/// Description of WindowsPath.
	/// </summary>
	public class WindowsPath : Path
	{
		public const string DefaultRoot = @"C:\";
		public static readonly char[] Separators =
		{
			System.IO.Path.DirectorySeparatorChar,
			System.IO.Path.AltDirectorySeparatorChar
		};
		
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
		
		/// <summary>
		/// Compares two abstract paths lexicographically.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public int CompareTo(Path other)
		{
			return 0;
		}
		
		/// <summary>
		/// Tests if this path ends with the given path.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool EndsWith(Path other)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Tests if this path ends with a Path, constructed by converting the
		/// given path string, in exactly the manner specified by the
		/// <c>endsWith(Path)</c> method.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool EndsWith(string other)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Tests this path for equality with the given object.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public override bool Equals(object other)
		{
			return Equals(other as WindowsPath);
		}
		
		/// <summary>
		/// Tests this path for equality with the given object.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(WindowsPath other)
		{
			return other != null &&
				root.Equals(other.root) &&
				path.Equals(other.path);
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
			string abs = System.IO.Path.Combine(root, path);
			string parent = System.IO.Path.GetDirectoryName(abs);
			string file = System.IO.Path.GetFileName(abs);
			return new WindowsPath(fs, WindowsPathType.Absolute, parent, file);
		}
		
		public Path GetRoot()
		{
			string root = System.IO.Path.GetPathRoot(this.root);
			return new WindowsPath(fs, WindowsPathType.Absolute, root, string.Empty);
		}
		
		public override int GetHashCode()
		{
			unchecked // Allow arithmetic overflow, numbers will just "wrap around"
			{
				int hashcode = 1430287;
				hashcode = hashcode * 7302013 ^ root.GetHashCode();
				hashcode = hashcode * 7302013 ^ path.GetHashCode();
				return hashcode;
			}
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
			return ToAbsolutePath();
		}
		
		public Path Relativize(Path other)
		{
			string[] s = ToString().Split(Separators);
			string[] o = other.ToString().Split(Separators);
			
			// Ignore the common subpaths
			int max = Math.Max(s.Length, o.Length);
			List<string> sd = new List<string>();
			List<string> od = new List<string>();
			for(int i = 0; i < max; i++)
			{
				if(i < s.Length && i < o.Length && s[i].Equals(o[i]))
					continue;
				
				if(i < s.Length)
					sd.Add(s[i]);
				
				if(i < o.Length)
					od.Add(o[i]);
			}
			
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < sd.Count; i++)
			{
				sb.Append(System.IO.Path.DirectorySeparatorChar);
				sb.Append("..");
			}
			
			foreach(string op in od)
			{
				sb.Append(System.IO.Path.DirectorySeparatorChar);
				sb.Append(op);
			}
			
			string path = sb.ToString();
			
			return new WindowsPath(fs, WindowsPathType.Relative, root, path);
		}
		
		public Path Resolve(Path other)
		{
			return Resolve(other.ToString());
		}
		
		public Path Resolve(string other)
		{
			return null;
		}
		
		public Path ResolveSibling(Path other)
		{
			return ResolveSibling(other.ToString());
		}
		
		public Path ResolveSibling(string other)
		{
			return null;
		}
		
		public bool StartsWith(Path other)
		{
			return StartsWith(other.ToString());
		}
		
		public bool StartsWith(string other)
		{
			string p = ToString();
			return p.StartsWith(other);
		}
		
		public Path SubPath(int beginIndex, int endIndex)
		{
			string[] subpaths = this.path.Split(Separators);
			StringBuilder sb = new StringBuilder();
			for(int i = beginIndex; i < endIndex; i++)
			{
				sb.Append(System.IO.Path.DirectorySeparatorChar);
				sb.Append(subpaths[i]);
			}
			string path = sb.ToString();
			return new WindowsPath(fs, type, root, path);
		}
		
		public Path ToAbsolutePath()
		{
			string root = !string.IsNullOrEmpty(this.root)?
				this.root :
				DefaultRoot;
			
			string path = this.path.StartsWith(root)?
				this.path.Replace(root, string.Empty) :
				this.path;

			return new WindowsPath(fs, WindowsPathType.Absolute, root, path);
		}
		
		public System.IO.FileSystemInfo ToFile()
		{
			System.IO.FileSystemInfo fsi;
			
			string p = ToString();
			if(System.IO.Directory.Exists(p))
				fsi = new System.IO.DirectoryInfo(p);
			else if(System.IO.File.Exists(p))
				fsi = new System.IO.FileInfo(p);
			else
			{
				if(!string.IsNullOrEmpty(System.IO.Path.GetExtension(p)))
					fsi = new System.IO.FileInfo(p);
				else
					fsi = new System.IO.DirectoryInfo(p);
			}
			
			return fsi;
		}
		
		/// <summary>
		/// Returns the real path of an existing file.
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public Path ToRealPath(params LinkOption[] options)
		{
			throw new NotImplementedException();
		}
		
		public Uri ToURI()
		{
			throw new NotImplementedException();
		}
		
		public override string ToString()
		{
			return string.IsNullOrEmpty(this.root)?
				System.IO.Path.Combine(DefaultRoot, this.path) :
				System.IO.Path.Combine(this.root, this.path);
		}
	}
}
