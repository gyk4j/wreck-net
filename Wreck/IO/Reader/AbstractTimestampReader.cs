
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Wreck.Resources;

namespace Wreck.IO.Reader
{
	/// <summary>
	/// Description of AbstractTimestampReader.
	/// </summary>
	public abstract class AbstractTimestampReader : ITimestampReader
	{
		private static readonly string[] NONE = new string[0];
		
		private readonly Dictionary<string, CorrectionEnum> keys;
		
		public AbstractTimestampReader()
		{
			keys = new Dictionary<string, CorrectionEnum>();
			
			foreach(string s in creation())
			{
				keys.Add(s, CorrectionEnum.CREATION);
			}
			
			foreach(string s in modified())
			{
				keys.Add(s, CorrectionEnum.MODIFIED);
			}
			
			foreach(string s in accessed())
			{
				keys.Add(s, CorrectionEnum.ACCESSED);
			}
		}
		
		public void extract(FileSystemInfo file, List<Metadata> metadata)
		{
			throw new NotImplementedException();
		}
		
		public string[] creation()
		{
			return NONE;
		}
		
		public string[] modified()
		{
			return NONE;
		}
		
		public string[] accessed()
		{
			return NONE;
		}
		
		protected Dictionary<string, CorrectionEnum> GetKeys()
		{
			return keys;
		}
		
		/// <summary>
		/// Add a <c>System.DateTime</c> value.
		/// </summary>
		/// <param name="metadata"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="time"></param>
		protected void Add(
			List<Metadata> metadata,
			string key,
			string val,
			DateTime time)
		{
			CorrectionEnum ce = null;
			bool exists = GetKeys().TryGetValue(key, out ce);
			
			Metadata m = new Metadata(
				key,
				val,
				time,
				ce);
			metadata.Add(m);
		}
		
		/// <summary>
		/// Add a File Time represented as a <c>long</c> value.
		/// </summary>
		/// <param name="metadata"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="time"></param>
		protected void Add(
			List<Metadata> metadata,
			string key,
			string val,
			long time)
		{
			DateTime ft = DateTime.FromFileTimeUtc(time);
			Add(metadata, key, val, ft);
		}
		
		/// <summary>
		/// Add a ISO-8601 formatted string date time.
		/// </summary>
		/// <param name="metadata"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="time"></param>
		protected void Add(
			List<Metadata> metadata,
			string key,
			string val,
			string time)
		{
			DateTime it = DateTime.Parse(time, null, DateTimeStyles.RoundtripKind);
			Add(metadata, key, val, it);
		}
		
		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
