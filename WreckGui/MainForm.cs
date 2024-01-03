
using System;
using System.ComponentModel;
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
		private TreeNode pathNode;
		private TreeNode dirNode;
		private TreeNode fileNode;
		
		BackgroundWorker backgroundWorker = null;
		
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
			
			backgroundWorker = new BackgroundWorker(); 
			
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
			if(!backgroundWorker.IsBusy)
			{
				backgroundWorker.DoWork += new DoWorkEventHandler(DoWork);
				backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
				backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
				backgroundWorker.WorkerReportsProgress = true;
				backgroundWorker.WorkerSupportsCancellation = false;
				backgroundWorker.RunWorkerAsync();
			}	
		}
		
		void DoWork(object sender, DoWorkEventArgs e)
		{
			log.Debug("DoWork");
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
		
		void ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			//log.DebugFormat("Progress: {0} %", e.ProgressPercentage);
			
			switch(e.UserState.GetType().Name)
			{
				case "String":
					string p = (string) e.UserState;
					log.InfoFormat("P: {0}", p);
					
					pathNode = new TreeNode();
					pathNode.Name = p;
					pathNode.Text = p;
					
					if(rootNode != null)
						rootNode.Nodes.Add(pathNode);
					else
						log.Error("rootNode is null");
					break;
				case "FileInfo":
					FileInfo fi = (FileInfo) e.UserState;
					log.InfoFormat("F: {0}", fi.Name);
					
					fileNode = new TreeNode();
					fileNode.Name = fi.FullName;
					fileNode.Text = fi.Name;
					
					if(dirNode != null)
						dirNode.Nodes.Add(fileNode);
					else
						log.Error("dirNode is null");
					break;
				case "DirectoryInfo":
					DirectoryInfo di = (DirectoryInfo) e.UserState;
					log.InfoFormat("D: {0}", di.FullName);
					
					dirNode = new TreeNode();
					dirNode.
					dirNode.Name = di.FullName;
					dirNode.Text = di.Name;
					
					if(pathNode != null)
						pathNode.Nodes.Add(dirNode);
					else
						log.Error("pathNode is null");
					break;
				default:
					log.WarnFormat("{0}: {1}", e.UserState.GetType().FullName, e.UserState.ToString());
					break;
			}
			
		}
		
		void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			log.Debug("RunWorkerCompleted");
			this.treeViewPaths.ExpandAll();
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
			if(backgroundWorker != null)
				backgroundWorker.ReportProgress(0, p);
		}
		
		public void CurrentFile(FileInfo f)
		{
			if(backgroundWorker != null)
				backgroundWorker.ReportProgress(0, f);
		}
		
		public void CurrentDirectory(DirectoryInfo d)
		{
			if(backgroundWorker != null)
				backgroundWorker.ReportProgress(0, d);
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
			log.WarnFormat("Skipped reparse point: {0}", d.Name);
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
			log.WarnFormat("Skipped reparse point: {0}", f.Name);
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
