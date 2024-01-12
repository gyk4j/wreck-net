
using System;
using System.Collections.Generic;
using Wreck.Resources;

namespace Wreck.IO.Reducer
{
	/// <summary>
	/// Description of ITimestampReducer.
	/// </summary>
	public interface ITimestampReducer
	{
		void Reduce(
			List<Metadata> metadata, 
			Dictionary<CorrectionEnum, DateTime> corrections);
	}
}
