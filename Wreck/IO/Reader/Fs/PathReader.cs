
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

using log4net;
using Wreck.Resources;

namespace Wreck.IO.Reader.Fs
{
	/// <summary>
	/// Description of PathReader.
	/// </summary>
	public class PathReader : AbstractTimestampReader
	{
		private static readonly ILog LOGGER = LogManager.GetLogger(typeof(PathReader));
		
		public static readonly string[] CREATION =
		{
			R.strings.PATH_NAME
		};
		
		public const string DATE_PART_SEPARATOR = "[\\\\\\-_\\.:/]?";
		public const string YEAR_FULL_RANGE = "(?<year>19[8,9][0-9]|20([0-1][0-9]|2[0-3]))";
		public const string YEAR_SHORT_RANGE = "(?<year>[8,9][0-9]|[0-1][0-9]|2[0-3])";
		public const string MONTH_RANGE = "(?<month>0[1-9]|1[0-2])";
		public const string DAY_RANGE = "(?<day>0[1-9]|[1,2][0-9]|3[0,1])";
		
		public const string TIME_RANGE = "([0,1][0-9]|2[0-3])[0-5][0-9][0-5][0-9]";
		
		private const String WORD_BOUNDARY = "";
		public const string FILENAME_DATE_LONG =
			WORD_BOUNDARY + YEAR_FULL_RANGE + MONTH_RANGE + DAY_RANGE + WORD_BOUNDARY;
		public const string FILENAME_DATE_SHORT =
			WORD_BOUNDARY + YEAR_SHORT_RANGE + MONTH_RANGE + DAY_RANGE + WORD_BOUNDARY;
		public const string FILENAME_DATE_PATH =
			DATE_PART_SEPARATOR + YEAR_FULL_RANGE + DATE_PART_SEPARATOR + MONTH_RANGE + DATE_PART_SEPARATOR + DAY_RANGE + WORD_BOUNDARY;
		
		public const string FILENAME_TIME = WORD_BOUNDARY + TIME_RANGE + WORD_BOUNDARY;
		public const string FILENAME_DATETIME = WORD_BOUNDARY + YEAR_FULL_RANGE + MONTH_RANGE + DAY_RANGE + TIME_RANGE + WORD_BOUNDARY;
		
		public static readonly Regex FILENAME_PATTERN_DATE_LONG =
			new Regex(FILENAME_DATE_LONG, RegexOptions.Compiled);
		public static readonly Regex FILENAME_PATTERN_DATE_SHORT =
			new Regex(FILENAME_DATE_SHORT, RegexOptions.Compiled);
		public static readonly Regex FILENAME_PATTERN_DATE_PATH =
			new Regex(FILENAME_DATE_PATH, RegexOptions.Compiled);
		public static readonly Regex FILENAME_PATTERN_TIME =
			new Regex(FILENAME_TIME, RegexOptions.Compiled);
		
		public const string DATE_FORMATTER = "yyyyMMdd";
		public const string TIME_FORMATTER = "HHmmss";
		
		public PathReader() : base()
		{
		}
		
		public new string[] creation()
		{
			return CREATION;
		}
		
		public new void extract(FileSystemInfo file, List<Metadata> metadata)
		{
			string date = null;
			string time = null;
			DateTime? localDate = null;
			DateTime? localTime = null;
			string filename = file.Name;
			Match matcher;

			matcher = FILENAME_PATTERN_DATE_LONG.Match(filename);
			if (matcher.Success)
			{
				date = matcher.Value;
			}

			if (date == null)
			{
				matcher = FILENAME_PATTERN_DATE_SHORT.Match(filename);
				if (matcher.Success)
				{
					date = matcher.Value;
					string prefix = (date.ToCharArray()[0] == '8' || date.ToCharArray()[0] == '9') ? "19" : "20";
					date = prefix + date;
				}
			}

			if (date == null)
			{
				matcher = FILENAME_PATTERN_DATE_PATH.Match(file.FullName);
				if (matcher.Success)
				{
					string year = matcher.Groups["year"].Value;
					string month = matcher.Groups["month"].Value;
					string day = matcher.Groups["day"].Value;
					LOGGER.DebugFormat("y = {0}, m = {1}, d = {2}", year, month, day);
					date = string.Format("{0:D4}{1:D2}{2:D2}",
					                     int.Parse(year), int.Parse(month), int.Parse(day));
				}
			}

			// Found a date. Let's try to find the time if available.
			if (date != null)
			{
				try
				{
					localDate = DateTime.ParseExact(date, DATE_FORMATTER, CultureInfo.InvariantCulture);
				} 
				catch (FormatException e)
				{
					LOGGER.Error(e.Message + ":" + date);
				}

				int end = matcher.Index + matcher.Length; // Start searching after the date.
				matcher = FILENAME_PATTERN_TIME.Match(filename.Substring(end));
				if (matcher.Success)
				{
					time = matcher.Value;
					try
					{
						localTime = DateTime.ParseExact(time, TIME_FORMATTER, CultureInfo.InvariantCulture);
					}
					catch (FormatException e)
					{
						LOGGER.Error(e.Message + ":" + time);
					}
				}
			}

			if (localDate != null && localDate.HasValue)
			{
				DateTime instant = (localTime == null || !localTime.HasValue) ?
					new DateTime(localDate.Value.Year, localDate.Value.Month, localDate.Value.Day,
					             12, 0, 0):
					new DateTime(localDate.Value.Year, localDate.Value.Month, localDate.Value.Day,
					             localTime.Value.Hour, localTime.Value.Minute, localTime.Value.Second, localTime.Value.Millisecond);

				Add(metadata, CREATION[0], instant.ToString(), instant);
			}
		}
	}
}
