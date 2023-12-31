﻿
namespace Wreck
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.treeViewPaths = new System.Windows.Forms.TreeView();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.lblDirectoriesCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblFilesCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblSkippedCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblCurrentFile = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnRun = new System.Windows.Forms.ToolStripButton();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.statusStrip.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeViewPaths
			// 
			this.treeViewPaths.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewPaths.Location = new System.Drawing.Point(0, 0);
			this.treeViewPaths.Margin = new System.Windows.Forms.Padding(3, 128, 3, 128);
			this.treeViewPaths.Name = "treeViewPaths";
			this.treeViewPaths.Size = new System.Drawing.Size(364, 442);
			this.treeViewPaths.TabIndex = 1;
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.lblDirectoriesCount,
									this.lblFilesCount,
									this.lblSkippedCount,
									this.lblCurrentFile});
			this.statusStrip.Location = new System.Drawing.Point(364, 420);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(340, 22);
			this.statusStrip.TabIndex = 2;
			this.statusStrip.Text = "statusStrip";
			// 
			// lblDirectoriesCount
			// 
			this.lblDirectoriesCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.lblDirectoriesCount.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.lblDirectoriesCount.Name = "lblDirectoriesCount";
			this.lblDirectoriesCount.Size = new System.Drawing.Size(4, 17);
			// 
			// lblFilesCount
			// 
			this.lblFilesCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.lblFilesCount.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.lblFilesCount.Name = "lblFilesCount";
			this.lblFilesCount.Size = new System.Drawing.Size(4, 17);
			// 
			// lblSkippedCount
			// 
			this.lblSkippedCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.lblSkippedCount.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.lblSkippedCount.Name = "lblSkippedCount";
			this.lblSkippedCount.Size = new System.Drawing.Size(4, 17);
			// 
			// lblCurrentFile
			// 
			this.lblCurrentFile.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.lblCurrentFile.Name = "lblCurrentFile";
			this.lblCurrentFile.Size = new System.Drawing.Size(0, 17);
			this.lblCurrentFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.btnRun});
			this.toolStrip.Location = new System.Drawing.Point(364, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(340, 25);
			this.toolStrip.TabIndex = 3;
			this.toolStrip.Text = "toolStrip";
			// 
			// btnRun
			// 
			this.btnRun.Image = ((System.Drawing.Image)(resources.GetObject("btnRun.Image")));
			this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(48, 22);
			this.btnRun.Text = "Run";
			this.btnRun.Click += new System.EventHandler(this.BtnRunClick);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "folder.ico");
			this.imageList.Images.SetKeyName(1, "file.ico");
			this.imageList.Images.SetKeyName(2, "start.ico");
			this.imageList.Images.SetKeyName(3, "app.ico");
			this.imageList.Images.SetKeyName(4, "app_idle.ico");
			this.imageList.Images.SetKeyName(5, "app_running.ico");
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(704, 442);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.treeViewPaths);
			this.Name = "MainForm";
			this.Text = "WreckGui";
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnRun;
		private System.Windows.Forms.TreeView treeViewPaths;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel lblSkippedCount;
		private System.Windows.Forms.ToolStripStatusLabel lblFilesCount;
		private System.Windows.Forms.ToolStripStatusLabel lblDirectoriesCount;
		private System.Windows.Forms.ToolStripStatusLabel lblCurrentFile;
	}
}
