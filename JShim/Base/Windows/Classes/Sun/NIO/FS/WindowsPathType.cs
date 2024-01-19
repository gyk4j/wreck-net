
using System;

namespace Sun.NIO.FS
{
	/// <summary>
	/// A type safe enum of Windows path types.
	/// </summary>
	public enum WindowsPathType
	{
		Absolute,                   //  C:\foo
		UNC,                        //  \\server\share\foo
		Relative,                   //  foo
		DirectoryRelative,         	//  \foo
		DriveRelative              	//  C:foo
	}
}
