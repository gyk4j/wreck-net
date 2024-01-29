
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
			
			ConsoleView view = new ConsoleView();
			view.Run(args);
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
	}
}