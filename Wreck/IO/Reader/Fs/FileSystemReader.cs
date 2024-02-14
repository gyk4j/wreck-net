
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
			R.Strings.FsCreation
		};
		
		protected static readonly string[] MODIFIED =
		{
			R.Strings.FsModified
		};
		
		public FileSystemReader() : base()
		{
		}
		
		public override string[] Creation()
		{
			return CREATION;
		}
		
		public override string[] Modified()
		{
			return MODIFIED;
		}
		
		public override string[] Accessed()
		{
			return MODIFIED;
		}
		
		public override void Extract(FileSystemInfo file, List<Metadata> metadata)
		{
			int order = file.CreationTime.CompareTo(file.LastWriteTime);
			DateTime earlier = (order < 0)? file.CreationTime: file.LastWriteTime;
			DateTime later = file.LastWriteTime;
			
			Add(metadata, CREATION[0], earlier.ToString(), earlier);
			Add(metadata, MODIFIED[0], later.ToString(), later);
		}
	}
}
