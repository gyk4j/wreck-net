﻿
using System;
using System.Collections.Generic;
using System.IO;

using log4net;
using Wreck.Resources;

namespace Wreck.IO.Reader.User
{
	/// <summary>
	/// Description of CustomDateTimeReader.
	/// </summary>
	public class CustomDateTimeReader : AbstractTimestampReader
	{		
		private static readonly ILog LOG = LogManager.GetLogger(typeof(CustomDateTimeReader));

		protected static readonly string[] USER =
		{
			R.Strings.UserCustomDateTime
		};
		
		private DateTime custom;
		
		public CustomDateTimeReader() : base()
		{
			custom = DateTime.Now;
		}
		
		public override string[] Creation()
		{
			return USER;
		}
		
		public DateTime Custom
		{
			get { return custom; }
			set { custom = value; }
		}
		
		public override void Extract(FileSystemInfo file, List<Metadata> metadata)
		{
			DateTime i = custom;
			string v = (i != null)? i.ToString() : null;
			Add(metadata,
			    USER[0],
			    v,
			    i);
		}
	}
}
