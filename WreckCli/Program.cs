
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using log4net;
using log4net.Config;
using Wreck.Controller;
using Wreck.Corrector;
using Wreck.Logging;
using Wreck.View;

namespace Wreck
{
	class Program
	{
		private const string LOG4NET_XML = "log4net.xml";
		/*
		private IController controller;
		private Logger logger;
		*/
		
		private static readonly ILog log = LogManager.GetLogger(typeof(Program));
		
		public static void Main(string[] args)
		{
			//BasicConfigurator.Configure();
			XmlConfigurator.Configure(new System.IO.FileInfo(LOG4NET_XML));
			TestParse();
			
//			ConsoleView view = new ConsoleView();
//			view.Run(args);
			/*
			Program app = new Program();
			
			string[] dirs = (args.Length > 0)? args: new string[]{ Directory.GetCurrentDirectory() };
			app.Run(args);
			*/
		}
		
		/*
		public Program()
		{
			controller = new CliController();
			logger = new Logger();
		}
		
		public void Run(string[] args)
		{
			logger.Version();
			
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
		*/
		
		private static void TestParse()
		{
			const string FMT1 = "ddd MMM dd yyyy HH:mm:ss";
			const string FMT2 = "ddd MMM dd HH:mm:ss yyyy";
			
			string test1 = "Thu Feb 01 2024 09:10:11";
			string test2 = "Thu Feb 01 09:10:11 2024";
			
			Java.Time.LocalDateTime d1t1 = Java.Time.LocalDateTime.Parse(test1, FMT1);
			Java.Time.LocalDateTime d1t2 = Java.Time.LocalDateTime.Parse(test1, FMT2);
			
			log.DebugFormat("FMT1: {0}", d1t1 != null? d1t1.ToString() : "-");
			log.DebugFormat("FMT2: {0}", d1t2 != null? d1t2.ToString() : "-");
			
			Java.Time.LocalDateTime d2t1 = Java.Time.LocalDateTime.Parse(test2, FMT1);
			Java.Time.LocalDateTime d2t2 = Java.Time.LocalDateTime.Parse(test2, FMT2);

			log.DebugFormat("FMT1: {0}", d2t1 != null ? d2t1.ToString() : "-");
			log.DebugFormat("FMT2: {0}", d2t2 != null ? d2t2.ToString() : "-");
		}
	}
}