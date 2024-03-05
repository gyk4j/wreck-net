
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
			this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.lblCurrentFile = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnRun = new System.Windows.Forms.ToolStripButton();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.grpTimestampSources = new System.Windows.Forms.GroupBox();
			this.customSpinner = new System.Windows.Forms.DateTimePicker();
			this.cbxCustom = new System.Windows.Forms.CheckBox();
			this.cbxFileSystemAttributes = new System.Windows.Forms.CheckBox();
			this.cbxMetadataTags = new System.Windows.Forms.CheckBox();
			this.grpCorrectionTargets = new System.Windows.Forms.GroupBox();
			this.cbxLastAccessed = new System.Windows.Forms.CheckBox();
			this.cbxLastModified = new System.Windows.Forms.CheckBox();
			this.cbxCreation = new System.Windows.Forms.CheckBox();
			this.statusStrip.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.grpTimestampSources.SuspendLayout();
			this.grpCorrectionTargets.SuspendLayout();
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
									this.toolStripProgressBar,
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
			// toolStripProgressBar
			// 
			this.toolStripProgressBar.Name = "toolStripProgressBar";
			this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
			this.toolStripProgressBar.Visible = false;
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
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.grpTimestampSources);
			this.flowLayoutPanel1.Controls.Add(this.grpCorrectionTargets);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(364, 25);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(340, 395);
			this.flowLayoutPanel1.TabIndex = 4;
			// 
			// grpTimestampSources
			// 
			this.grpTimestampSources.Controls.Add(this.customSpinner);
			this.grpTimestampSources.Controls.Add(this.cbxCustom);
			this.grpTimestampSources.Controls.Add(this.cbxFileSystemAttributes);
			this.grpTimestampSources.Controls.Add(this.cbxMetadataTags);
			this.grpTimestampSources.Location = new System.Drawing.Point(3, 3);
			this.grpTimestampSources.Name = "grpTimestampSources";
			this.grpTimestampSources.Size = new System.Drawing.Size(325, 122);
			this.grpTimestampSources.TabIndex = 0;
			this.grpTimestampSources.TabStop = false;
			this.grpTimestampSources.Text = "Timestamp Sources";
			// 
			// customSpinner
			// 
			this.customSpinner.CustomFormat = "yyyy/MM/dd HH:mm:ss";
			this.customSpinner.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.customSpinner.Location = new System.Drawing.Point(13, 90);
			this.customSpinner.Name = "customSpinner";
			this.customSpinner.Size = new System.Drawing.Size(306, 20);
			this.customSpinner.TabIndex = 3;
			// 
			// cbxCustom
			// 
			this.cbxCustom.Location = new System.Drawing.Point(13, 60);
			this.cbxCustom.Name = "cbxCustom";
			this.cbxCustom.Size = new System.Drawing.Size(104, 24);
			this.cbxCustom.TabIndex = 2;
			this.cbxCustom.Text = "Custom";
			this.cbxCustom.UseVisualStyleBackColor = true;
			// 
			// cbxFileSystemAttributes
			// 
			this.cbxFileSystemAttributes.Location = new System.Drawing.Point(13, 40);
			this.cbxFileSystemAttributes.Name = "cbxFileSystemAttributes";
			this.cbxFileSystemAttributes.Size = new System.Drawing.Size(138, 24);
			this.cbxFileSystemAttributes.TabIndex = 1;
			this.cbxFileSystemAttributes.Text = "File system attributes";
			this.cbxFileSystemAttributes.UseVisualStyleBackColor = true;
			// 
			// cbxMetadataTags
			// 
			this.cbxMetadataTags.Location = new System.Drawing.Point(13, 19);
			this.cbxMetadataTags.Name = "cbxMetadataTags";
			this.cbxMetadataTags.Size = new System.Drawing.Size(104, 24);
			this.cbxMetadataTags.TabIndex = 0;
			this.cbxMetadataTags.Text = "Metadata tags";
			this.cbxMetadataTags.UseVisualStyleBackColor = true;
			// 
			// grpCorrectionTargets
			// 
			this.grpCorrectionTargets.Controls.Add(this.cbxLastAccessed);
			this.grpCorrectionTargets.Controls.Add(this.cbxLastModified);
			this.grpCorrectionTargets.Controls.Add(this.cbxCreation);
			this.grpCorrectionTargets.Location = new System.Drawing.Point(3, 131);
			this.grpCorrectionTargets.Name = "grpCorrectionTargets";
			this.grpCorrectionTargets.Size = new System.Drawing.Size(325, 115);
			this.grpCorrectionTargets.TabIndex = 1;
			this.grpCorrectionTargets.TabStop = false;
			this.grpCorrectionTargets.Text = "Correction Targets";
			// 
			// cbxLastAccessed
			// 
			this.cbxLastAccessed.Location = new System.Drawing.Point(13, 59);
			this.cbxLastAccessed.Name = "cbxLastAccessed";
			this.cbxLastAccessed.Size = new System.Drawing.Size(104, 24);
			this.cbxLastAccessed.TabIndex = 2;
			this.cbxLastAccessed.Text = "Last accessed";
			this.cbxLastAccessed.UseVisualStyleBackColor = true;
			// 
			// cbxLastModified
			// 
			this.cbxLastModified.Location = new System.Drawing.Point(13, 38);
			this.cbxLastModified.Name = "cbxLastModified";
			this.cbxLastModified.Size = new System.Drawing.Size(104, 24);
			this.cbxLastModified.TabIndex = 1;
			this.cbxLastModified.Text = "Last modified";
			this.cbxLastModified.UseVisualStyleBackColor = true;
			// 
			// cbxCreation
			// 
			this.cbxCreation.Location = new System.Drawing.Point(13, 19);
			this.cbxCreation.Name = "cbxCreation";
			this.cbxCreation.Size = new System.Drawing.Size(104, 24);
			this.cbxCreation.TabIndex = 0;
			this.cbxCreation.Text = "Creation";
			this.cbxCreation.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(704, 442);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.treeViewPaths);
			this.Name = "MainForm";
			this.Text = "WreckGui";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.grpTimestampSources.ResumeLayout(false);
			this.grpCorrectionTargets.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.CheckBox cbxCreation;
		private System.Windows.Forms.CheckBox cbxLastModified;
		private System.Windows.Forms.CheckBox cbxLastAccessed;
		private System.Windows.Forms.GroupBox grpCorrectionTargets;
		private System.Windows.Forms.DateTimePicker customSpinner;
		private System.Windows.Forms.GroupBox grpTimestampSources;
		private System.Windows.Forms.CheckBox cbxMetadataTags;
		private System.Windows.Forms.CheckBox cbxFileSystemAttributes;
		private System.Windows.Forms.CheckBox cbxCustom;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
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
