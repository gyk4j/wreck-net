
using System;
using System.IO;
using System.Diagnostics;

namespace Wreck.Corrector
{
	/// <summary>
	/// Description of Previewer.
	/// </summary>
	public class Previewer : ICorrector
	{
		public void ByCreationMetadata(FileSystemInfo fsi, DateTime? creation)
		{
			Debug.Print("MC: {0}", fsi.Name);
		}

		public void ByLastWriteMetadata(FileSystemInfo fsi, DateTime? lastWrite)
		{
			Debug.Print("MW: {0}", fsi.Name);
		}

		public void ByLastAccessMetadata(FileSystemInfo fsi, DateTime? lastAccess)
		{
			Debug.Print("MA: {0}", fsi.Name);
		}

		public void ByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
			Debug.Print("LW: {0}", fsi.Name);
		}
	}
}
