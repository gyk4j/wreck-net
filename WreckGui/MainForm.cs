
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using log4net;
using log4net.Config;
using Wreck.Corrector;
using Wreck.Logging;

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
		/*
		private TreeNode pathNode;
		private TreeNode dirNode;
		private TreeNode fileNode;
		*/
		
		private Thread workerThread = null;
		
		// Declare a delegate used to communicate with the UI thread
		private delegate void UpdateStatusDelegate();
		private UpdateStatusDelegate updateStatusDelegate = null;
		
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
			
			// Initialise the delegate
			this.updateStatusDelegate = new UpdateStatusDelegate(this.UpdateStatus);
			
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
			
			// Initialise and start worker thread
			this.workerThread = new Thread(new ThreadStart(this.BackgroundWorker));
			this.workerThread.Start();
			
			this.treeViewPaths.ExpandAll();
		}
		
		private void BackgroundWorker()
		{
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
		}
		
		private void UpdateStatus()
		{
			log.Debug("*");
		}
		
		public void Version()
		{
			this.Text = String.Format("{0} v{1}", Wreck.NAME, Wreck.VERSION);
		}
		
		public void UnknownPathType(string path)
		{
			MessageBox.Show(
				string.Format("UnknownPathType: {0}", path),
				"Unknown Path Type",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error
			);
		}
		
		public void CurrentPath(string p)
		{
			log.DebugFormat("{0}", p);
			/*
			pathNode = new TreeNode();
			pathNode.Name = p;
			pathNode.Text = p;
			
			rootNode.Nodes.Add(pathNode);
			*/
		}
		
		public void CurrentFile(FileInfo f)
		{
			log.DebugFormat("    - {0}", f.Name);
			/*
			fileNode = new TreeNode();
			fileNode.Name = f.FullName;
			fileNode.Text = f.Name;
			
			dirNode.Nodes.Add(fileNode);
			*/
		}
		
		public void CurrentDirectory(DirectoryInfo d)
		{
			log.DebugFormat("  - {0}", d.FullName);
			/*
			dirNode = new TreeNode();
			dirNode.Name = d.FullName;
			dirNode.Text = d.FullName;
			
			pathNode.Nodes.Add(dirNode);
			*/
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
			log.DebugFormat("Skipped reparse point: {0}", d.Name);
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
			log.DebugFormat("Skipped reparse point: {0}", f.Name);
		}
		
		// For Corrector
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
			log.DebugFormat("MW: {0} : {1} -> {2}", fsi.Name, fsi.LastWriteTime, lastWrite);
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
			log.DebugFormat("MC: {0} : {1} -> {2}", fsi.Name, fsi.CreationTime, creation);
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
			log.DebugFormat("MA: {0} : {1} -> {2}", fsi.Name, fsi.LastAccessTime, lastAccess);
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi, DateTime creationOrLastAccess)
		{
			if(creationOrLastAccess == fsi.CreationTime)
				log.DebugFormat("LC: {0} : {1} -> {2}", fsi.Name, fsi.CreationTime, fsi.LastWriteTime);
			else if(creationOrLastAccess == fsi.LastAccessTime)
				log.DebugFormat("LA: {0} : {1} -> {2}", fsi.Name, fsi.LastAccessTime, fsi.LastWriteTime);
		}
		
		public void Statistics(Statistics stats)
		{
			ToolStripItem lblDirectoriesCount = statusStrip.Items["lblDirectoriesCount"];
			lblDirectoriesCount.Text = string.Format("Directories: {0}", stats.Directories);
			
			ToolStripItem lblFilesCount = statusStrip.Items["lblFilesCount"];
			lblFilesCount.Text = string.Format("Files: {0}", stats.Files);
			
			ToolStripItem lblSkippedCount = statusStrip.Items["lblSkippedCount"];
			lblSkippedCount.Text = string.Format("Skipped: {0}", stats.Skipped);
		}
		
		// For error reporting
		public void UnauthorizedAccessException(UnauthorizedAccessException ex)
		{
			MessageBox.Show(
				ex.ToString(),
				"Unauthorized Access",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error
			);
		}
	}
}
