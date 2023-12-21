
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
		}
		
		public void Run(string[] args)
		{
			logger.Version();
			
//			Test.TestMediaInfo();
//			Test.TestExifTool();
//			Test.Test7Zip();
			
			using(Wreck wreck = new Wreck(logger, new Previewer()))
			{
				foreach(string p in args)
				{
					logger.CurrentPath(p);
					wreck.Walk(p);
				}
				
				logger.Statistics(wreck.GetStatistics());
			}	
		}
	}
}