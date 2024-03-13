
using System;
using System.IO;
using System.Text;

using Java.Time;
using log4net;

namespace Java.NIO.File.Attribute
{
	/// <summary>
	/// A file attribute view that provides a view of a basic set of file
	/// attributes common to many file systems. The basic set of file
	/// attributes consist of mandatory and optional file attributes as defined
	/// by the BasicFileAttributes interface.
	/// 
	/// The file attributes are retrieved from the file system as a bulk
	/// operation by invoking the readAttributes method. This class also
	/// defines the setTimes method to update the file's time attributes.
	/// 
	/// Where dynamic access to file attributes is required, the attributes
	/// supported by this attribute view have the following names and types:
	/// <list type="table">
	/// 	<listheader>
	/// 		<term>Name</term>
	/// 		<description>Type</description>
	/// 	</listheader>
	/// 	<item>
	/// 		<term>"lastModifiedTime"</term>
	/// 		<description><c>DateTime</c></description>
	/// 	</item>
	/// 	<item>
	/// 		<term>"lastAccessTime"</term>
	/// 		<description><c>DateTime</c></description>
	/// 	</item>
	/// 	<item>
	/// 		<term>"creationTime"</term>
	/// 		<description><c>DateTime</c></description>
	/// 	</item>
	/// 	<item>
	/// 		<term>"size"</term>
	/// 		<description><c>long</c></description>
	/// 	</item>
	/// 	<item>
	/// 		<term>"isRegularFile"</term>
	/// 		<description><c>bool</c></description>
	/// 	</item>
	/// 	<item>
	/// 		<term>"isDirectory"</term>
	/// 		<description><c>bool</c></description>
	/// 	</item>
	/// 	<item>
	/// 		<term>"isSymbolicLink"</term>
	/// 		<description><c>bool</c></description>
	/// 	</item>
	/// 	<item>
	/// 		<term>"isOther"</term>
	/// 		<description><c>bool</c></description>
	/// 	</item>
	/// 	<item>
	/// 		<term>"fileKey"</term>
	/// 		<description><c>object</c></description>
	/// 	</item>
	/// </list>
	/// 
	/// <para>
	/// The <c>GetAttribute</c> method may be used to read any of these
	/// attributes as if by invoking the <c>ReadAttributes()</c> method.
	/// </para>
	/// 
	/// <para>
	/// The <c>SetAttribute</c> method may be used to update the file's last
	/// modified time, last access time or create time attributes as if by
	/// invoking the <c>SetTimes</c> method.
	/// </para>
	/// </summary>
	public class BasicFileAttributeView : FileAttributeView
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(BasicFileAttributeView));
		
		public const string Basic = "basic";
		
		private FileSystemInfo file;
		
		public BasicFileAttributeView(FileSystemInfo file)
		{
			this.file = file;
		}
		
		public void SetTimes(DateTime? modified, DateTime? accessed, DateTime? creation)
		{
			bool readOnly = DisableWriteProtection();
			
			StringBuilder sb = new StringBuilder();
			
			if(creation.HasValue &&
			   (
			   	(creation.Value.Kind == DateTimeKind.Local &&
			   	 !file.CreationTime.Equals(creation.Value)) ||
			   	(creation.Value.Kind == DateTimeKind.Utc &&
			   	 !file.CreationTimeUtc.Equals(creation.Value))
			   )
			  )
			{
				if(creation.Value.Kind == DateTimeKind.Local)
					file.CreationTime = creation.Value;
				else if(creation.Value.Kind == DateTimeKind.Utc)
					file.CreationTimeUtc = creation.Value;
				
				sb.Append(Instant.From(creation.Value).ToString());
			}
			else
				sb.Append("-");
			
			sb.Append(", ");
			
			if(modified.HasValue &&
			   (
			   	(modified.Value.Kind == DateTimeKind.Local &&
			   	 !file.LastWriteTime.Equals(modified.Value)
			   	) ||
			   	(modified.Value.Kind == DateTimeKind.Utc &&
			   	 !file.LastWriteTimeUtc.Equals(modified.Value)
			   	)
			   )
			  )
			{
				if(modified.Value.Kind == DateTimeKind.Local)
					file.LastWriteTime = modified.Value;
				else if(modified.Value.Kind == DateTimeKind.Utc)
					file.LastWriteTimeUtc = modified.Value;
				
				sb.Append(Instant.From(modified.Value).ToString());
			}
			else
				sb.Append("-");
			
			sb.Append(", ");
			
			if(accessed.HasValue &&
			   (
			   	(accessed.Value.Kind == DateTimeKind.Local && 
			   	 !file.LastAccessTime.Equals(accessed.Value)) ||
			   	(accessed.Value.Kind == DateTimeKind.Utc &&
			   	!file.LastAccessTimeUtc.Equals(accessed.Value))
			   )
			  )
			{
				if(accessed.Value.Kind == DateTimeKind.Local)
					file.LastAccessTime = accessed.Value;
				else if(accessed.Value.Kind == DateTimeKind.Utc)
					file.LastWriteTimeUtc = accessed.Value;
				
				sb.Append(Instant.From(accessed.Value).ToString());
			}
			else
				sb.Append("-");
			
			EnableWriteProtection(readOnly);
			
			string change = sb.ToString();
			
			if(!"-, -, -".Equals(change)) // "-, -, -" = no change.
				LOG.DebugFormat("{0}: {1}", file.FullName, change);
			else
				LOG.DebugFormat("Unchanged: {0}", file.FullName);
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
		
		public string Name()
		{
			return Basic;
		}
	}
}
