
using System;
using System.Collections.Generic;

namespace Wreck.Parser
{
	/// <summary>
	/// Description of ExifToolParser.
	/// </summary>
	public class ExifToolParser : IDisposable
	{
		private ExifToolWrapper.ExifTool exifTool;
		
		public ExifToolParser()
		{
			exifTool = new ExifToolWrapper.ExifTool();
		}
		
		public void Dispose()
		{
			exifTool.Dispose(); // Always stop the ExifTool process.
		}
		
		public void Get(string path, ICollection<KeyValuePair<string, string>> properties)
		{
			exifTool.GetProperties(path, properties);
		}
	}
}
