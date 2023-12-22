
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using Wreck.Corrector;
using Wreck.Logging;
using log4net;
using log4net.Config;

namespace Wreck
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private const string LOG4NET_XML = "log4net.xml";
		
		private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
		private Logger logger;
		private Wreck wreck;
		
		private TreeNode rootNode;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			//BasicConfigurator.Configure();
			XmlConfigurator.Configure(new System.IO.FileInfo(LOG4NET_XML));
			log.Debug("Initializing MainForm");
			
			this.logger = new Logger(this);
			this.wreck = new Wreck(logger, new Previewer());
			logger.Version();
			logger.Statistics(wreck.GetStatistics());
			
			this.rootNode = new TreeNode();
            this.rootNode.Name = "rootNode";
            this.rootNode.Text = "rootNode";
            this.treeViewPaths.Nodes.Add(this.rootNode);
            this.rootNode.ExpandAll();
            
            log.Debug("Initialized MainForm");
		}
		
		public void Run(string[] args)
		{			
			foreach(string p in args)
			{
				logger.CurrentPath(p);
				wreck.Walk(p);
			}
			
			logger.Statistics(wreck.GetStatistics());
		}
		
		void BtnRunClick(object sender, EventArgs e)
		{
			log.Debug("Run clicked");
			
			string[] args = Environment.GetCommandLineArgs();			
			string[] dirs; 
			
			if(args.Length > 1)
			{
				dirs = new string[args.Length-1];
				Array.Copy(args, 1, dirs, 0, args.Length-1);
			}
			else
			{
				dirs = new string[]
				{
					Directory.GetCurrentDirectory()
				};
			}
			
			
			this.Run(dirs);
			
			this.treeViewPaths.ExpandAll();
		}
		
		public StatusStrip getStatusStrip()
		{
			return this.statusStrip;
		}
		
		public TreeView getTreeView()
		{
			return this.treeViewPaths;
		}
		
		public TreeNode getRootNode()
		{
			return this.rootNode;
		}
	}
}
