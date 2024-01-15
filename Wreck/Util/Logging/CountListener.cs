
using System;
using System.Collections.Generic;

namespace Wreck.Util.Logging
{
	/// <summary>
	/// Description of CountListener.
	/// </summary>
	public class CountListener<T> : StatisticsListener<T, int>
	{
		private readonly IDictionary<T, int> statistics = new Dictionary<T, int>();
		
		public IDictionary<T, int> Statistics
		{
		
			get { return statistics; }
		}
		
		public IDictionary<T, int> Get()
		{
			return Statistics;
		}

		public int Get(T t)
		{
			int val;
			bool ok = Statistics.TryGetValue(t, out val);
			return ok ? val : 0;
		}

		public int Count(T t)
		{
			int v = Get(t);
			statistics[t] = ++v;
			return v;
		}

		public void Clear()
		{
			Statistics.Clear();
		}
	}
}
