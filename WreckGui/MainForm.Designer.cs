
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.treeViewPaths = new System.Windows.Forms.TreeView();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.lblDirectoriesCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblFilesCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblSkippedCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnRun = new System.Windows.Forms.ToolStripButton();
			this.statusStrip.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeViewPaths
			// 
			this.treeViewPaths.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewPaths.Location = new System.Drawing.Point(0, 0);
			this.treeViewPaths.Margin = new System.Windows.Forms.Padding(3, 128, 3, 128);
			this.treeViewPaths.Name = "treeViewPaths";
			this.treeViewPaths.Size = new System.Drawing.Size(469, 321);
			this.treeViewPaths.TabIndex = 1;
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.lblDirectoriesCount,
									this.lblFilesCount,
									this.lblSkippedCount});
			this.statusStrip.Location = new System.Drawing.Point(0, 299);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(469, 22);
			this.statusStrip.TabIndex = 2;
			this.statusStrip.Text = "statusStrip";
			// 
			// lblDirectoriesCount
			// 
			this.lblDirectoriesCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.lblDirectoriesCount.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.lblDirectoriesCount.Name = "lblDirectoriesCount";
			this.lblDirectoriesCount.Size = new System.Drawing.Size(4, 17);
			// 
			// lblFilesCount
			// 
			this.lblFilesCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.lblFilesCount.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.lblFilesCount.Name = "lblFilesCount";
			this.lblFilesCount.Size = new System.Drawing.Size(4, 17);
			// 
			// lblSkippedCount
			// 
			this.lblSkippedCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
									| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.lblSkippedCount.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.lblSkippedCount.Name = "lblSkippedCount";
			this.lblSkippedCount.Size = new System.Drawing.Size(4, 17);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.btnRun});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(469, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
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
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(469, 321);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.treeViewPaths);
			this.Name = "MainForm";
			this.Text = "WreckGui";
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripButton btnRun;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.TreeView treeViewPaths;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel lblSkippedCount;
		private System.Windows.Forms.ToolStripStatusLabel lblFilesCount;
		private System.Windows.Forms.ToolStripStatusLabel lblDirectoriesCount;
	}
}
