
using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Wreck.Logging
{
	/// <summary>
	/// Description of Logger.
	/// </summary>
	public class Logger : ILogger
	{
		private MainForm form;
		
		private TreeNode pathNode;
		private TreeNode dirNode;
		private TreeNode fileNode;
		
		public Logger(MainForm form)
		{
			this.form = form;
		}
		
		public void Version()
		{
			this.form.Text = String.Format("{0} v{1}", Wreck.NAME, Wreck.VERSION);
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
			pathNode = new TreeNode();
			pathNode.Name = p;
			pathNode.Text = p;
			
			TreeNode rootNode = this.form.getRootNode();
			rootNode.Nodes.Add(pathNode);
//			pathNode.ExpandAll();
//			this.form.getTreeView().ExpandAll();
		}
		
		public void CurrentFile(FileInfo f)
		{
			fileNode = new TreeNode();
			fileNode.Name = f.FullName;
			fileNode.Text = f.Name;
			
			dirNode.Nodes.Add(fileNode);
//			dirNode.ExpandAll();
//			this.form.getTreeView().ExpandAll();
		}
		
		public void CurrentDirectory(DirectoryInfo d)
		{
			dirNode = new TreeNode();
			dirNode.Name = d.FullName;
			dirNode.Text = d.FullName;
			
			pathNode.Nodes.Add(dirNode);
//			dirNode.ExpandAll();
//			this.form.getTreeView().ExpandAll();
		}
		
		public void SkipReparsePoint(DirectoryInfo d)
		{
			Debug.Print("Skipped reparse point: {0}", d.Name);
		}
		
		public void SkipReparsePoint(FileInfo f)
		{
			Debug.Print("Skipped reparse point: {0}", f.Name);
		}
		
		// For Corrector
		public void CorrectedByLastWriteMetadata(FileSystemInfo fsi, DateTime lastWrite)
		{
			Debug.Print("WM: {0} : {1} -> {2}", fsi.Name, fsi.LastWriteTime, lastWrite);
		}
		
		public void CorrectedByCreationMetadata(FileSystemInfo fsi, DateTime creation)
		{
			Debug.Print("CM: {0} : {1} -> {2}", fsi.Name, fsi.CreationTime, creation);
		}
		
		public void CorrectedByLastAccessMetadata(FileSystemInfo fsi, DateTime lastAccess)
		{
			Debug.Print("AM: {0} : {1} -> {2}", fsi.Name, fsi.LastAccessTime, lastAccess);
		}
		
		public void CorrectedByLastWriteTime(FileSystemInfo fsi)
		{
			Debug.Print("AM: {0} : {1} -> {2}", fsi.Name, fsi.CreationTime, fsi.LastWriteTime);
			Debug.Print("AM: {0} : {1} -> {2}", fsi.Name, fsi.LastAccessTime, fsi.LastWriteTime);
		}
		
		public void Statistics(Statistics stats)
		{
			ToolStripItem lblDirectoriesCount = this.form.getStatusStrip().Items["lblDirectoriesCount"];
			lblDirectoriesCount.Text = string.Format("Directories: {0}", stats.Directories);
			
			ToolStripItem lblFilesCount = this.form.getStatusStrip().Items["lblFilesCount"];
			lblFilesCount.Text = string.Format("Files: {0}", stats.Files);
			
			ToolStripItem lblSkippedCount = this.form.getStatusStrip().Items["lblSkippedCount"];
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
