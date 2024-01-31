
using System;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Permissions;

using Java.Time;
using log4net;
using log4net.Config;
using Wreck.IO;

namespace Wreck.Time
{
	/// <summary>
	/// Description of TimestampFormatter.
	/// </summary>
	public class TimestampFormatter
	{
		private static readonly ILog LOGGER = LogManager.GetLogger(typeof(TimestampFormatter));

		public const string WORD_BOUNDARY = "\\b";

		private const string EXIF_FORMATTER = "yyyy:MM:dd HH:mm:ss";
		private const string LOCAL_DATE_TIME_FORMATTER = "yyyy-MM-dd HH:mm:ss";

		public static FileSystemInfo IsValidPath(string path)
		{
			FileSystemInfo p = null;

			if(Directory.Exists(path))
			{
				p = new DirectoryInfo(path);
			}
			else if(File.Exists(path))
			{
				p = new FileInfo(path);
			}

			if(p == null)
			{
				throw new FileNotFoundException(p + " does not exist.");
			}

			// Disabling the check.
			/*
			if(!Directory.Exists(p)
			{
				throw new DirectoryNotFoundException(p + " is not a directory.");
			}
			 */

			FileIOPermission readPermission = new FileIOPermission(FileIOPermissionAccess.Read, path);
			try
			{
				readPermission.Demand();
			}
			catch(SecurityException s)
			{
				LOGGER.Error(s.Message);
				throw new UnauthorizedAccessException(p + " is not readable.");
			}

			FileIOPermission writePermission = new FileIOPermission(FileIOPermissionAccess.Write, path);
			try
			{
				writePermission.Demand();
			}
			catch(SecurityException s)
			{
				LOGGER.Error(s.Message);
				throw new UnauthorizedAccessException(p + " is not writable.");
			}

			return p;
		}

		public static DateTime? IsISO8601(string date)
		{
			if(date.Trim().Length != 10 && 		// 2022-12-23
			   date.Trim().Length < 19)			// 2022-12-23T02:37:59.999999Z
			{
				return null;					// 2022-12-23T02:37:59
			}

			// check date
			if(date.ToCharArray()[4] != '-' ||
			   date.ToCharArray()[7] != '-')
			{
				return null;
			}

			// check time.
			// Date and time must be separated by T.
			// Time must be separated by colons for hours:minutes:seconds
			// May or may not end in UTC Z.
			if (date.Length > 10)
			{
				if (date.ToCharArray()[10] != 'T' ||
				    date.ToCharArray()[13] != ':' ||
				    date.ToCharArray()[16] != ':')
				{
					return null;
				}
			}

			// Date only without time, time zone, and time offset
			if(date.Length == 10)
			{
				// https://stackoverflow.com/questions/3556144/how-to-create-a-net-datetime-from-iso-8601-format
				DateTime dt = Instant.Parse(date);
				DateTime d = dt.Date;
				return new DateTime(d.Year, d.Month, d.Day);
			}
			// Date with local time without UTC Z
			else if(date.Length >= 19 && !date.EndsWith("Z"))
			{
				DateTime dt = Instant.Parse(date);
				return dt;
			}
			else
			{
				return Instant.Parse(date);
			}
		}

		public static DateTime? IsLocalDateTime(String date)
		{
			DateTime? i = null;

			try
			{
				DateTime l = DateTime.ParseExact(date, LOCAL_DATE_TIME_FORMATTER, CultureInfo.InvariantCulture);
				DateTime utc = l.ToUniversalTime();
				i = utc;
			} catch(FormatException e) {
				LOGGER.Error(e.Message);
				i = null;
			}
			return i;
		}
		
		public static DateTime? HasExif(String date)
		{
			DateTime? instant = null;
			try
			{
				DateTime local= DateTime.ParseExact(date, EXIF_FORMATTER, CultureInfo.InvariantCulture);
				instant = local.ToUniversalTime();
//				Console.WriteLine("Exif: " + instant);
			}
			catch(FormatException) {
//				Console.Error.WriteLine("Not exif date: " + date);
				instant = null;
			}
			return instant;
		}
		
