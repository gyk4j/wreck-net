
using System;
using System.IO;
using System.Diagnostics;

using log4net;
using log4net.Config;

namespace Wreck.Corrector
{
	/// <summary>
	/// Description of Previewer.
	/// </summary>
	public class Previewer : ICorrector
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Previewer));
		
		public void ByCreationMetadata(FileSystemInfo fsi, DateTime? creation)
		{
			log.DebugFormat("MC: {0} {1}", fsi.Name, creation.Value.ToString());
		}

		public void ByLastWriteMetadata(FileSystemInfo fsi, DateTime? lastWrite)
		{
			log.DebugFormat("MW: {0} {1}", fsi.Name, lastWrite.Value.ToString());
		}

		public void ByLastAccessMetadata(FileSystemInfo fsi, DateTime? lastAccess)
		{
			log.DebugFormat("MA: {0} {1}", fsi.Name, lastAccess.Value.ToString());
		}

		public void ByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
			log.DebugFormat("LW: {0} {1}", fsi.Name, creationOrLastAccess.ToString());
		}
	}
}
