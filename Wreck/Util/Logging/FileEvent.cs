
using System;

namespace Wreck.Util.Logging
{
	/// <summary>
	/// Description of FileEvent.
	/// </summary>
	public enum FileEvent
	{
		DirectoryFound,
		FileFound,
		FileError,
		CorrectibleCreation,
		CorrectibleModified,
		CorrectibleAccessed,
		UncorrectibleCreation,
		UncorrectibleModified,
		UncorrectibleAccessed
	}
}
