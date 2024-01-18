
using System;

namespace JShim.NIO.File
{
	/// <summary>
	/// Description of FileStore.
	/// </summary>
	public abstract class FileStore
	{
		protected FileStore()
		{
		}
		
		public abstract object GetAttribute(string attribute);
		public abstract V GetFileStoreAttributeView<V>(Type type) where V : FileStoreAttributeView;
		public abstract long GetTotalSpace();
		public abstract long GetUnallocatedSpace();
		public abstract long GetUsableSpace();
		public abstract bool IsReadOnly();
		public abstract string	Name();
		public abstract bool SupportsFileAttributeView<T>(Type type) where T : FileAttributeView;
		public abstract bool SupportsFileAttributeView(string name);
		public abstract string	Type();
	}
}
