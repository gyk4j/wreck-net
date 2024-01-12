
using System;
using System.Collections.Generic;
using log4net;
using Wreck.IO.Reader;
using Wreck.IO.Reducer;
using Wreck.IO.Writer;
using Wreck.Resources;

namespace Wreck.IO
{
	/// <summary>
	/// Description of MetadataBuilder.
	/// </summary>
	public class MetadataBuilder
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(MetadataBuilder));

		private readonly List<ITimestampReader> readers;
		private readonly List<ITimestampWriter> writers;
		private readonly ITimestampReducer reducer;
		private readonly List<Metadata> metadata;
		private readonly Dictionary<CorrectionEnum, DateTime> suggestions;
		
		public MetadataBuilder()
		{
			readers = new List<ITimestampReader>();
			writers = new List<ITimestampWriter>();
			metadata = new List<Metadata>();
			reducer = new TimestampReducer();
			suggestions = new Dictionary<CorrectionEnum, DateTime>();
		}
	}
}
