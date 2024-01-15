﻿
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using log4net;
using MediaInfoLib;

namespace Wreck.IO.Reader.MetaTag
{
	/// <summary>
	/// Description of MediaInfoReader.
	/// </summary>
	public class MediaInfoReader : AbstractTimestampReader
	{
		private const string SECTION_GENERAL = "General";

		private static readonly ILog LOG = LogManager.GetLogger(typeof(MediaInfoReader));

		protected static readonly string[] CREATION =
		{
			"Encoded date",
			"Recorded date",
			"creation_time",
			"Encoded_Date",
			"Tagged_Date",
			"Recorded_Date",
		};

		private static readonly Regex REGEXP_VALUE = new Regex(
			@"(UTC )?([\d]{4})-([\d]{2})-([\d]{2}) ([\d]{2}):([\d]{2}):([\d]{2})",
			RegexOptions.Compiled);

		protected const string FORMATTER = "[z ]yyyy-MM-dd HH:mm:ss[.SSS]";

		public MediaInfoReader() : base()
		{
		}

		public override string[] Creation()
		{
			return CREATION;
		}

		public override void Extract(FileSystemInfo file, List<Metadata> metadata)
		{
			MediaInfo mi = new MediaInfo();

			String ifLoaded = mi.Option(string.Format("Info_Version", "0.7.0.0;{0};0.7.0.0", Wreck.NAME));
			if (ifLoaded.Length == 0 ||
			    "Unable to load MediaInfo library".Equals(ifLoaded) ||
			    !ifLoaded.StartsWith("MediaInfoLib - v"))
			{
				LOG.Error("Failed to load MediaInfo.Dll or this version of the DLL is not compatible");
				throw new ApplicationException("Failed to load MediaInfo");
			}

			mi.Open(file.FullName);
			string encodedDate = string.Empty;

			// Loop through the metadata tags we are aiming for.
			foreach(string k in CREATION)
			{
				string v = mi.Get(StreamKind.General, 0, k).Trim();

				if(v != null && !v.Equals(string.Empty))
				{
					try
					{
						Add(metadata, k, v, Parse(v));
					}
					catch (FormatException e)
					{
						LOG.Error(e.ToString());
					}
				}
			}

			mi.Close();
		}

		private DateTime Parse(string dateTime)
		{
			DateTime? i = null;

			Match m = REGEXP_VALUE.Match(dateTime);
			if(m.Success)
			{
				StringBuilder temp = new StringBuilder(dateTime.Trim());

				if(dateTime.StartsWith("UTC "))
				{
					temp.Remove(0, "UTC ".Length);
				}

				if(!dateTime.EndsWith("Z"))
				{
					temp.Append("Z");
				}

				if(temp[10] == ' ')
				{
					temp[10] = 'T';
				}

				if(!"0000-00-00T00:00:00Z".Equals(temp.ToString()))
					i = DateTime.Parse(temp.ToString(), null, DateTimeStyles.RoundtripKind);
			}
			else if(dateTime.Length == 4)
			{
				int yyyy;
				bool isParsed = int.TryParse(dateTime, out yyyy);

				if(isParsed)
				{
					if(yyyy >= DateTime.Now.Year-100 &&
					   yyyy <= DateTime.Now.Year)
					{
						i = new DateTime(yyyy, 1, 1, 12, 0, 0, DateTimeKind.Utc);
					}
				}
				else
					i = null;
			}

			if(i == null || !i.HasValue)
			{
				throw new FormatException("Unparseable date time: " + dateTime);
			}

			return i.Value;
		}
	}
}