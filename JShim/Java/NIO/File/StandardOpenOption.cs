
using System;

namespace Java.NIO.File
{
	/// <summary>
	/// Description of StandardOpenOption.
	/// </summary>
	public enum StandardOpenOption
	{
		/// <summary>
		/// If the file is opened for WRITE access then bytes will be written 
		/// to the end of the file rather than the beginning.
		/// </summary>
		Append,
		
		/// <summary>
		/// Create a new file if it does not exist.
		/// </summary>
		Create,
		
		/// <summary>
		/// Create a new file, failing if the file already exists.
		/// </summary>
		CreateNew,
		
		/// <summary>
		/// Delete on close.
		/// </summary>
		DeleteOnClose,
		
		/// <summary>
		/// Requires that every update to the file's content be written 
		/// synchronously to the underlying storage device.
		/// </summary>
		Dsync,
		
		/// <summary>
		/// Open for read access.
		/// </summary>
		Read,
		
		/// <summary>
		/// Sparse file.
		/// </summary>
		Sparse,
		
		/// <summary>
		/// Requires that every update to the file's content or metadata be 
		/// written synchronously to the underlying storage device.
		/// </summary>
		Sync,
		
		/// <summary>
		/// If the file already exists and it is opened for WRITE access, then 
		/// its length is truncated to 0.
		/// </summary>
		TruncateExisting,
		
		/// <summary>
		/// Open for write access.
		/// </summary>
		Write
	}
	
}
