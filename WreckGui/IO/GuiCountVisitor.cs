
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
			if(file.Name.Equals(R.Strings.SkipDesktopIni) ||
			   file.Name.Equals(R.Strings.LogFileName))
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
