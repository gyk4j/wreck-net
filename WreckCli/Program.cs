
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
			MediaInfoParser mip = new MediaInfoParser();
			//mip.TestMediaInfoLoad();
			string[] parameters = { "Copyright", "Title" };
			Dictionary<string, string> info = mip.ExampleWithStream(
				@"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv",
				parameters
			);
			IDictionaryEnumerator e = info.GetEnumerator();
			while(e.MoveNext())
			{
				Console.WriteLine(e.Key + " : " + e.Value); 
			}
			
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
	}
}