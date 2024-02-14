
namespace Wreck
{
	partial class ScanningDialogForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanningDialogForm));
			this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.lblIcon = new System.Windows.Forms.Label();
			this.pgbProgress = new System.Windows.Forms.ProgressBar();
			this.lblAction = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.flowLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel
			// 
			this.flowLayoutPanel.Controls.Add(this.lblIcon);
			this.flowLayoutPanel.Controls.Add(this.pgbProgress);
			this.flowLayoutPanel.Controls.Add(this.lblAction);
			this.flowLayoutPanel.Controls.Add(this.btnCancel);
			this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Size = new System.Drawing.Size(284, 262);
			this.flowLayoutPanel.TabIndex = 0;
			// 
			// lblIcon
			// 
			this.lblIcon.AutoSize = true;
			this.lblIcon.Dock = System.Windows.Forms.DockStyle.Left;
			this.lblIcon.Image = ((System.Drawing.Image)(resources.GetObject("lblIcon.Image")));
			this.lblIcon.Location = new System.Drawing.Point(8, 8);
			this.lblIcon.Margin = new System.Windows.Forms.Padding(8);
			this.lblIcon.MinimumSize = new System.Drawing.Size(32, 32);
			this.lblIcon.Name = "lblIcon";
			this.lblIcon.Size = new System.Drawing.Size(32, 32);
			this.lblIcon.TabIndex = 0;
			
			// 
			// pgbProgress
			// 
			this.pgbProgress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgbProgress.Location = new System.Drawing.Point(3, 51);
			this.pgbProgress.Name = "pgbProgress";
			this.pgbProgress.Size = new System.Drawing.Size(75, 23);
			this.pgbProgress.TabIndex = 1;
			// 
			// lblAction
			// 
			this.lblAction.AutoSize = true;
			this.lblAction.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblAction.Location = new System.Drawing.Point(3, 77);
			this.lblAction.Name = "lblAction";
			this.lblAction.Size = new System.Drawing.Size(75, 13);
			this.lblAction.TabIndex = 2;
			this.lblAction.Text = "Action";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.AutoSize = true;
			this.btnCancel.Location = new System.Drawing.Point(3, 93);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// ScanningDialogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.flowLayoutPanel);
			this.Name = "ScanningDialogForm";
			this.Text = "ScanningDialogForm";
			this.flowLayoutPanel.ResumeLayout(false);
			this.flowLayoutPanel.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblAction;
		private System.Windows.Forms.ProgressBar pgbProgress;
		private System.Windows.Forms.Label lblIcon;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
	}
}
