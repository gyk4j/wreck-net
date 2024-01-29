
using System;
using System.IO;

namespace Microsoft.NET
{
	/// <summary>
	/// Description of FileSystemUtils.
	/// </summary>
	public class FSUtils
	{
		public static bool IsTypeOf(FileSystemInfo fsi, FileAttributes attr)
		{
			return (fsi.Attributes & attr) == attr;
		}
		
		public static bool IsArchive(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.Archive);
		}
		
		public static bool IsCompressed(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.Compressed);
		}
		
		public static bool IsDevice(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.Device);
		}
		
		public static bool IsDirectory(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.Directory);
		}
		
		public static bool IsEncrypted(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.Encrypted);
		}
		
		public static bool IsHidden(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.Hidden);
		}
		
		public static bool IsNormal(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.Normal);
		}
		
		public static bool IsNotContentIndexed(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.NotContentIndexed);
		}
		
		public static bool IsOffline(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.Offline);
		}
		
		public static bool IsReadOnly(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.ReadOnly);
		}
		
		public static bool IsReparsePoint(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.ReparsePoint);
		}
		
		public static bool IsSparseFile(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.SparseFile);
		}
		
		public static bool IsSystem(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.System);
		}
		
		public static bool IsTemporary(FileSystemInfo fsi)
		{
			return IsTypeOf(fsi, FileAttributes.Temporary);
		}
		
		public static void rollback(DateTime actual, DateTime expected)
		{
			if(expected.CompareTo(actual) < 0)
				actual = new DateTime(expected.Ticks, DateTimeKind.Local);
		}
		
		/// <summary>
		/// Add or remove attributes from a file or directory.
		/// </summary>
		/// <param name="path">path of file or directory to update</param>
		/// <param name="attributes">attributes to act on</param>
		/// <param name="enable">To add (true) or remove (false) the attributes</param>
		public static void SetAttribute(string path, FileAttributes attributes, bool enable)
		{
			FileAttributes current = File.GetAttributes(path);
			FileAttributes attrs = enable ? 
				current | attributes: 	// Enable attributes
				current & ~attributes;	// Disable attributes
				
			File.SetAttributes(path, attrs);
		}
	}
}
