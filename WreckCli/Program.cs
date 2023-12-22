
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Wreck.Corrector;

using Wreck.Logging;
using log4net;
using log4net.Config;

namespace Wreck
{
	class Program
	{
		private const string LOG4NET_XML = "log4net.xml";
		private Logger logger;
		
		private static readonly ILog log = LogManager.GetLogger(typeof(Program));
		
		public static void Main(string[] args)
		{
			//BasicConfigurator.Configure();
			XmlConfigurator.Configure(new System.IO.FileInfo(LOG4NET_XML));
			log.Info("Entering application.");
			
			Program app = new Program();
			
			string[] dirs = (args.Length > 0)? args: new string[]{ Directory.GetCurrentDirectory() };
//			app.Run(args);
			
			log.Info("Exiting application.");			
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