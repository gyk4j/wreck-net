
using System;
using System.Collections.Generic;

namespace Wreck.Util.Logging
{
	/// <summary>
	/// Description of StatisticsListener.
	/// </summary>
	public interface StatisticsListener<T, U>
	{
		IDictionary<T, U> Get();
		U Get(T t);
		U Count(T t);
		void Clear();
	}
}
