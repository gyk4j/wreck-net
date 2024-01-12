
using System;
using System.Collections.Generic;
using System.IO;

using Wreck.Resources;

namespace Wreck.IO.Reader.Fs
{
	/// <summary>
	/// Description of FileSystemReader.
	/// </summary>
	public class FileSystemReader : AbstractTimestampReader
	{
		protected static readonly string[] CREATION =
		{
			R.strings.FS_CREATION
		};
		
		protected static readonly string[] MODIFIED =
		{
			R.strings.FS_MODIFIED
		};
		
		public FileSystemReader() : base()
		{
		}
		
		public new string[] creation()
		{
			return CREATION;
		}
		
		public new string[] modified()
		{
			return MODIFIED;
		}
		
		public new string[] accessed()
		{
			return MODIFIED;
		}
		
		public new void extract(FileSystemInfo file, List<Metadata> metadata)
		{
			int order = file.CreationTime.CompareTo(file.LastWriteTime);
			DateTime earlier = (order < 0)? file.CreationTime: file.LastWriteTime;
			DateTime later = file.LastWriteTime;
			
			Add(metadata, CREATION[0], earlier.ToString(), earlier);
			Add(metadata, MODIFIED[0], later.ToString(), later);
		}
	}
}
