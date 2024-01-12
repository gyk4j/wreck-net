
using System;

namespace Wreck.IO.Writer
{
	/// <summary>
	/// Description of WriterFactory.
	/// </summary>
	public class WriterFactory : IDisposable
	{
		private static WriterFactory INSTANCE = null;
		
//		private readonly AnalyzeWriter analyzeWriter;
//		private readonly BasicFileAttributesWriter basicFileAttributesWriter;
//		private readonly CsvLogWriter csvLogWriter;
//		private readonly RestoreBasicFileAttributesWriter restoreBasicFileAttributesWriter;
		
		public static bool IsInitialized()
		{
			return INSTANCE != null;
		}
		
		private WriterFactory()
		{
//			analyzeWriter = new AnalyzeWriter();
//			basicFileAttributesWriter = new BasicFileAttributesWriter();
//			csvLogWriter = new CsvLogWriter();
//			restoreBasicFileAttributesWriter = new RestoreBasicFileAttributesWriter();
		}
		
		public static WriterFactory Instance
		{
			get
			{
				if(INSTANCE == null) {
					INSTANCE = new WriterFactory();
				}
				return INSTANCE;
			}
		}
		
		public void Dispose()
		{
//			csvLogWriter.Dispose();
			
			INSTANCE = null;
		}
		
//		public AnalyzeWriter AnalyzeWriter
//		{
//			get { return analyzeWriter; }
//		}
//		
//		public BasicFileAttributesWriter BasicFileAttributesWriter
//		{
//			get { return basicFileAttributesWriter; }
//		}
//
//		public CsvLogWriter CsvLogWriter
//		{
//			get { return csvLogWriter; }
//		}
//		
//		public RestoreBasicFileAttributesWriter RestoreBasicFileAttributesWriter
//		{
//			get { return restoreBasicFileAttributesWriter; }
//		}
	}
}
