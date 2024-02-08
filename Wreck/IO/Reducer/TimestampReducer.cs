
using System;
using System.Collections.Generic;
using log4net;
using Wreck.Resources;
using Wreck.Time;

namespace Wreck.IO.Reducer
{
	/// <summary>
	/// Description of TimestampReducer.
	/// </summary>
	public class TimestampReducer : ITimestampReducer
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(TimestampReducer));
		
		private DateTime? Get(DateTime? t, List<Metadata> s, bool isEarliest)
		{
			if(t == null || !t.HasValue)
			{
				if(s.Count > 0)
				{
					// Make a copy for sorting to preserve original List s.
					List<Metadata> copy = new List<Metadata>(s);
					copy.Sort();
					t = isEarliest ? copy[0].Time : copy[copy.Count-1].Time;
				}
			}
			return t;
		}
		
		private DateTime? Get(List<Metadata> metadata, CorrectionEnum c, bool isEarliest)
		{
			DateTime? t = null;
			List<Metadata> s;
			
			if(metadata.Count == 0)
				return null;
			
			Metadata custom = metadata
				.Find( md =>
				      R.strings.USER_CUSTOM_DATE_TIME.Equals(md.Key));
			
//			metadata.ForEach(md => LOG.DebugFormat("{0} = {1} {2}", md.Key, md.Value, md.Time));
			
			List<Metadata> embedded = metadata
				.FindAll(md =>
				         !R.strings.USER_CUSTOM_DATE_TIME.Equals(md.Key) &&
				         !R.strings.FS_CREATION.Equals(md.Key) &&
				         !R.strings.FS_MODIFIED.Equals(md.Key) &&
				         !R.strings.BACKUP_CREATION_TIME.Equals(md.Key) &&
				         !R.strings.BACKUP_LAST_MODIFIED_TIME.Equals(md.Key) &&
				         !R.strings.BACKUP_LAST_ACCESSED_TIME.Equals(md.Key));
			
			List<Metadata> fileSystem = metadata
				.FindAll(md =>
				         R.strings.FS_CREATION.Equals(md.Key) ||
				         R.strings.FS_MODIFIED.Equals(md.Key));
			
			// High precision by attribute group (creation, modified, accessed).
			s = embedded.FindAll(md => c.Equals(md.Group));
			t = Get(t, s, isEarliest);
			
			// Any embedded metadata
			s = embedded;
			t = Get(t, s, isEarliest);

			// Use backup options between file system or custom user-defined time.
			s = fileSystem;
			t = Get(t, s, isEarliest);
			
			// Use any metadata found.
			s = metadata;
			t = Get(t, s, isEarliest);
			
			if(t != null && t.HasValue)
			{
				// If custom limit is set, and time found is later than the limit, use the
				// user set limit instead.
				if(custom != null && custom.Time.CompareTo(t) < 0)
					t = custom.Time;
				
				// Last check: Between 1980-01-01 and current date time.
				if(t.Value.CompareTo(TimeUtils.VALID_PERIOD_MIN) < 0)
					t = fileSystem.Find(
						md =>
						{
							return R.strings.FS_CREATION.Equals(md.Key);
						})
						.Time;
				// TimeUtils.VALID_PERIOD_MIN;
				else if(t.Value.CompareTo(TimeUtils.VALID_PERIOD_MAX) > 0)
					t = fileSystem.Find(
						md =>
						{
							return R.strings.FS_MODIFIED.Equals(md.Key);
						})
						.Time;
				// TimeUtils.VALID_PERIOD_MAX;
			}
			
			embedded.Clear();
			fileSystem.Clear();
			
			return t;
		}
		
		public void Reduce(List<Metadata> metadata, Dictionary<CorrectionEnum, DateTime> corrections)
		{
			corrections.Clear();
			// Metadata
			foreach(CorrectionEnum c in CorrectionEnum.Values)
			{
				DateTime? t = null;
				if(c.Equals(CorrectionEnum.CREATION))
					t = Get(metadata, c, true);
				else if(c.Equals(CorrectionEnum.MODIFIED))
					t = Get(metadata, c, false);
				else if(c.Equals(CorrectionEnum.ACCESSED))
					t = Get(metadata, c, false);
				else
					LOG.ErrorFormat("Unknown enum correction: {0}", c.ToString());
				
				corrections.Add(c, t.Value);
			}
		}
	}
}
