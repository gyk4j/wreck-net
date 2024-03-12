
using System;
using System.Collections.Generic;
using log4net;

namespace Wreck.Logging
{
	/// <summary>
	/// Description of Dumper.
	/// </summary>
	public static class Dumper
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(Dumper));
		
		public static void Dump<K,V>(IDictionary<K, V> values)
		{
			foreach(KeyValuePair<K, V> entry in values)
			{
				LOG.DebugFormat(
					"{0} = {1}",
					entry.Key.ToString(),
					entry.Value.ToString());
			}
		}
		
		public static void Dump<T>(IList<T> values)
		{
			foreach(T v in values)
			{
				LOG.DebugFormat("{0}", v.ToString());
			}
		}
	}
}
