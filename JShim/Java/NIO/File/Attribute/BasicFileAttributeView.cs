
using System;
using System.IO;
using log4net;

namespace Java.NIO.File.Attribute
{
	/// <summary>
	/// Description of BasicFileAttributeView.
	/// </summary>
	public class BasicFileAttributeView
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(BasicFileAttributeView));
		
		private FileSystemInfo file;
		
		public BasicFileAttributeView(FileSystemInfo file)
		{
			this.file = file;
		}
		
		public void SetTimes(DateTime? modified, DateTime? accessed, DateTime? creation)
		{
			bool readOnly = DisableWriteProtection();
			
			if(creation.HasValue)
				file.CreationTime = creation.Value;
			
			if(modified.HasValue)
				file.LastWriteTime = modified.Value;
			
			if(accessed.HasValue)
				file.LastAccessTime = accessed.Value;
			
			EnableWriteProtection(readOnly);
			
			bool changed = creation.HasValue || modified.HasValue || accessed.HasValue;
			if(changed)
			{
				LOG.InfoFormat("Updated {0}: {1}, {2}, {3}",
				               file.FullName,
				               (creation.HasValue)? creation.Value.ToString() : "-",
				               (modified.HasValue)? modified.Value.ToString() : "-",
				               (accessed.HasValue)? accessed.Value.ToString() : "-");
			}
		}
		
		private bool DisableWriteProtection()
		{
			bool readOnly = false;
			
			if(file is FileInfo)
			{
				FileInfo fi = (FileInfo) file;
				readOnly = fi.IsReadOnly;
				fi.IsReadOnly = false;
				
				if(readOnly)
					LOG.WarnFormat("{0} is read-only!", fi.FullName);
			}
			
			return readOnly;
		}
		
		private void EnableWriteProtection(bool writeProtect)
		{
			if(file is FileInfo)
			{
				FileInfo fi = (FileInfo) file;
				fi.IsReadOnly = writeProtect;
			}
		}
	}
}
