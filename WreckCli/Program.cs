
using System;
using System.IO;
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
			Program wreck = new Program();
			
			string[] dirs = (args.Length > 0)? args: new string[]{ Directory.GetCurrentDirectory() };
			wreck.Run(args);
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