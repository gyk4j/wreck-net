using System;

namespace Java.NIO.File
{
	/// <summary>
	/// Description of StandardCopyOption.
	/// </summary>
	public enum StandardCopyOption
	{
		/// <summary>
		/// Move the file as an atomic file system operation.
		/// </summary>
		AtomicMove,
		
		/// <summary>
		/// Copy attributes to the new file.
		/// </summary>
		CopyAttributes,
		
		/// <summary>
		/// Replace an existing file if it exists.
		/// </summary>
		ReplaceExisting
	}
}
