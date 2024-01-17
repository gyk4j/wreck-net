
using System;
using System.IO;

namespace Wreck.Entity
{
	/// <summary>
	/// Description of FileEntity.
	/// </summary>
	public class FileEntity
	{
		private FileSystemInfo path;
		protected DateTime creation;
		protected DateTime modified;
		protected DateTime metadata;
		private TimeSpan period;
		
		public FileEntity(FileSystemInfo path, DateTime creation, DateTime modified, DateTime metadata, TimeSpan period) : base()
		{
			this.path = path;
			this.creation = creation;
			this.modified = modified;
			this.metadata = metadata;
			this.period = period;
		}
		
		public FileSystemInfo Path
		{
			get { return path; }
			set { path = value; }
		}
		
		public DateTime Creation
		{
			get { return creation; }
			set { creation = value; }
		}
		
		public DateTime Modified
		{
			get { return modified; }
			set { modified = value; }
		}
		
		public DateTime Metadata
		{
			get { return metadata; }
			set { metadata = value; }
		}
		
		public TimeSpan Period
		{
			get { return period; }
			set { period = value; }
		}
	}
}
