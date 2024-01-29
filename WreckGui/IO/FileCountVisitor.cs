
using System;
using System.IO;
using Java.NIO.File;
using Wreck.Resources;

namespace Wreck.IO
{
	class GuiCountVisitor : SimpleFileVisitor
	{
		GuiWorker progressWorker = null;
		
		public GuiCountVisitor(GuiWorker progressWorker)
		{
			this.progressWorker = progressWorker;
		}
		
		public override FileVisitResult PreVisitDirectory(DirectoryInfo dir)
		{
			progressWorker.IncrementTotal();
			return FileVisitResult.Continue;
		}
		
		public override FileVisitResult VisitFile(FileInfo file)
		{
			if(file.Name.Equals(R.strings.SKIP_DESKTOP_INI) ||
			   file.Name.Equals(R.strings.LOG_FILE_NAME))
				return FileVisitResult.Continue;
			
			progressWorker.IncrementTotal();
			return FileVisitResult.Continue;
		}

		public override FileVisitResult VisitFileFailed(FileSystemInfo file, IOException exc)
		{
			return FileVisitResult.Continue;
		}
	}
}
