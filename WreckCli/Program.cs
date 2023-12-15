
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Wreck.Corrector;

using Wreck.Logging;
using Wreck.Parser;

namespace Wreck
{
	class Program
	{
		private Logger logger;
		private Wreck wreck;
		
		public static void Main(string[] args)
		{
			TestExifTool();
			
			/*
			Program wreck = new Program();
			
			string[] dirs = (args.Length > 0)? args: new string[]{ Directory.GetCurrentDirectory() };
			wreck.Run(args);
			*/
			Console.ReadLine();
		}
		
		public Program()
		{
			logger = new Logger();
			wreck = new Wreck(logger, new Applicator());
		}
		
		public void Run(string[] args)
		{
			logger.Version();
			
			foreach(string p in args)
			{
				logger.CurrentPath(p);
				wreck.Walk(p);
			}
			
			logger.Statistics(wreck.GetStatistics());
		}
		
		private static void Dump(Dictionary<string, string> metadata)
		{
			IDictionaryEnumerator e = metadata.GetEnumerator();
			while(e.MoveNext())
			{
				Console.WriteLine(e.Key + " : " + e.Value); 
			}
		}
		
		public static void TestMediaInfo()
		{
			MediaInfoParser mip = new MediaInfoParser();
			//mip.TestMediaInfoLoad();
			string[] parameters = { "Copyright", "Title" };
			Dictionary<string, string> info = mip.ExampleWithStream(
				@"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv",
				parameters
			);
			Dump(info);
		}
		
		public static void TestExifTool()
		{
			ExifToolWrapper.ExifTool et = new ExifToolWrapper.ExifTool();
			
			Dictionary<string, string> properties = new Dictionary<string, string>();
			
			// Repeat as many times as necessary in a loop.
			string[] docs =
			{
				@"C:\test\wd-spectools-word-sample-04.doc",
				@"C:\test\exploring-microsoft-office-word-2010.ppt"
			};
			
			IEnumerator e = docs.GetEnumerator();
			while( e.MoveNext() && (e.Current != null) )
			{
				string path = (string) e.Current;
				
				Console.WriteLine("--- {0} ---", path);
				
				// Extract the metadata.
				et.GetProperties(path, properties);
				
				// Use the extracted metadata.
				Dump(properties);
				
				// Prepare for next document
				properties.Clear();
			}
			
			et.Dispose(); // Always stop the ExifTool process.
		}
	}
}