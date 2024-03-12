
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using log4net;
using Wreck.Entity;
using Wreck.Resources;

namespace Wreck
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
		
		private TreeNode rootNode;
		private IDictionary<string, TreeNode> pathNodes;
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
		
		// This BindingSource binds the list to the DataGridView control.
		private IDictionary<Control, BindingSource> bindings;
		
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
			
			this.treeViewPaths.ImageList = this.imageList;
			this.rootNode = new TreeNode();
			this.rootNode.Name = "rootNode";
			
			this.treeViewPaths.Nodes.Add(this.rootNode);
			
			this.pathNodes = new Dictionary<string, TreeNode>();
			
			bindings = new Dictionary<Control, BindingSource>();
			
			log.Debug("Initialized MainForm");
		}
		
		void MainForm_Load(object sender, EventArgs e)
		{
			SetAppState(false);
			this.treeViewPaths.ExpandAll();
		}
		
		public void Bind(object source, SourceEnum type, string member)
		{
			Control control;
			
			if(type.Name.Equals(SourceEnum.METADATA.Name))
			{
				control = cbxMetadataTags;
			}
			else if(type.Name.Equals(SourceEnum.FILE_SYSTEM.Name))
			{
				control = cbxFileSystemAttributes;
				
			}
			else if(type.Name.Equals(SourceEnum.CUSTOM.Name))
			{
				control = cbxCustom;
			}
			else
			{
				log.ErrorFormat("Unsupported member: {0}", Enum.GetName(typeof(SourceEnum), type));
				throw new ArgumentException(type.Name);
			}
			
			Binding binding = new Binding(
				"Checked",
				source,
				member,
				false,
				DataSourceUpdateMode.OnPropertyChanged);
			
			control.DataBindings.Add(binding);
		}
		
		public void Bind(object source, CorrectionEnum type, string member)
		{
			Control control;
			
			if(type.Name.Equals(CorrectionEnum.CREATION.Name))
			{
				control = cbxCreation;
			}
			else if(type.Name.Equals(CorrectionEnum.MODIFIED.Name))
			{
				control = cbxLastModified;
			}
			else if(type.Name.Equals(CorrectionEnum.ACCESSED.Name))
			{
				control = cbxLastAccessed;
			}
			else
			{
				log.ErrorFormat("Unsupported member: {0}", Enum.GetName(typeof(CorrectionEnum), type));
				throw new ArgumentException(type.Name);
			}
			
			Binding binding = new Binding(
				"Checked",
				source,
				member,
				false,
				DataSourceUpdateMode.OnPropertyChanged);
			
			control.DataBindings.Add(binding);
		}
		
		public void Bind(object source, string member)
		{
			// HACK: must be true if not subsequent correction enum checkboxes would not be bound properly. 
			//       Maybe date time string parsing between DateTimePicker 
			//       (string) and model DateTime crashes?
			Binding binding = new Binding(
				"Value",
				source,
				member,
				true,
				DataSourceUpdateMode.OnPropertyChanged);
			dtpCustom.DataBindings.Add(binding);
			dtpCustom.MinDate = new DateTime(1980, 1, 1);
			dtpCustom.MaxDate = DateTime.Now;
		}
		
		void BtnRunClick(object sender, EventArgs e)
		{
			Program.Controller.Repair();
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
		
		public void AddPath(string p)
		{
			if(!pathNodes.ContainsKey(p))
			{
				TreeNode pathArgNode = new TreeNode();
				pathArgNode.Name = p;
				pathArgNode.Text = p;
				
				pathArgNode.ImageIndex = (int) TreeViewIcon.Start;
				pathArgNode.SelectedImageIndex = (int) TreeViewIcon.Start;
				
				if(rootNode != null)
				{
					rootNode.Nodes.Add(pathArgNode);
				}
				else
					log.Error("rootNode is null");
				
				pathNodes.Add(p, pathArgNode);
			}
			else
			{
				log.WarnFormat("Skipped possible duplicate path argument: {0}", p);
			}
		}
		
		public void CurrentPath(string p)
		{
			//log.InfoFormat("P: {0}", p);
			SetCurrentFile(p);
			
			if(pathNodes.ContainsKey(p))
			{
				pathNode = pathNodes[p];
				
				if(Directory.Exists(p))
					dirNode = pathNode;
				else if(File.Exists(p))
					fileNode = pathNode;
			}
			else
			{
				log.ErrorFormat("Non-existent path node: {0}", p);
			}
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
		
		/// <summary>
		/// For ProgressPropertyChangeListener to update UI
		/// </summary>
		/// <param name="progress"></param>
		public void SetProgress(int progress)
		{
			if(!toolStripProgressBar.Visible)
				toolStripProgressBar.Visible = true;
			this.toolStripProgressBar.Value = progress;
		}
		
		/// <summary>
		/// For ProgressPropertyChangeListener to update UI
		/// </summary>
		/// <param name="visit"></param>
		public void SetAction(FileVisit visit)
		{
			// Update the current path node being processed?
			if(pathNode == null && pathNodes.ContainsKey(visit.File.FullName))
			{
				pathNode = pathNodes[visit.File.FullName];
			}
			else if(pathNode != null && !visit.File.FullName.StartsWith(pathNode.Text))
			{
				string p = visit.File.FullName;
				
				do
				{
					if(p != null && pathNodes.ContainsKey(p))
						pathNode = pathNodes[p];
					else
						p = Path.GetDirectoryName(p);
				} while(p != null && !pathNodes.ContainsKey(p));
			}
			
			if(pathNode != null && pathNode.Name.Equals(visit.File.FullName))
			{
				string path = pathNode.Text;
				this.CurrentPath(path);
			}
			else if(visit.File is FileInfo)
			{
				FileInfo fi = (FileInfo) visit.File;
				this.CurrentFile(fi);
			}
			else if(visit.File is DirectoryInfo)
			{
				DirectoryInfo di = (DirectoryInfo) visit.File;
				this.CurrentDirectory(di);
			}
		}
		
		public void Done()
		{
			SetCurrentFile(string.Empty);
			rootNode.ExpandAll();
			toolStripProgressBar.Visible = false;
		}
		
		public void Error(string startPath)
		{
			MessageBox.Show(
				startPath + " is invalid.",
				"Invalid path",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
			Application.Exit();
		}
	}
}
