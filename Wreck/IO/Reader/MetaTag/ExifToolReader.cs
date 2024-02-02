
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using ExifToolWrapper;
using Java.Time;
using log4net;
using log4net.Config;

namespace Wreck.IO.Reader.MetaTag
{
	/// <summary>
	/// Description of ExifToolReader.
	/// </summary>
	public class ExifToolReader : AbstractTimestampReader
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(ExifToolReader));

		protected static readonly string[] CREATION =
		{
			"CREATE_DATE",
			"CREATION_DATE",
			"DATE_TIME_ORIGINAL",
			"DateTimeOriginal",
			"CreationDate",
			"CreateDate",
		};

		protected static readonly string[] MODIFIED =
		{
			"UnspecifiedTag{name: \"ModifyDate\"}",
			"ModifyDate",
		};

		private static readonly Regex REGEXP_VALUE = new Regex(@"([\d]{4}):([\d]{2}):([\d]{2}) ([\d]{2}):([\d]{2}):([\d]{2}).*", RegexOptions.Compiled);

		protected const string FORMATTER = "yyyy:MM:dd HH:mm:ss[.SSS]";

		private const string FMT1 = "ddd MMM dd yyyy HH:mm:ss";
		private const string FMT2 = "ddd MMM dd HH:mm:ss yyyy";

		private ExifTool exifTool = null;

		public ExifToolReader() : base()
		{
			if(StopRunningInstances())
				LOG.Info("All running exiftool instances were stopped successfully.");
			else
				LOG.Warn("Some exiftool instance(s) were not cleaned up.");

			try
			{
				exifTool = new ExifTool();
				LOG.Info("ExifTool started");
			}
			catch(ApplicationException)
			{
				LOG.Error("Failed to initialize ExifTool.");
			}
		}

		public override string[] Creation()
		{
			return CREATION;
		}

		public override string[] Modified()
		{
			return MODIFIED;
		}

		public override void Extract(FileSystemInfo file, List<Metadata> metadata)
		{
//			LOG.Debug(file.ToString());

			if(exifTool == null)
				return;

			Dictionary<string, string> output = new Dictionary<string, string>();
			// GetProperties extracts ALL tags found with no filtering/selection.
			exifTool.GetProperties(file.FullName, output);

			List<string> removeables = new List<string>();
			// Filter out unwanted tags.
			foreach(string key in output.Keys)
			{
				bool remove = true;

				foreach(string s in CREATION)
				{
					if(!key.StartsWith("System:") && key.EndsWith(s))
						remove = false;
				}

				foreach(string s in MODIFIED)
				{
					if(!key.StartsWith("System:") && key.EndsWith(s))
						remove = false;
				}

				// To be removed if it is not in the list.
				if(remove)
					removeables.Add(key);
			}
			
			// Now strip out the ignored tags.
			removeables.ForEach(
				s =>
				{
					output.Remove(s);
				}
			);

			if(output.Count > 0)
			{
				IDictionaryEnumerator e = output.GetEnumerator();
				while(e.MoveNext())
				{
					string k = (string) e.Key;
					string v = (string) e.Value;
					try
					{
						Add(metadata, k, v, Parse(v));
					}
					catch(FormatException ex)
					{
						LOG.Error(ex.Message);
					}
				}
			}
			else
				LOG.WarnFormat("No metadata found: {0}", file.Name);
		}

		public new void Dispose()
		{
			try
			{
				if(exifTool != null)
				{
					exifTool.Dispose();
					exifTool = null;
				}
			}
			catch (Exception e)
			{
				LOG.Error(e.ToString());
			}
		}

		private DateTime Parse(string dateTime)
		{
			Instant i = null;
			
			DateTime date;
			if(ExifToolWrapper.ExifTool.TryParseDate(dateTime, DateTimeKind.Utc, out date))
			{
				i = Instant.From(date);
			}
			
			if(i == null && "0".Equals(dateTime))
			{
				throw new FormatException("Unparsable date time: " + dateTime);
			}
			
			if(i == null && dateTime.IndexOf("0000-00-00T00:00:00") >= 0)
				throw new FormatException("Unparsable date time: " + dateTime);
			
			if(i == null && dateTime.IndexOf("0000:00:00 00:00:00") >= 0)
				throw new FormatException("Unparsable date time: " + dateTime);

			if(i == null && dateTime.Length == 4) {
				int yyyy;
				if(int.TryParse(dateTime, out yyyy))
				{
					if(yyyy >= DateTime.Now.Year-100 &&
					   yyyy <= DateTime.Now.Year)
					{
						i = Instant.From(new DateTime(yyyy, 1, 1, 12, 0, 0, DateTimeKind.Utc));
					}
				}
				else
					i = null;
			}

			Match m = REGEXP_VALUE.Match(dateTime);
			if(i == null && m.Success && m.Groups.Count == 7) {
				string month = m.Groups[2].Value, dayOfMonth = m.Groups[3].Value;

				// If US date.
				int mm = int.Parse(month);
				int dd = int.Parse(dayOfMonth);
				if(mm > 12 && mm <= 31 && dd > 0 && dd <= 12) {
					// month looks like a dayOfMonth
					month = m.Groups[3].Value;

					// dayOfMonth looks like a month
					dayOfMonth = m.Groups[2].Value;
				}

				string temp = string.Format(
					"{0}-{1}-{2}T{3}:{4}:{5}Z",
					m.Groups[1].Value,
					month,
					dayOfMonth,
					m.Groups[4].Value,
					m.Groups[5].Value,
					m.Groups[6].Value);

				
				i = Instant.Parse(temp);
			}

			if(i == null)
			{
				LocalDateTime l = LocalDateTime.Parse(dateTime, FMT1);
				
				if(l != null)
				{
					i = Instant.From(l);
					LOG.InfoFormat("{0} -> {1}", dateTime, i.ToString());
				}
				else
					i = null;
			}

			if(i == null)
			{
				LocalDateTime l = LocalDateTime.Parse(dateTime, FMT2);

				if(l != null)
				{
					i = Instant.From(l);
					LOG.InfoFormat("{0} -> {1}", dateTime, i.ToString());
				}
				else
					i = null;
			}

			if(i == null)
			{
				throw new FormatException("Unparsable date time: " + dateTime);
			}

			return i.DateTime;
		}
		
		private const string ExifTool = "exiftool";
		private static bool StopRunningInstances()
		{
			int stopped = 0;
			
			Process[] instances = Process.GetProcessesByName(ExifTool);
			
			LOG.WarnFormat("{0} running exiftool found.", instances.Length);
			
			foreach(Process p in instances)
			{
				try
				{
					if(!p.HasExited)
					{
						p.Kill();
						LOG.InfoFormat("Cleaned up process {0}", p.Id);
						stopped++;
					}
				}
				catch(Exception ex)
				{
					LOG.ErrorFormat("Exception caught during clean-up: {0}", ex.Message);
				}
			}
			
			// Return if all instances were stopped successfully.
			return (stopped == instances.Length);
		}
	}
}
