
using System;
using System.Collections.Generic;

namespace Java.NIO.File.SPI
{
	/// <summary>
	/// Service-provider class for file systems. The methods defined by the Files class will typically delegate to an instance of this class.
	/// A file system provider is a concrete implementation of this class that implements the abstract methods defined by this class. A provider is identified by a URI scheme. The default provider is identified by the URI scheme "file". It creates the FileSystem that provides access to the file systems accessible to the Java virtual machine. The FileSystems class defines how file system providers are located and loaded. The default provider is typically a system-default provider but may be overridden if the system property java.nio.file.spi.DefaultFileSystemProvider is set. In that case, the provider has a one argument constructor whose formal parameter type is FileSystemProvider. All other providers have a zero argument constructor that initializes the provider.
	///
	/// A provider is a factory for one or more FileSystem instances. Each file system is identified by a URI where the URI's scheme matches the provider's scheme. The default file system, for example, is identified by the URI "file:///". A memory-based file system, for example, may be identified by a URI such as "memory:///?name=logfs". The newFileSystem method may be used to create a file system, and the getFileSystem method may be used to obtain a reference to an existing file system created by the provider. Where a provider is the factory for a single file system then it is provider dependent if the file system is created when the provider is initialized, or later when the newFileSystem method is invoked. In the case of the default provider, the FileSystem is created when the provider is initialized.
	///
	/// All of the methods in this class are safe for use by multiple concurrent threads.
	/// </summary>
	public abstract class FileSystemProvider
	{
		protected FileSystemProvider()
		{
		}
		
//		public abstract void CheckAccess(Path path, params AccessMode[] modes);
//		public abstract void Copy(Path source, Path target, params CopyOption[] options);
//		public abstract void CreateDirectory<T>(Path dir, params FileAttribute<T>[] attrs);
//		public void CreateLink(Path link, Path existing) { }
//		public void CreateSymbolicLink<T>(Path link, Path target, params FileAttribute<T>[] attrs) { }
//		public abstract void Delete(Path path);
//		public bool DeleteIfExists(Path path) { }
//		public abstract GetFileAttributeView<V>(Path path, Type<V> type, params LinkOption[] options) where V : FileAttributeView;
//		public abstract FileStore GetFileStore(Path path);
//		public abstract FileSystem GetFileSystem(URI uri);
//		public abstract Path GetPath(URI uri);
//		public abstract string GetScheme();
//		public static List<FileSystemProvider> InstalledProviders()
//		{
//			return new List<FileSystemProvider>();
//		}
//		public abstract bool IsHidden(Path path);
//		public abstract bool IsSameFile(Path path, Path path2);
//		public abstract void Move(Path source, Path target, params CopyOption[] options);
//		public AsynchronousFileChannel NewAsynchronousFileChannel<T, U>(Path path, List<T> options, ExecutorService executor, params FileAttribute<U>[] attrs) where T : OpenOption;
//		public abstract SeekableByteChannel NewByteChannel<T>(Path path, Set<T> options, params FileAttribute<U>[] attrs) where TimeSpan : OpenOption;
//		public abstract DirectoryStream<Path> NewDirectoryStream<T>(Path dir, DirectoryStream.Filter<T> filter) where T : Path;
//		public FileChannel NewFileChannel<T, U>(Path path, Set<T> options, params FileAttribute<U>[] attrs) where T : OpenOption;
//		public FileSystem NewFileSystem<T>(Path path, IDictionary<string,T> env)
//		{
//			return null;
//		}
//		public abstract FileSystem NewFileSystem<T>(URI uri, IDictionary<string,T> env);
//		public InputStream NewInputStream(Path path, params OpenOption[] options)
//		{
//			return null;
//		}
//		public OutputStream NewOutputStream(Path path, params OpenOption[] options)
//		{
//			return null;
//		}
//		public abstract A ReadAttributes<A>(Path path, Type<A> type, params LinkOption[] options) where A : BasicFileAttributes;
	}	
}
