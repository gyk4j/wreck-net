
using System;

namespace Java.NIO.File.Attribute
{
	/// <summary>
	/// An attribute view that is a read-only or updatable view of non-opaque 
	/// values associated with a file in a filesystem. This interface is 
	/// extended or implemented by specific file attribute views that define 
	/// methods to read and/or update the attributes of a file.
	/// </summary>
	public interface FileAttributeView : AttributeView
	{
	}
}