		// For Apache Tika's Metadata. Unused in .NET implementation.
		/*
		public static DateTime? GetMetaDataInstant(Metadata metadata, string key)
		{
			DateTime? extracted = null;
			string value = metadata.get(key);
			if(value != null)
			{
				extracted = TimestampFormatter.IsISO8601(value);
				if(extracted == null)
				{
					LOGGER.WarnFormat("{0} does not resemble a ISO8601 string.", value);
				}
			}
			else
			{
				foreach (string name in metadata.names())
				{
					extracted = TimestampFormatter.IsISO8601(metadata.get(name));
					if(extracted != null)
					{
						LOGGER.InfoFormat("Found {0} = {1}", name, extracted);
					}
				}
			}
			return extracted;
		}
		 */
		
		private const string FORMATTER_EXIFTOOL_LOCAL = "EEE MMM dd HH:mm:ss yyyy";
		private const string FORMATTER_EXIFTOOL_ENUS = "yyyy[:dd:MM HH:mm:ss[XXX][X]]";
		private const string FORMATTER_EXIFTOOL_ENSG = "yyyy[:MM:dd HH:mm:ss[XXX][X]]";
		public static DateTime? GetExifToolInstant(string v) {
			DateTime? instant = new DateTime?();
			
			if(v.IndexOf("0000:00:00 00:00:00") > -1 || v.IndexOf("1970-01-01 00:00:00") > -1)
				return instant;
			
			int n;
			bool isNumberic = int.TryParse(v, out n);
			
			if(v.Length == 4 && isNumberic)
				instant = new DateTime(n, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			
			if(!instant.HasValue)
			{
				try
				{
					instant = DateTime
						.ParseExact(
							v,
							FORMATTER_EXIFTOOL_LOCAL,
							CultureInfo.InvariantCulture)
						.ToUniversalTime();
				}
				catch(FormatException e)
				{
					LOGGER.Error(e);
				}
			}
			
			// No offset time or zone offset
			if(!instant.HasValue && v.StartsWith("UTC"))
			{
				string stripped = v.Replace("UTC ", "");
				
				char[] sb = stripped.ToCharArray();
				if(sb[10] == ' ')
					sb[10] = 'T';
				
				string mediaInfoValue = new String(sb);
				
				try
				{
					DateTime ldt = Instant.Parse(mediaInfoValue);
					instant = ldt.ToUniversalTime();
				}
				catch(FormatException e)
				{
					LOGGER.Warn(e.ToString());
				}
			}
			
			
			if(!instant.HasValue && !v.EndsWith("Z") && v.IndexOf('+') == -1 && v.IndexOf('-') == -1) {
				try
				{
					instant = DateTime.Parse(v).ToUniversalTime();
				}
				catch(FormatException e)
				{
					LOGGER.Warn(e.ToString());
				}
			}
			
			// if yyyy-mm-dd
			if(!instant.HasValue) {
				try
				{
					instant = DateTime
						.ParseExact(
							v, 
							FORMATTER_EXIFTOOL_ENSG, 
							CultureInfo.InvariantCulture)
						.ToUniversalTime();
				}
				catch(FormatException e)
				{
					LOGGER.Warn(e.ToString());
				}
			}
			
			// If yyyy-dd-mm
			if(!instant.HasValue) {
				try
				{
					instant = DateTime
						.ParseExact(
							v, 
							FORMATTER_EXIFTOOL_ENUS, 
							CultureInfo.InvariantCulture)
						.ToUniversalTime();
				}
				catch(FormatException e)
				{
					LOGGER.Warn(e.ToString());
				}
			}
			
			if(!instant.HasValue)
				LOGGER.WarnFormat("Failed to parse: {0}", v);
			
			return instant;
		}
	}
}
