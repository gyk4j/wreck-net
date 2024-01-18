
using System;
using System.Collections.Generic;
using System.IO;

namespace JShim.NIO.File
{
	/// <summary>
	/// Description of Path.
	/// </summary>
	public interface Path
	{
		int	CompareTo(Path other);
		bool EndsWith(Path other);
		bool EndsWith(string other);
		bool Equals(object other);
		Path GetFileName();
		FileSystem GetFileSystem();
		Path GetName(int index);
		int GetNameCount();
		Path GetParent();
		Path GetRoot();
		int HashCode();
		bool IsAbsolute();
		IEnumerable<Path> Iterator();
		Path Normalize();
//		WatchKey Register<T>(WatchService watcher, params WatchEvent.Kind<T>[] events);
//		WatchKey Register<T>(WatchService watcher, params WatchEvent.Kind<T>[] events, params WatchEvent.Modifier[] modifiers);
		Path Relativize(Path other);
		Path Resolve(Path other);
		Path Resolve(string other);
		Path ResolveSibling(Path other);
		Path ResolveSibling(string other);
		bool StartsWith(Path other);
		bool StartsWith(string other);
		Path SubPath(int beginIndex, int endIndex);
		Path ToAbsolutePath();
		FileSystemInfo ToFile();
		Path ToRealPath(params LinkOption[] options);
		string ToString();
		Uri ToURI();
	}
}
