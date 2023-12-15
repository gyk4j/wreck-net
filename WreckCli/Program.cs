
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Wreck.Corrector;

using Wreck.Logging;

namespace Wreck
{
	class Program
	{
		private Logger logger;
		private Wreck wreck;
		
		public static void Main(string[] args)
		{			
			Program app = new Program();
			
			string[] dirs = (args.Length > 0)? args: new string[]{ Directory.GetCurrentDirectory() };
			app.Run(args);
			
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
			
			Test.TestMediaInfo();
			Test.TestExifTool();
			Test.Test7Zip();
			
			/*
			foreach(string p in args)
			{
				logger.CurrentPath(p);
				wreck.Walk(p);
			}
			*/
			
			logger.Statistics(wreck.GetStatistics());
		}
	}
}