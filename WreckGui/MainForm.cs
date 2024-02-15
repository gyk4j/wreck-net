
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using log4net;
using Wreck.Logging;

namespace Wreck
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{		
		private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
		private Logger logger;
		
		private TreeNode rootNode;
		private TreeNode pathNode;
		private TreeNode dirNode;
		private TreeNode fileNode;
		
		// Based on the index assigned by imageList in Design mode.
		private enum TreeViewIcon
		{
			Folder,
			File,
			Start,
			App,
			AppIdle,
			AppRunning
		}
		
		public const string AppStateIdle = "Idle";
		public const string AppStateRunning = "Running";
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			log.Debug("Initializing MainForm");
			
			this.logger = new Logger(this);
			logger.Version();
			
			this.treeViewPaths.ImageList = this.imageList;
			this.rootNode = new TreeNode();
			this.rootNode.Name = "rootNode";
			SetAppState(false);
			this.treeViewPaths.Nodes.Add(this.rootNode);
			
			log.Debug("Initialized MainForm");
		}
		
		void BtnRunClick(object sender, EventArgs e)
		{
			Program.Controller.Analyze();
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
			//log.InfoFormat("P: {0}", p);
			SetCurrentFile(p);
			
			pathNode = new TreeNode();
			pathNode.Name = p;
			pathNode.Text = p;
			
			pathNode.ImageIndex = (int) TreeViewIcon.Start;
			pathNode.SelectedImageIndex = (int) TreeViewIcon.Start;
			
			if(rootNode != null)
			{
				rootNode.Nodes.Add(pathNode);
				
				if(Directory.Exists(p))
					dirNode = pathNode;
				else if(File.Exists(p))
					fileNode = pathNode;
			}
			else
				log.Error("rootNode is null");
		}
		
		public void CurrentFile(FileInfo fi)
		{
			//log.InfoFormat("F: {0}", fi.Name);
			SetCurrentFile(fi.Name);
			
			// If file is not a top-level starting path
			if(!pathNode.Name.Equals(fi.FullName))
			{
				fileNode = new TreeNode();
				fileNode.Name = fi.FullName;
				fileNode.Text = fi.Name;
				fileNode.ImageIndex = (int) TreeViewIcon.File;
				fileNode.SelectedImageIndex = (int) TreeViewIcon.File;
				
				if(dirNode != null)
					dirNode.Nodes.Add(fileNode);
				else
					log.Error("dirNode is null");
			}
		}
		
		public void CurrentDirectory(DirectoryInfo di)
		{
			//log.InfoFormat("D: {0}", di.FullName);
			SetCurrentFile(di.Name);
			
			// If directory is not a top-level starting path
			// i.e. directory is a sub-directory
			if(!pathNode.Name.Equals(di.FullName))
			{
				dirNode = new TreeNode();
				dirNode.Name = di.FullName;
				dirNode.Text = di.FullName.Replace(pathNode.Name, "");
				dirNode.ImageIndex = (int) TreeViewIcon.Folder;
				dirNode.SelectedImageIndex = (int) TreeViewIcon.Folder;
				
				if(pathNode != null)
					pathNode.Nodes.Add(dirNode);
				else
					log.Error("pathNode is null");
			}
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
		
		public void SetCurrentFile(string p)
		{
			ToolStripItem lblCurrentFile = statusStrip.Items["lblCurrentFile"];
			lblCurrentFile.Text = p;
		}
		
		public void SetAppState(bool running)
		{
			string text = running ? AppStateRunning : AppStateIdle;
			int icon = running ?
				(int) TreeViewIcon.AppRunning :
				(int) TreeViewIcon.AppIdle;
			
			Debug.Assert(this.rootNode != null);
			
			this.rootNode.Text = text;
			this.rootNode.ImageIndex = icon;
			this.rootNode.SelectedImageIndex = icon;
			
			btnRun.Enabled = !running;
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
