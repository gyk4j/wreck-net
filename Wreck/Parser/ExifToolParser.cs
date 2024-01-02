
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

using log4net;
using log4net.Config;

using ExifToolWrapper;

namespace Wreck.Parser
{
	/// <summary>
	/// Description of ExifToolParser.
	/// </summary>
	public class ExifToolParser : IFileDateable, IDisposable
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ExifToolParser));
		
		private ExifTool exifTool;
		
		public ExifToolParser()
		{
			exifTool = new ExifTool();
		}
		
		~ExifToolParser()
		{
			log.DebugFormat("Called ~ExifToolParser");
			Dispose();
		}
		
		public void Dispose()
		{
			log.DebugFormat("Called ExifToolParser.Dispose()");
			exifTool.Dispose(); // Always stop the ExifTool process.
			GC.SuppressFinalize(this);
		}
		
		public void Get(string path, ICollection<KeyValuePair<string, string>> properties)
		{
			exifTool.GetProperties(path, properties);
		}
		
		public void GetDateTimes(
			FileInfo fi, 
			out DateTime? creationTime, 
			out DateTime? lastWriteTime, 
			out DateTime? lastAccessTime)
		{
			Dictionary<string, string> properties = new Dictionary<string, string>();
			Get(fi.FullName, properties);
			
			DateTime? createDate = new Nullable<DateTime>(); 
			DateTime? modifyDate = new Nullable<DateTime>(); 
			DateTime? dateTimeOriginal = new Nullable<DateTime>();
			
			IDictionaryEnumerator e = properties.GetEnumerator();
			while(e.MoveNext())
			{
				DictionaryEntry entry = e.Entry;
				string tag = (string) entry.Key;
				if(!tag.StartsWith("System:") && tag.ToLower().Contains("date"))
				{
					string k = tag.Substring(tag.IndexOf(':')+1).Trim();
					
					DateTime parsed;
					
					switch(k)
					{
						case "CreateDate":
							ExifTool.TryParseDate((string) entry.Value, DateTimeKind.Local, out parsed);
							createDate = parsed;
							break;
						case "ModifyDate":
							ExifTool.TryParseDate((string) entry.Value, DateTimeKind.Local, out parsed);
							modifyDate = parsed;
							break;
						case "DateTimeOriginal":
							ExifTool.TryParseDate((string) entry.Value, DateTimeKind.Local, out parsed);
							dateTimeOriginal = parsed;
							break;
						default:
							break;
					}
//					log.DebugFormat("exiftool: {0}: {1} = {2}", fi.Name, entry.Key, entry.Value);
				}
			}
			
			properties.Clear();			
			
			if(createDate.HasValue)
				creationTime = createDate;
			else if(dateTimeOriginal.HasValue)
				creationTime = dateTimeOriginal;
			else
				creationTime = null;

			if(modifyDate.HasValue)
				lastWriteTime = modifyDate;
			else if(dateTimeOriginal.HasValue)
				lastWriteTime = dateTimeOriginal;
			else
				lastWriteTime = null;
			
			if(creationTime.HasValue && lastWriteTime.HasValue)
				lastAccessTime = lastWriteTime.Value.CompareTo(creationTime.Value) > 0? 
					lastWriteTime.Value: creationTime.Value;
			else if(lastWriteTime.HasValue)
				lastAccessTime = lastWriteTime.Value;
			else if(creationTime.HasValue)
				lastAccessTime = creationTime.Value;
			else
				lastAccessTime = null;
		}
	}
}
